using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.AggregateTermService;

public class Term
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Retry exchange
        channel.FanOutExchangeDeclare("AggregateTerm_Term_Exchange_Retry_1");
        channel.FanOutExchangeDeclare("AggregateTerm_Term_Exchange_Retry_2");

        //Main queue
        channel.QueueDeclare("AggregateTerm_Term_Queue", new Dictionary<string, object> {
            { "x-delayed-type", "fanout" },
            { "x-dead-letter-exchange", "AggregateTerm_Term_Exchange_Retry_1" }
        });

        //Retry queue
        channel.QueueDeclare("AggregateTerm_Term_Queue_Retry", new Dictionary<string, object> {
            { "x-message-ttl", 5000 }, //5s
            { "x-delayed-type", "fanout" },
            { "x-dead-letter-exchange", "AggregateTerm_Term_Exchange_Retry_2" }
        });

        //Binding
        channel.BindQueueToFanOutExchange("Term_Term_Exchange", "AggregateTerm_Term_Queue");
        channel.BindQueueToFanOutExchange("AggregateTerm_Term_Exchange_Retry_1", "AggregateTerm_Term_Queue_Retry");
        channel.BindQueueToFanOutExchange("AggregateTerm_Term_Exchange_Retry_2", "AggregateTerm_Term_Queue");
    }
}