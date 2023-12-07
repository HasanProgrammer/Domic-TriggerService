using Karami.Core.Domain.Constants;
using Karami.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Karami.Init.MessageBroker.CategoryService;

public class Category
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Main exchange
        channel.FanOutExchangeDeclare(Broker.Category_Category_Exchange);
    }
}