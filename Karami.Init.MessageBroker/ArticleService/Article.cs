using Karami.Core.Domain.Constants;
using Karami.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Karami.Init.MessageBroker.ArticleService;

public class Article
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Main exchange
        channel.FanOutExchangeDeclare(Broker.Article_Article_Exchange);
    }
}