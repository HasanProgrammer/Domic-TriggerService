using Domic.Core.Domain.Constants;
using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.UserService;

public class User
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Main exchange
        channel.FanOutExchangeDeclare(Broker.User_User_Exchange);
        
        //Retry exchange
        channel.FanOutExchangeDeclare(Broker.User_User_Exchange_Retry_1);
        channel.FanOutExchangeDeclare(Broker.User_User_Exchange_Retry_2);
        
        //Main queue
        channel.QueueDeclare(Broker.User_User_Queue, new Dictionary<string, object> {
            { "x-delayed-type"         , "fanout" },
            { "x-dead-letter-exchange" , Broker.User_User_Exchange_Retry_1 }
        });
    
        //Retry queue
        channel.QueueDeclare(Broker.User_User_Queue_Retry, new Dictionary<string, object> {
            { "x-message-ttl"          , 5000 }     , //5s
            { "x-delayed-type"         , "fanout" } ,
            { "x-dead-letter-exchange" , Broker.User_User_Exchange_Retry_2 }
        });
        
        //Binding
        channel.BindQueueToFanOutExchange(Broker.User_User_Exchange         , Broker.User_User_Queue);
        channel.BindQueueToFanOutExchange(Broker.User_User_Exchange_Retry_1 , Broker.User_User_Queue_Retry);
        channel.BindQueueToFanOutExchange(Broker.User_User_Exchange_Retry_2 , Broker.User_User_Queue);
    }
}