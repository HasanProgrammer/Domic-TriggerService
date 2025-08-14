using Domic.Core.Domain.Constants;
using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.StateTrackerService;

public class Event
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Retry exchange
        channel.FanOutExchangeDeclare(Broker.StateTracker_Event_Exchange_Retry_1);
        channel.FanOutExchangeDeclare(Broker.StateTracker_Event_Exchange_Retry_2);
        
        //Main queue
        channel.QueueDeclare(Broker.StateTracker_Event_Queue, new Dictionary<string, object> {
            { "x-delayed-type"         , "fanout" },
            { "x-dead-letter-exchange" , Broker.StateTracker_Event_Exchange_Retry_1 }
        });
    
        //Retry queue
        channel.QueueDeclare(Broker.StateTracker_Event_Queue_Retry, new Dictionary<string, object> {
            { "x-message-ttl"          , 5000 }     , //5s
            { "x-delayed-type"         , "fanout" } ,
            { "x-dead-letter-exchange" , Broker.StateTracker_Event_Exchange_Retry_2 }
        });
        
        //Binding
        channel.BindQueueToFanOutExchange(Broker.User_Role_Exchange                    , Broker.StateTracker_Event_Queue);
        channel.BindQueueToFanOutExchange(Broker.User_User_Exchange                    , Broker.StateTracker_Event_Queue);
        channel.BindQueueToFanOutExchange(Broker.User_Permission_Exchange              , Broker.StateTracker_Event_Queue);
        channel.BindQueueToFanOutExchange(Broker.Category_Category_Exchange            , Broker.StateTracker_Event_Queue);
        channel.BindQueueToFanOutExchange(Broker.Article_Article_Exchange              , Broker.StateTracker_Event_Queue);
        channel.BindQueueToFanOutExchange("Term_Term_Exchange"                         , Broker.StateTracker_Event_Queue);
        channel.BindQueueToFanOutExchange("Term_Video_Exchange"                        , Broker.StateTracker_Event_Queue);
        channel.BindQueueToFanOutExchange(Broker.Comment_ArticleComment_Exchange       , Broker.StateTracker_Event_Queue);
        channel.BindQueueToFanOutExchange(Broker.Comment_ArticleCommentAnswer_Exchange , Broker.StateTracker_Event_Queue);
        channel.BindQueueToFanOutExchange("Comment_TermComment_Exchange"               , Broker.StateTracker_Event_Queue);
        channel.BindQueueToFanOutExchange("Comment_TermCommentAnswer_Exchange"         , Broker.StateTracker_Event_Queue);
        channel.BindQueueToFanOutExchange("Ticket_Ticket_Exchange"                     , Broker.StateTracker_Event_Queue);
        channel.BindQueueToFanOutExchange("Ticket_TicketComment_Exchange"              , Broker.StateTracker_Event_Queue);
        channel.BindQueueToFanOutExchange(Broker.StateTracker_Event_Exchange_Retry_1   , Broker.StateTracker_Event_Queue_Retry);
        channel.BindQueueToFanOutExchange(Broker.StateTracker_Event_Exchange_Retry_2   , Broker.StateTracker_Event_Queue);
    }
}