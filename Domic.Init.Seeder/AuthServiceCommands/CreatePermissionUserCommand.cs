using System.Data.SqlClient;
using Domic.Core.Common.ClassExtensions;

namespace Domic.Init.Seeder.AuthServiceCommands;

public class CreatePermissionUserCommand
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="userId"></param>
    /// <param name="permissionId"></param>
    public static async Task ExecuteAsync(SqlConnection connection, string userId, string permissionId)
    {
        string permissionUserId = Guid.NewGuid().ToString();
        
        const string createCommand = """
            INSERT INTO PermissionUsers (Id, UserId, PermissionId) VALUES (@Id, @UserId, @PermissionId) 
        """;
        
        var sqlCommand = new SqlCommand(createCommand, connection);
        
        sqlCommand.Parameters.AddWithValue("@Id", permissionUserId);
        sqlCommand.Parameters.AddWithValue("@UserId", userId);
        sqlCommand.Parameters.AddWithValue("@PermissionId", permissionId);
        
        await sqlCommand.ExecuteNonQueryAsync();
    }
}