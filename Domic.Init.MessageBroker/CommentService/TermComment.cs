using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.CommentService;

public class TermComment
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Main exchange
        channel.FanOutExchangeDeclare("Comment_TermComment_Exchange");
    }
}