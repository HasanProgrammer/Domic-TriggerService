using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;

#region Init

var rootDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

var pathConfig = Path.Combine(rootDir, "Configs", "EventStreamBroker.json");

var config = new ConfigurationBuilder().AddJsonFile(pathConfig).Build();

#endregion

try
{
    var adminConfig = new AdminClientConfig {
        BootstrapServers = config.GetValue<string>("Kafka:External:HostName"),
        SaslUsername = config.GetValue<string>("Kafka:External:Username"),
        SaslPassword = config.GetValue<string>("Kafka:External:Password")
    };

    using (var adminClient = new AdminClientBuilder(adminConfig).Build())
    {
        try
        {
            await adminClient.CreateTopicsAsync(new List<TopicSpecification> {
                //user
                new() { Name = "User", NumPartitions = 10, ReplicationFactor = 1 },
                new() { Name = "UserService-Retry-User", NumPartitions = 10, ReplicationFactor = 1 },
                new() { Name = "ArticleService-Retry-User", NumPartitions = 10, ReplicationFactor = 1 },
                new() { Name = "AggregateArticleService-Retry-User", NumPartitions = 10, ReplicationFactor = 1 },
                new() { Name = "TermService-Retry-User", NumPartitions = 10, ReplicationFactor = 1 },
                new() { Name = "AggregateTermService-Retry-User", NumPartitions = 10, ReplicationFactor = 1 },
                new() { Name = "AuthService-Retry-User", NumPartitions = 10, ReplicationFactor = 1 },
                new() { Name = "CommentService-Retry-User", NumPartitions = 10, ReplicationFactor = 1 },
                new() { Name = "FinancialService-Retry-User", NumPartitions = 10, ReplicationFactor = 1 },
                
                //category
                new() { Name = "Category", NumPartitions = 10, ReplicationFactor = 1 },
                new() { Name = "ArticleService-Retry-Category", NumPartitions = 10, ReplicationFactor = 1 },
                new() { Name = "TermService-Retry-Category", NumPartitions = 10, ReplicationFactor = 1 },
                new() { Name = "AggregateArticleService-Retry-Category", NumPartitions = 10, ReplicationFactor = 1 },
                new() { Name = "AggregateTermService-Retry-Category", NumPartitions = 10, ReplicationFactor = 1 },
                
                //article
                new() { Name = "Article", NumPartitions = 10, ReplicationFactor = 1 },
                new() { Name = "AggregateArticleService-Retry-Article", NumPartitions = 10, ReplicationFactor = 1 },
                new() { Name = "CommentService-Retry-Article", NumPartitions = 10, ReplicationFactor = 1 },
                
                //articleComment
                new() { Name = "ArticleComment", NumPartitions = 10, ReplicationFactor = 1 },
                new() { Name = "AggregateArticleService-Retry-ArticleComment", NumPartitions = 10, ReplicationFactor = 1 },
                
                //articleCommentAnswer
                new() { Name = "ArticleCommentAsnwer", NumPartitions = 10, ReplicationFactor = 1 },
                new() { Name = "AggregateArticleService-Retry-ArticleCommentAsnwer", NumPartitions = 10, ReplicationFactor = 1 },
                
                //termComment
                new() { Name = "TermComment", NumPartitions = 10, ReplicationFactor = 1 },
                new() { Name = "AggregateTermService-Retry-TermComment", NumPartitions = 10, ReplicationFactor = 1 },
                
                //termCommentAnswer
                new() { Name = "TermCommentAnswer", NumPartitions = 10, ReplicationFactor = 1 },
                new() { Name = "AggregateTermService-Retry-TermCommentAnswer", NumPartitions = 10, ReplicationFactor = 1 },
                
                //statetracker
                new() { Name = "StateTracker", NumPartitions = 10, ReplicationFactor = 1 },
                new() { Name = "StateTrackerService-Retry-StateTracker", NumPartitions = 10, ReplicationFactor = 1 },
                
                //notification
                new() { Name = "Notification", NumPartitions = 10, ReplicationFactor = 1 },
                new() { Name = "NotificationService-Retry-Notification", NumPartitions = 10, ReplicationFactor = 1 },
            });
        }
        catch (CreateTopicsException e)
        {
            Console.WriteLine($"An error occured creating topic {e.Results[0].Topic} : {e.Results[0].Error.Reason}");
        }
    }
}
catch(Exception e)
{
    Console.WriteLine(e.StackTrace);
}