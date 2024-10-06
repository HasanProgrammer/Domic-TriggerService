using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.AggregateFinancialService;

public class Account
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Const
        const string AggregateFinancial_Account_Exchange_Retry_1 = "AggregateFinancial_Account_Exchange_Retry_1";
        const string AggregateFinancial_Account_Exchange_Retry_2 = "AggregateFinancial_Account_Exchange_Retry_2";
        const string AggregateFinancial_Account_Queue            = "AggregateFinancial_Account_Queue";
        const string AggregateFinancial_Account_Queue_Retry      = "AggregateFinancial_Account_Queue_Retry";
        
        //Retry exchange
        channel.FanOutExchangeDeclare(AggregateFinancial_Account_Exchange_Retry_1);
        channel.FanOutExchangeDeclare(AggregateFinancial_Account_Exchange_Retry_2);
        
        //Main queue
        channel.QueueDeclare(AggregateFinancial_Account_Queue, new Dictionary<string, object> {
            { "x-delayed-type"         , "fanout" },
            { "x-dead-letter-exchange" , AggregateFinancial_Account_Exchange_Retry_1 }
        });
    
        //Retry queue
        channel.QueueDeclare(AggregateFinancial_Account_Queue_Retry, new Dictionary<string, object> {
            { "x-message-ttl"          , 5000 }     , //5s
            { "x-delayed-type"         , "fanout" } ,
            { "x-dead-letter-exchange" , AggregateFinancial_Account_Exchange_Retry_2 }
        });
        
        //Binding
        channel.BindQueueToFanOutExchange("Financial_Account_Exchange"                , AggregateFinancial_Account_Queue);
        channel.BindQueueToFanOutExchange(AggregateFinancial_Account_Exchange_Retry_1 , AggregateFinancial_Account_Queue_Retry);
        channel.BindQueueToFanOutExchange(AggregateFinancial_Account_Exchange_Retry_2 , AggregateFinancial_Account_Queue);
    }
}