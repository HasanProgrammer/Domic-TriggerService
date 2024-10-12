using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.CommentService;

public class Term
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Const
        const string Comment_Term_Exchange_Retry_1 = "Comment_Term_Exchange_Retry_1";
        const string Comment_Term_Exchange_Retry_2 = "Comment_Term_Exchange_Retry_2";
        const string Comment_Term_Queue            = "Comment_Term_Queue";
        const string Comment_Term_Queue_Retry      = "Comment_Term_Queue_Retry";
        
        //Retry exchange
        channel.FanOutExchangeDeclare(Comment_Term_Exchange_Retry_1);
        channel.FanOutExchangeDeclare(Comment_Term_Exchange_Retry_2);
        
        //Main queue
        channel.QueueDeclare(Comment_Term_Queue, new Dictionary<string, object> {
            { "x-delayed-type"         , "fanout" },
            { "x-dead-letter-exchange" , Comment_Term_Exchange_Retry_1 }
        });
    
        //Retry queue
        channel.QueueDeclare(Comment_Term_Queue_Retry, new Dictionary<string, object> {
            { "x-message-ttl"          , 5000 }     , //5s
            { "x-delayed-type"         , "fanout" } ,
            { "x-dead-letter-exchange" , Comment_Term_Exchange_Retry_2 }
        });
        
        //Binding
        channel.BindQueueToFanOutExchange("Term_Term_Exchange"          , Comment_Term_Queue);
        channel.BindQueueToFanOutExchange(Comment_Term_Exchange_Retry_1 , Comment_Term_Queue_Retry);
        channel.BindQueueToFanOutExchange(Comment_Term_Exchange_Retry_2 , Comment_Term_Queue);
    }
}