using Karami.Core.Domain.Constants;
using Karami.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Karami.Init.MessageBroker.CommentService;

public class ArticleCommentAnswer
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Main exchange
        channel.FanOutExchangeDeclare(Broker.Comment_ArticleCommentAnswer_Exchange);
    }
}