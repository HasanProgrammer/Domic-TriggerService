using Domic.Core.Domain.Constants;
using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.FinancialService;

public class User
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Const
        const string Financial_User_Exchange_Retry_1 = "Financial_User_Exchange_Retry_1";
        const string Financial_User_Exchange_Retry_2 = "Financial_User_Exchange_Retry_2";
        const string Financial_User_Queue            = "Financial_User_Queue";
        const string Financial_User_Queue_Retry      = "Financial_User_Queue_Retry";
        
        //Retry exchange
        channel.FanOutExchangeDeclare(Financial_User_Exchange_Retry_1);
        channel.FanOutExchangeDeclare(Financial_User_Exchange_Retry_2);
        
        //Main queue
        channel.QueueDeclare(Financial_User_Queue, new Dictionary<string, object> {
            { "x-delayed-type"         , "fanout" },
            { "x-dead-letter-exchange" , Financial_User_Exchange_Retry_1 }
        });
    
        //Retry queue
        channel.QueueDeclare(Financial_User_Queue_Retry, new Dictionary<string, object> {
            { "x-message-ttl"          , 5000 }     , //5s
            { "x-delayed-type"         , "fanout" } ,
            { "x-dead-letter-exchange" , Financial_User_Exchange_Retry_2 }
        });
        
        //Binding
        channel.BindQueueToFanOutExchange(Broker.User_User_Exchange       , Financial_User_Queue);
        channel.BindQueueToFanOutExchange(Financial_User_Exchange_Retry_1 , Financial_User_Queue_Retry);
        channel.BindQueueToFanOutExchange(Financial_User_Exchange_Retry_2 , Financial_User_Queue);
    }
}