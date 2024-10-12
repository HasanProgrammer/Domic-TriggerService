using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.CommentService;

public class TermCommentAnswer
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Main exchange
        channel.FanOutExchangeDeclare("Comment_TermCommentAnswer_Exchange");
    }
}