using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.TicketService;

public class TicketComment
{
    public static void Register(IModel channel)
    {
        //Main exchange
        channel.FanOutExchangeDeclare("Ticket_TicketComment_Exchange");
    }
}