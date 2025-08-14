using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.AggregateTermService;

public class TermCommentAnswer
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Retry exchange
        channel.FanOutExchangeDeclare("AggregateTerm_TermCommentAnswer_Exchange_Retry_1");
        channel.FanOutExchangeDeclare("AggregateTerm_TermCommentAnswer_Exchange_Retry_2");
        
        //Main queue
        channel.QueueDeclare("AggregateTerm_TermCommentAnswer_Queue", new Dictionary<string, object> {
            { "x-delayed-type"         , "fanout" },
            { "x-dead-letter-exchange" , "AggregateTerm_TermCommentAnswer_Exchange_Retry_1" }
        });
    
        //Retry queue
        channel.QueueDeclare("AggregateTerm_TermCommentAnswer_Queue_Retry", new Dictionary<string, object> {
            { "x-message-ttl"          , 5000 }     , //5s
            { "x-delayed-type"         , "fanout" } ,
            { "x-dead-letter-exchange" , "AggregateTerm_TermCommentAnswer_Exchange_Retry_2" }
        });
        
        //Binding
        channel.BindQueueToFanOutExchange("Comment_TermCommentAnswer_Exchange"               , "AggregateTerm_TermCommentAnswer_Queue");
        channel.BindQueueToFanOutExchange("AggregateTerm_TermCommentAnswer_Exchange_Retry_1" , "AggregateTerm_TermCommentAnswer_Queue_Retry");
        channel.BindQueueToFanOutExchange("AggregateTerm_TermCommentAnswer_Exchange_Retry_2" , "AggregateTerm_TermCommentAnswer_Queue");
    }
}