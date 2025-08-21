using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.TermService;

public class Term
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Main exchange
        channel.FanOutExchangeDeclare("Term_Term_Exchange");
        
        //Retry exchange
        channel.FanOutExchangeDeclare("Term_Book_Exchange_Retry_1");
        channel.FanOutExchangeDeclare("Term_Book_Exchange_Retry_2");

        //Main queue
        channel.QueueDeclare("Term_Book_Queue", new Dictionary<string, object> {
            { "x-delayed-type", "fanout" },
            { "x-dead-letter-exchange", "Term_Book_Exchange_Retry_1" }
        });

        //Retry queue
        channel.QueueDeclare("Term_Book_Queue_Retry", new Dictionary<string, object> {
            { "x-message-ttl", 5000 }, //5s
            { "x-delayed-type", "fanout" },
            { "x-dead-letter-exchange", "Term_Book_Exchange_Retry_2" }
        });

        //Binding
        channel.BindQueueToFanOutExchange("Financial_Account_Exchange" , "Term_Book_Queue");
        channel.BindQueueToFanOutExchange("Term_Book_Exchange_Retry_1" , "Term_Book_Queue_Retry");
        channel.BindQueueToFanOutExchange("Term_Book_Exchange_Retry_2" , "Term_Book_Queue");
    }
}