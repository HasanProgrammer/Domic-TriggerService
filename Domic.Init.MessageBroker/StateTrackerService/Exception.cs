using Domic.Core.Domain.Constants;
using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.StateTrackerService;

public class Exception
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Main exchange
        channel.DirectExchangeDeclare(Broker.Exception_Exchange);
        
        //Retry exchange
        channel.FanOutExchangeDeclare(Broker.StateTracker_Exception_Exchange_Retry_1);
        channel.FanOutExchangeDeclare(Broker.StateTracker_Exception_Exchange_Retry_2);

        //Main queue
        channel.QueueDeclare(Broker.StateTracker_Exception_Queue, new Dictionary<string, object> {
            { "x-delayed-type"         , "fanout" },
            { "x-dead-letter-exchange" , Broker.StateTracker_Exception_Exchange_Retry_1 }
        });
    
        //Retry queue
        channel.QueueDeclare(Broker.StateTracker_Exception_Queue_Retry, new Dictionary<string, object> {
            { "x-message-ttl"          , 5000 }     , //5s
            { "x-delayed-type"         , "fanout" } ,
            { "x-dead-letter-exchange" , Broker.StateTracker_Exception_Exchange_Retry_2 }
        });
        
        //Binding
        channel.BindQueueToDirectExchange(Broker.Exception_Exchange, Broker.StateTracker_Exception_Queue, Broker.StateTracker_Exception_Route);
        channel.BindQueueToFanOutExchange(Broker.StateTracker_Exception_Exchange_Retry_1, Broker.StateTracker_Exception_Queue_Retry);
        channel.BindQueueToFanOutExchange(Broker.StateTracker_Exception_Exchange_Retry_2, Broker.StateTracker_Exception_Queue);
    }
}