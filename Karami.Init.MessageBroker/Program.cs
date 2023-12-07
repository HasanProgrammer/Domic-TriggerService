using Karami.Core.Common.ClassExtensions;
using Karami.Core.Domain.Constants;
using Karami.Core.Infrastructure.Extensions;
using Karami.Init.MessageBroker.AggregateArticleService;
using Karami.Init.MessageBroker.StateTrackerService;
using Karami.Init.MessageBroker.UserService;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

using Article     = Karami.Init.MessageBroker.ArticleService.Article;
using Environment = Karami.Core.Common.ClassConsts.Environment;
using Exception   = System.Exception;

using User                          = Karami.Init.MessageBroker.UserService.User;
using Category                      = Karami.Init.MessageBroker.CategoryService.Category;
using UserOfAuth                    = Karami.Init.MessageBroker.AuthService.User;
using RoleOfAuth                    = Karami.Init.MessageBroker.AuthService.Role;
using PermissionOfAuth              = Karami.Init.MessageBroker.AuthService.Permission;
using CategoryOfArticle             = Karami.Init.MessageBroker.ArticleService.Category;
using UserOfArticle                 = Karami.Init.MessageBroker.ArticleService.User;
using ArticleOfAggregateArticle     = Karami.Init.MessageBroker.AggregateArticleService.Article;
using CategoryOfAggregateArticle    = Karami.Init.MessageBroker.AggregateArticleService.Category;
using UserOfAggregateArticle        = Karami.Init.MessageBroker.AggregateArticleService.User;
using ExceptionOfStateTracker       = Karami.Init.MessageBroker.StateTrackerService.Exception;
using ArticleCommentOfComment       = Karami.Init.MessageBroker.CommentService.ArticleComment;
using ArticleCommentAnswerOfComment = Karami.Init.MessageBroker.CommentService.ArticleCommentAnswer;
using ArticleOfComment              = Karami.Init.MessageBroker.CommentService.Article;

#region Init

var rootDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

var pathConfig = Path.Combine(rootDir, "Configs", "MessageBroker.json");

var config = new ConfigurationBuilder().AddJsonFile(pathConfig).Build();

var factory = new ConnectionFactory {
    HostName = config.GetExternalRabbitHostName(Environment.Development),
    UserName = config.GetExternalRabbitUsername(Environment.Development),
    Password = config.GetExternalRabbitPassword(Environment.Development),
    Port     = config.GetExternalRabbitPort(Environment.Development)
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

    //StateTrackerService
    Event.Register(channel);
    ExceptionOfStateTracker.Register(channel);
    Request.Register(channel);
    
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