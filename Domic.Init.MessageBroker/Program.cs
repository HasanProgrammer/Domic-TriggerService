using Domic.Core.Domain.Constants;
using Domic.Core.Infrastructure.Extensions;
using Domic.Init.MessageBroker.AggregateArticleService;
using Domic.Init.MessageBroker.StateTrackerService;
using Domic.Init.MessageBroker.TermService;
using Domic.Init.MessageBroker.TicketService;
using Domic.Init.MessageBroker.UserService;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

using Article   = Domic.Init.MessageBroker.ArticleService.Article;
using Exception = System.Exception;

using User                           = Domic.Init.MessageBroker.UserService.User;
using Category                       = Domic.Init.MessageBroker.CategoryService.Category;
using UserOfAuth                     = Domic.Init.MessageBroker.AuthService.User;
using RoleOfAuth                     = Domic.Init.MessageBroker.AuthService.Role;
using PermissionOfAuth               = Domic.Init.MessageBroker.AuthService.Permission;
using CategoryOfArticle              = Domic.Init.MessageBroker.ArticleService.Category;
using UserOfArticle                  = Domic.Init.MessageBroker.ArticleService.User;
using ArticleOfAggregateArticle      = Domic.Init.MessageBroker.AggregateArticleService.Article;
using TermOfAggregateTerm            = Domic.Init.MessageBroker.AggregateTermService.Term;
using VideoOfAggregateTerm           = Domic.Init.MessageBroker.AggregateTermService.Video;
using CategoryOfAggregateArticle     = Domic.Init.MessageBroker.AggregateArticleService.Category;
using UserOfAggregateArticle         = Domic.Init.MessageBroker.AggregateArticleService.User;
using ExceptionOfStateTracker        = Domic.Init.MessageBroker.StateTrackerService.Exception;
using ArticleCommentOfComment        = Domic.Init.MessageBroker.CommentService.ArticleComment;
using ArticleCommentAnswerOfComment  = Domic.Init.MessageBroker.CommentService.ArticleCommentAnswer;
using ArticleOfComment               = Domic.Init.MessageBroker.CommentService.Article;
using TicketOfAggregateTicket        = Domic.Init.MessageBroker.AggregateTicketService.Ticket;
using TicketCommentOfAggregateTicket = Domic.Init.MessageBroker.AggregateTicketService.TicketComment;
using UserOfAggregateTicket          = Domic.Init.MessageBroker.AggregateTicketService.User;
using UserOfTicket                   = Domic.Init.MessageBroker.TicketService.User;

#region Init

var rootDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

var pathConfig = Path.Combine(rootDir, "Configs", "MessageBroker.json");

var config = new ConfigurationBuilder().AddJsonFile(pathConfig).Build();

var factory = new ConnectionFactory {
    HostName = config.GetValue<string>("RabbitMQ:External:HostName"),
    UserName = config.GetValue<string>("RabbitMQ:External:Username"),
    Password = config.GetValue<string>("RabbitMQ:External:Password"),
    Port     = config.GetValue<int>("RabbitMQ:External:Port")
};
            
var connection = factory.CreateConnection();
var channel    = connection.CreateModel();

#endregion

try
{
    //UserService
    User.Register(channel);
    Role.Register(channel);
    Permission.Register(channel);

    //AuthService
    UserOfAuth.Register(channel);
    RoleOfAuth.Register(channel);
    PermissionOfAuth.Register(channel);

    //CategoryService
    Category.Register(channel);

    //ArticleService
    Article.Register(channel);
    CategoryOfArticle.Register(channel);
    UserOfArticle.Register(channel);
    
    //CommentService
    ArticleOfComment.Register(channel);
    ArticleCommentOfComment.Register(channel);
    ArticleCommentAnswerOfComment.Register(channel);

    //AggregateArticleService
    ArticleOfAggregateArticle.Register(channel);
    CategoryOfAggregateArticle.Register(channel);
    UserOfAggregateArticle.Register(channel);
    ArticleCommentAnswer.Register(channel);
    ArticleComment.Register(channel);
    
    //TermService
    Term.Register(channel);
    Video.Register(channel);
    
    //AggregateTermService
    TermOfAggregateTerm.Register(channel);
    VideoOfAggregateTerm.Register(channel);

    //StateTrackerService
    Event.Register(channel);
    ExceptionOfStateTracker.Register(channel);
    Request.Register(channel);
    
    //TicketService
    Ticket.Register(channel);
    TicketComment.Register(channel);
    UserOfTicket.Register(channel);
    
    //AggregateTicketService
    TicketOfAggregateTicket.Register(channel);
    TicketCommentOfAggregateTicket.Register(channel);
    UserOfAggregateTicket.Register(channel);
    
    //ServiceRegistry
    
    channel.DirectExchangeDeclare(Broker.ServiceRegistry_Exchange);
    channel.FanOutExchangeDeclare(Broker.ServiceRegistry_Exchange_Retry_1);
    channel.FanOutExchangeDeclare(Broker.ServiceRegistry_Exchange_Retry_2);
    
    channel.QueueDeclare(Broker.ServiceRegistry_Queue, new Dictionary<string, object> {
        { "x-delayed-type"         , "fanout" },
        { "x-dead-letter-exchange" , Broker.ServiceRegistry_Exchange_Retry_1 }
    });
    
    channel.QueueDeclare(Broker.ServiceRegistry_Queue_Retry, new Dictionary<string, object> {
        { "x-message-ttl"          , 5000 }     , //5s
        { "x-delayed-type"         , "fanout" } ,
        { "x-dead-letter-exchange" , Broker.ServiceRegistry_Exchange_Retry_2 }
    });
    
    channel.BindQueueToDirectExchange(Broker.ServiceRegistry_Exchange , Broker.ServiceRegistry_Queue, 
        Broker.ServiceRegistry_Route
    );
    
    channel.BindQueueToFanOutExchange(Broker.ServiceRegistry_Exchange_Retry_1 , Broker.ServiceRegistry_Queue_Retry);
    channel.BindQueueToFanOutExchange(Broker.ServiceRegistry_Exchange_Retry_2 , Broker.ServiceRegistry_Queue);
    
}
catch(Exception e)
{
    Console.WriteLine(e.StackTrace);
}
finally
{
    connection.Close();
    connection.Dispose();
}