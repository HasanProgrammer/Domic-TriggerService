using Domic.Init.MessageBroker.CommentService;
using Domic.Init.MessageBroker.DiscoveryService;
using Domic.Init.MessageBroker.FinancialService;
using Domic.Init.MessageBroker.IdentityService;
using Domic.Init.MessageBroker.NotificationService;
using Domic.Init.MessageBroker.StateTrackerService;
using Domic.Init.MessageBroker.TermService;
using Domic.Init.MessageBroker.TicketService;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

using Article   = Domic.Init.MessageBroker.ArticleService.Article;
using Exception = System.Exception;

using User                                = Domic.Init.MessageBroker.UserService.User;
using Category                            = Domic.Init.MessageBroker.CategoryService.Category;
using UserOfAuth                          = Domic.Init.MessageBroker.IdentityService.User;
using RoleOfAuth                          = Domic.Init.MessageBroker.IdentityService.Role;
using PermissionOfAuth                    = Domic.Init.MessageBroker.IdentityService.Permission;
using CategoryOfArticle                   = Domic.Init.MessageBroker.ArticleService.Category;
using UserOfArticle                       = Domic.Init.MessageBroker.ArticleService.User;
using ArticleOfAggregateArticle           = Domic.Init.MessageBroker.AggregateArticleService.Article;
using TermOfAggregateTerm                 = Domic.Init.MessageBroker.AggregateTermService.Term;
using VideoOfAggregateTerm                = Domic.Init.MessageBroker.AggregateTermService.Video;
using UserOfAggregateTerm                 = Domic.Init.MessageBroker.AggregateTermService.User;
using CategoryOfAggregateTerm             = Domic.Init.MessageBroker.AggregateTermService.Category;
using CategoryOfAggregateArticle          = Domic.Init.MessageBroker.AggregateArticleService.Category;
using UserOfAggregateArticle              = Domic.Init.MessageBroker.AggregateArticleService.User;
using ExceptionOfStateTracker             = Domic.Init.MessageBroker.StateTrackerService.Exception;
using ArticleCommentOfComment             = Domic.Init.MessageBroker.CommentService.ArticleComment;
using ArticleCommentAnswerOfComment       = Domic.Init.MessageBroker.CommentService.ArticleCommentAnswer;
using ArticleOfComment                    = Domic.Init.MessageBroker.CommentService.Article;
using TicketOfAggregateTicket             = Domic.Init.MessageBroker.AggregateTicketService.Ticket;
using TicketCommentOfAggregateTicket      = Domic.Init.MessageBroker.AggregateTicketService.TicketComment;
using UserOfAggregateTicket               = Domic.Init.MessageBroker.AggregateTicketService.User;
using CategoryOfAggregateTicket           = Domic.Init.MessageBroker.AggregateTicketService.Category;
using UserOfTicket                        = Domic.Init.MessageBroker.TicketService.User;
using UserOfFinancial                     = Domic.Init.MessageBroker.FinancialService.User;
using AccountOfAggregateFinancial         = Domic.Init.MessageBroker.AggregateFinancialService.Account;
using ArticleComment                      = Domic.Init.MessageBroker.AggregateArticleService.ArticleComment;
using ArticleCommentAnswer                = Domic.Init.MessageBroker.AggregateArticleService.ArticleCommentAnswer;
using Permission                          = Domic.Init.MessageBroker.UserService.Permission;
using Role                                = Domic.Init.MessageBroker.UserService.Role;
using Term                                = Domic.Init.MessageBroker.TermService.Term;
using UserOfAggregateFinancial            = Domic.Init.MessageBroker.AggregateFinancialService.User;
using TermOfComment                       = Domic.Init.MessageBroker.CommentService.Term;
using UserOfComment                       = Domic.Init.MessageBroker.CommentService.User;
using TermCommentOfAggregateTerm          = Domic.Init.MessageBroker.AggregateTermService.TermComment;
using TermCommentAnswerOfAggregateTerm    = Domic.Init.MessageBroker.AggregateTermService.TermCommentAnswer;

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

    //IdentityService
    UserOfAuth.Register(channel);
    RoleOfAuth.Register(channel);
    PermissionOfAuth.Register(channel);
    OtpLog.Register(channel);
    
    //NotificationService
    Notification.Register(channel);

    //CategoryService
    Category.Register(channel);

    //ArticleService
    Article.Register(channel);
    CategoryOfArticle.Register(channel);
    UserOfArticle.Register(channel);
    
    //TermService
    Term.Register(channel);
    Video.Register(channel);
    
    //CommentService
    ArticleOfComment.Register(channel);
    ArticleCommentOfComment.Register(channel);
    ArticleCommentAnswerOfComment.Register(channel);
    TermOfComment.Register(channel);
    TermComment.Register(channel);
    TermCommentAnswer.Register(channel);
    UserOfComment.Register(channel);

    //AggregateArticleService
    ArticleOfAggregateArticle.Register(channel);
    CategoryOfAggregateArticle.Register(channel);
    UserOfAggregateArticle.Register(channel);
    ArticleCommentAnswer.Register(channel);
    ArticleComment.Register(channel);
    
    //AggregateTermService
    UserOfAggregateTerm.Register(channel);
    CategoryOfAggregateTerm.Register(channel);
    TermOfAggregateTerm.Register(channel);
    VideoOfAggregateTerm.Register(channel);
    VideoOfAggregateTerm.Register(channel);
    TermCommentOfAggregateTerm.Register(channel);
    TermCommentAnswerOfAggregateTerm.Register(channel);

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
    CategoryOfAggregateTicket.Register(channel);
    
    //FinancialService
    Account.Register(channel);
    Transaction.Register(channel);
    UserOfFinancial.Register(channel);
    
    //AggregateFinancialService
    AccountOfAggregateFinancial.Register(channel);
    Domic.Init.MessageBroker.AggregateFinancialService.Transaction.Register(channel);
    UserOfAggregateFinancial.Register(channel);
    
    //ServiceRegistry
    ServiceRegistry.Register(channel);
    
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