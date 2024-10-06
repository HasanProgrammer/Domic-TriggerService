using Domic.Core.Domain.Constants;
using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.TicketService;

public class User
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Const
        const string Ticket_User_Exchange_Retry_1 = "Ticket_User_Exchange_Retry_1";
        const string Ticket_User_Exchange_Retry_2 = "Ticket_User_Exchange_Retry_2";
        const string Ticket_User_Queue            = "Ticket_User_Queue";
        const string Ticket_User_Queue_Retry      = "Ticket_User_Queue_Retry";
        
        //Retry exchange
        channel.FanOutExchangeDeclare(Ticket_User_Exchange_Retry_1);
        channel.FanOutExchangeDeclare(Ticket_User_Exchange_Retry_2);
        
        //Main queue
        channel.QueueDeclare(Ticket_User_Queue, new Dictionary<string, object> {
            { "x-delayed-type"         , "fanout" },
            { "x-dead-letter-exchange" , Ticket_User_Exchange_Retry_1 }
        });
    
        //Retry queue
        channel.QueueDeclare(Ticket_User_Queue_Retry, new Dictionary<string, object> {
            { "x-message-ttl"          , 5000 }     , //5s
            { "x-delayed-type"         , "fanout" } ,
            { "x-dead-letter-exchange" , Ticket_User_Exchange_Retry_2 }
        });
        
        //Binding
        channel.BindQueueToFanOutExchange(Broker.User_User_Exchange    , Ticket_User_Queue);
        channel.BindQueueToFanOutExchange(Ticket_User_Exchange_Retry_1 , Ticket_User_Queue_Retry);
        channel.BindQueueToFanOutExchange(Ticket_User_Exchange_Retry_2 , Ticket_User_Queue);
    }
}