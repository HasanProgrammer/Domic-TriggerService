using Domic.Core.Domain.Constants;
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
        channel.FanOutExchangeDeclare("Notification_EmailDelivery_Exchange");
        
        //Retry exchange
        channel.FanOutExchangeDeclare("Notification_Exchange_Retry_1");
        channel.FanOutExchangeDeclare("Notification_Exchange_Retry_2");
        
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
        channel.BindQueueToFanOutExchange("Notification_EmailDelivery_Exchange" , Broker.User_User_Queue);
        channel.BindQueueToFanOutExchange("Identity_OtpLog_Exchange"            , "Notification_OtpLog_Queue");
        channel.BindQueueToFanOutExchange("Notification_Exchange_Retry_1"       , "Notification_OtpLog_Queue_Retry");
        channel.BindQueueToFanOutExchange("Notification_Exchange_Retry_2"       , "Notification_OtpLog_Queue");
    }
}