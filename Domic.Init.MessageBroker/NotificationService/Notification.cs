using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.NotificationService;

public class Notification
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Main queue
        channel.QueueDeclare("Notification_OtpLog_Queue", new Dictionary<string, object> {
            { "x-delayed-type"         , "fanout" },
            { "x-dead-letter-exchange" , "Notification_Exchange_Retry_1" }
        });
    
        //Retry queue
        channel.QueueDeclare("Notification_OtpLog_Queue_Retry", new Dictionary<string, object> {
            { "x-message-ttl"          , 5000 }     , //5s
            { "x-delayed-type"         , "fanout" } ,
            { "x-dead-letter-exchange" , "Notification_Exchange_Retry_2" }
        });
        
        //Binding
        channel.BindQueueToFanOutExchange("Identity_OtpLog_Exchange"      , "Notification_OtpLog_Queue");
        channel.BindQueueToFanOutExchange("Notification_Exchange_Retry_1" , "Notification_OtpLog_Queue_Retry");
        channel.BindQueueToFanOutExchange("Notification_Exchange_Retry_2" , "Notification_OtpLog_Queue");
    }
}