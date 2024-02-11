using Domic.Core.Domain.Constants;
using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.StateTrackerService;

public class Log
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Main exchange
        channel.DirectExchangeDeclare(Broker.Log_Exchange);
        
        //Retry exchange
        channel.FanOutExchangeDeclare("StateTracker_Log_Exchange_Retry_1");
        channel.FanOutExchangeDeclare("StateTracker_Log_Exchange_Retry_2");
        
        //Main queue
        channel.QueueDeclare(Broker.StateTracker_Log_Queue, new Dictionary<string, object> {
            { "x-delayed-type"         , "fanout" },
            { "x-dead-letter-exchange" , "StateTracker_Log_Exchange_Retry_1" }
        });
    
        //Retry queue
        channel.QueueDeclare(Broker.StateTracker_Log_Queue_Retry, new Dictionary<string, object> {
            { "x-message-ttl"          , 5000 }     , //5s
            { "x-delayed-type"         , "fanout" } ,
            { "x-dead-letter-exchange" , "StateTracker_Log_Exchange_Retry_2" }
        });
        
        //Binding
        channel.BindQueueToDirectExchange(Broker.Log_Exchange, Broker.StateTracker_Log_Queue, Broker.StateTracker_Log_Route);
        channel.BindQueueToFanOutExchange("StateTracker_Log_Exchange_Retry_1", Broker.StateTracker_Log_Queue_Retry);
        channel.BindQueueToFanOutExchange("StateTracker_Log_Exchange_Retry_2", Broker.StateTracker_Log_Queue);
    }
}