using Domic.Core.Domain.Constants;
using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.DiscoveryService;

public class ServiceRegistry
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        channel.DirectExchangeDeclare(Broker.ServiceRegistry_Exchange);
        channel.FanOutExchangeDeclare(Broker.ServiceRegistry_Exchange_Retry_1);
        channel.FanOutExchangeDeclare(Broker.ServiceRegistry_Exchange_Retry_2);
    
        channel.QueueDeclare(Broker.ServiceRegistry_Queue, new Dictionary<string, object> {
            { "x-delayed-type"         , "fanout" },
            { "x-dead-letter-exchange" , Broker.ServiceRegistry_Exchange_Retry_1 }
        });
    
        channel.QueueDeclare(Broker.ServiceRegistry_Queue_Retry, new Dictionary<string, object> {
            { "x-message-ttl"          , 5000 }     , //5s
            { "x-delayed-type"         , "fanout" } ,
            { "x-dead-letter-exchange" , Broker.ServiceRegistry_Exchange_Retry_2 }
        });
    
        channel.BindQueueToDirectExchange(Broker.ServiceRegistry_Exchange , Broker.ServiceRegistry_Queue, 
            Broker.ServiceRegistry_Route
        );
    
        channel.BindQueueToFanOutExchange(Broker.ServiceRegistry_Exchange_Retry_1 , Broker.ServiceRegistry_Queue_Retry);
        channel.BindQueueToFanOutExchange(Broker.ServiceRegistry_Exchange_Retry_2 , Broker.ServiceRegistry_Queue);
    }
}