using Domic.Core.Domain.Constants;
using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.AggregateArticleService;

public class ArticleCommentAnswer
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Retry exchange
        channel.FanOutExchangeDeclare(Broker.AggregateArticle_ArticleCommentAnswer_Exchange_Retry_1);
        channel.FanOutExchangeDeclare(Broker.AggregateArticle_ArticleCommentAnswer_Exchange_Retry_2);
        
        //Main queue
        channel.QueueDeclare(Broker.AggregateArticle_ArticleCommentAnswer_Queue, new Dictionary<string, object> {
            { "x-delayed-type"         , "fanout" },
            { "x-dead-letter-exchange" , Broker.AggregateArticle_ArticleCommentAnswer_Exchange_Retry_1 }
        });
    
        //Retry queue
        channel.QueueDeclare(Broker.AggregateArticle_ArticleCommentAnswer_Queue_Retry, new Dictionary<string, object> {
            { "x-message-ttl"          , 5000 }     , //5s
            { "x-delayed-type"         , "fanout" } ,
            { "x-dead-letter-exchange" , Broker.AggregateArticle_ArticleCommentAnswer_Exchange_Retry_2 }
        });
        
        //Binding
        channel.BindQueueToFanOutExchange(Broker.Comment_ArticleCommentAnswer_Exchange                  , Broker.AggregateArticle_ArticleCommentAnswer_Queue);
        channel.BindQueueToFanOutExchange(Broker.AggregateArticle_ArticleCommentAnswer_Exchange_Retry_1 , Broker.AggregateArticle_ArticleCommentAnswer_Queue_Retry);
        channel.BindQueueToFanOutExchange(Broker.AggregateArticle_ArticleCommentAnswer_Exchange_Retry_2 , Broker.AggregateArticle_ArticleCommentAnswer_Queue);
    }
}