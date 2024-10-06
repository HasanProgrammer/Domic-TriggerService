using Domic.Core.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace Domic.Init.MessageBroker.TicketService;

public class Ticket
{
    public static void Register(IModel channel)
    {
        //Main exchange
        channel.FanOutExchangeDeclare("Ticket_Ticket_Exchange");
    }
}