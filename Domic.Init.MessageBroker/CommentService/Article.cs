﻿using Domic.Core.Domain.Constants;
using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.CommentService;

public class Article
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Retry exchange
        channel.FanOutExchangeDeclare(Broker.Comment_Article_Exchange_Retry_1);
        channel.FanOutExchangeDeclare(Broker.Comment_Article_Exchange_Retry_2);
        
        //Main queue
        channel.QueueDeclare(Broker.Comment_Article_Queue, new Dictionary<string, object> {
            { "x-delayed-type"         , "fanout" },
            { "x-dead-letter-exchange" , Broker.Comment_Article_Exchange_Retry_1 }
        });
    
        //Retry queue
        channel.QueueDeclare(Broker.Comment_Article_Queue_Retry, new Dictionary<string, object> {
            { "x-message-ttl"          , 5000 }     , //5s
            { "x-delayed-type"         , "fanout" } ,
            { "x-dead-letter-exchange" , Broker.Comment_Article_Exchange_Retry_2 }
        });
        
        //Binding
        channel.BindQueueToFanOutExchange(Broker.Article_Article_Exchange         , Broker.Comment_Article_Queue);
        channel.BindQueueToFanOutExchange(Broker.Comment_Article_Exchange_Retry_1 , Broker.Comment_Article_Queue_Retry);
        channel.BindQueueToFanOutExchange(Broker.Comment_Article_Exchange_Retry_2 , Broker.Comment_Article_Queue);
    }
}