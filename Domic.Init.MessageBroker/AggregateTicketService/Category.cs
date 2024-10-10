using Domic.Core.Domain.Constants;
using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.AggregateTicketService;

public class Category
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Const
        const string AggregateTicket_Category_Exchange_Retry_1 = "AggregateTicket_Category_Exchange_Retry_1";
        const string AggregateTicket_Category_Exchange_Retry_2 = "AggregateTicket_Category_Exchange_Retry_2";
        const string AggregateTicket_Category_Queue            = "AggregateTicket_Category_Queue";
        const string AggregateTicket_Category_Queue_Retry      = "AggregateTicket_Category_Queue_Retry";
        
        //Retry exchange
        channel.FanOutExchangeDeclare(AggregateTicket_Category_Exchange_Retry_1);
        channel.FanOutExchangeDeclare(AggregateTicket_Category_Exchange_Retry_2);
        
        //Main queue
        channel.QueueDeclare(AggregateTicket_Category_Queue, new Dictionary<string, object> {
            { "x-delayed-type"         , "fanout" },
            { "x-dead-letter-exchange" , AggregateTicket_Category_Exchange_Retry_1 }
        });
    
        //Retry queue
        channel.QueueDeclare(AggregateTicket_Category_Queue_Retry, new Dictionary<string, object> {
            { "x-message-ttl"          , 5000 }     , //5s
            { "x-delayed-type"         , "fanout" } ,
            { "x-dead-letter-exchange" , AggregateTicket_Category_Exchange_Retry_2 }
        });
        
        //Binding
        channel.BindQueueToFanOutExchange(Broker.Category_Category_Exchange         , AggregateTicket_Category_Queue);
        channel.BindQueueToFanOutExchange(AggregateTicket_Category_Exchange_Retry_1 , AggregateTicket_Category_Queue_Retry);
        channel.BindQueueToFanOutExchange(AggregateTicket_Category_Exchange_Retry_2 , AggregateTicket_Category_Queue);
    }
}