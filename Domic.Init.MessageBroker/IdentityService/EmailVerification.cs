using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.IdentityService;

public class EmailVerification
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channel"></param>
    public static void Register(IModel channel)
    {
        //Retry exchange
        channel.FanOutExchangeDeclare("Identity_EmailVerification_Exchange");
    }
}