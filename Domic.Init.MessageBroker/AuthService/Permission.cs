using Domic.Core.Domain.Constants;
using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.AuthService;

public class Permission
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Retry exchange
        channel.FanOutExchangeDeclare(Broker.Auth_Permission_Exchange_Retry_1);
        channel.FanOutExchangeDeclare(Broker.Auth_Permission_Exchange_Retry_2);
        
        //Main queue
        channel.QueueDeclare(Broker.Auth_Permission_Queue, new Dictionary<string, object> {
            { "x-delayed-type"         , "fanout" },
            { "x-dead-letter-exchange" , Broker.Auth_Permission_Exchange_Retry_1 }
        });
    
        //Retry queue
        channel.QueueDeclare(Broker.Auth_Permission_Queue_Retry, new Dictionary<string, object> {
            { "x-message-ttl"          , 5000 }     , //5s
            { "x-delayed-type"         , "fanout" } ,
            { "x-dead-letter-exchange" , Broker.Auth_Permission_Exchange_Retry_2 }
        });
        
        //Binding
        channel.BindQueueToFanOutExchange(Broker.User_Permission_Exchange         , Broker.Auth_Permission_Queue);
        channel.BindQueueToFanOutExchange(Broker.Auth_Permission_Exchange_Retry_1 , Broker.Auth_Permission_Queue_Retry);
        channel.BindQueueToFanOutExchange(Broker.Auth_Permission_Exchange_Retry_2 , Broker.Auth_Permission_Queue);
    }
}