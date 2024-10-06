using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.AggregateFinancialService;

public class GiftTransaction
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Const
        const string AggregateFinancial_GiftTransaction_Exchange_Retry_1 = "AggregateFinancial_GiftTransaction_Exchange_Retry_1";
        const string AggregateFinancial_GiftTransaction_Exchange_Retry_2 = "AggregateFinancial_GiftTransaction_Exchange_Retry_2";
        const string AggregateFinancial_GiftTransaction_Queue            = "AggregateFinancial_GiftTransaction_Queue";
        const string AggregateFinancial_GiftTransaction_Queue_Retry      = "AggregateFinancial_GiftTransaction_Queue_Retry";
        
        //Retry exchange
        channel.FanOutExchangeDeclare(AggregateFinancial_GiftTransaction_Exchange_Retry_1);
        channel.FanOutExchangeDeclare(AggregateFinancial_GiftTransaction_Exchange_Retry_2);
        
        //Main queue
        channel.QueueDeclare(AggregateFinancial_GiftTransaction_Queue, new Dictionary<string, object> {
            { "x-delayed-type"         , "fanout" },
            { "x-dead-letter-exchange" , AggregateFinancial_GiftTransaction_Exchange_Retry_1 }
        });
    
        //Retry queue
        channel.QueueDeclare(AggregateFinancial_GiftTransaction_Queue_Retry, new Dictionary<string, object> {
            { "x-message-ttl"          , 5000 }     , //5s
            { "x-delayed-type"         , "fanout" } ,
            { "x-dead-letter-exchange" , AggregateFinancial_GiftTransaction_Exchange_Retry_2 }
        });
        
        //Binding
        channel.BindQueueToFanOutExchange("Financial_GiftTransaction_Exchange"                , AggregateFinancial_GiftTransaction_Queue);
        channel.BindQueueToFanOutExchange(AggregateFinancial_GiftTransaction_Exchange_Retry_1 , AggregateFinancial_GiftTransaction_Queue_Retry);
        channel.BindQueueToFanOutExchange(AggregateFinancial_GiftTransaction_Exchange_Retry_2 , AggregateFinancial_GiftTransaction_Queue);
    }
}