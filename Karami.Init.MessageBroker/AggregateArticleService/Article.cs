﻿using Karami.Core.Domain.Constants;
using Karami.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Karami.Init.MessageBroker.AggregateArticleService;

public class Article
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Retry exchange
        channel.FanOutExchangeDeclare(Broker.AggregateArticle_Article_Exchange_Retry_1);
        channel.FanOutExchangeDeclare(Broker.AggregateArticle_Article_Exchange_Retry_2);
        
        //Main queue
        channel.QueueDeclare(Broker.AggregateArticle_Article_Queue, new Dictionary<string, object> {
            { "x-delayed-type"         , "fanout" },
            { "x-dead-letter-exchange" , Broker.AggregateArticle_Article_Exchange_Retry_1 }
        });
    
        //Retry queue
        channel.QueueDeclare(Broker.AggregateArticle_Article_Queue_Retry, new Dictionary<string, object> {
            { "x-message-ttl"          , 5000 }     , //5s
            { "x-delayed-type"         , "fanout" } ,
            { "x-dead-letter-exchange" , Broker.AggregateArticle_Article_Exchange_Retry_2 }
        });
        
        //Binding
        channel.BindQueueToFanOutExchange(Broker.Article_Article_Exchange                  , Broker.AggregateArticle_Article_Queue);
        channel.BindQueueToFanOutExchange(Broker.AggregateArticle_Article_Exchange_Retry_1 , Broker.AggregateArticle_Article_Queue_Retry);
        channel.BindQueueToFanOutExchange(Broker.AggregateArticle_Article_Exchange_Retry_2 , Broker.AggregateArticle_Article_Queue);
    }
}