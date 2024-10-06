using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.AggregateTicketService;

public class TicketComment
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Const
        const string AggregateTicket_TicketComment_Exchange_Retry_1 = "AggregateTicket_TicketComment_Exchange_Retry_1";
        const string AggregateTicket_TicketComment_Exchange_Retry_2 = "AggregateTicket_TicketComment_Exchange_Retry_2";
        const string AggregateTicket_TicketComment_Queue            = "AggregateTicket_TicketComment_Queue";
        const string AggregateTicket_TicketComment_Queue_Retry      = "AggregateTicket_TicketComment_Queue_Retry";
        
        //Retry exchange
        channel.FanOutExchangeDeclare(AggregateTicket_TicketComment_Exchange_Retry_1);
        channel.FanOutExchangeDeclare(AggregateTicket_TicketComment_Exchange_Retry_2);
        
        //Main queue
        channel.QueueDeclare(AggregateTicket_TicketComment_Queue, new Dictionary<string, object> {
            { "x-delayed-type"         , "fanout" },
            { "x-dead-letter-exchange" , AggregateTicket_TicketComment_Exchange_Retry_1 }
        });
    
        //Retry queue
        channel.QueueDeclare(AggregateTicket_TicketComment_Queue_Retry, new Dictionary<string, object> {
            { "x-message-ttl"          , 5000 }     , //5s
            { "x-delayed-type"         , "fanout" } ,
            { "x-dead-letter-exchange" , AggregateTicket_TicketComment_Exchange_Retry_2 }
        });
        
        //Binding
        channel.BindQueueToFanOutExchange("Ticket_TicketComment_Exchange"                , AggregateTicket_TicketComment_Queue);
        channel.BindQueueToFanOutExchange(AggregateTicket_TicketComment_Exchange_Retry_1 , AggregateTicket_TicketComment_Queue_Retry);
        channel.BindQueueToFanOutExchange(AggregateTicket_TicketComment_Exchange_Retry_2 , AggregateTicket_TicketComment_Queue);
    }
}