using Domic.Core.Domain.Constants;
using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.StateTrackerService;

public class Request
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Main exchange
        channel.DirectExchangeDeclare(Broker.Request_Exchange);
        
        //Retry exchange
        channel.FanOutExchangeDeclare(Broker.StateTracker_Request_Exchange_Retry_1);
        channel.FanOutExchangeDeclare(Broker.StateTracker_Request_Exchange_Retry_2);

        //Main queue
        channel.QueueDeclare(Broker.StateTracker_Request_Queue, new Dictionary<string, object> {
            { "x-delayed-type"         , "fanout" },
            { "x-dead-letter-exchange" , Broker.StateTracker_Request_Exchange_Retry_1 }
        });
    
        //Retry queue
        channel.QueueDeclare(Broker.StateTracker_Request_Queue_Retry, new Dictionary<string, object> {
            { "x-message-ttl"          , 5000 }     , //5s
            { "x-delayed-type"         , "fanout" } ,
            { "x-dead-letter-exchange" , Broker.StateTracker_Request_Exchange_Retry_2 }
        });
        
        //Binding
        channel.BindQueueToDirectExchange(Broker.Request_Exchange, Broker.StateTracker_Request_Queue, Broker.StateTracker_Request_Route);
        channel.BindQueueToFanOutExchange(Broker.StateTracker_Request_Exchange_Retry_1, Broker.StateTracker_Request_Queue_Retry);
        channel.BindQueueToFanOutExchange(Broker.StateTracker_Request_Exchange_Retry_2, Broker.StateTracker_Request_Queue);
    }
}