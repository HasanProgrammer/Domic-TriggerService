using System.Data.SqlClient;

namespace Domic.Init.Seeder.AuthServiceCommands;

public class CreateRoleUserCommand
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="userId"></param>
    /// <param name="roleId"></param>
    public static async Task ExecuteAsync(SqlConnection connection, string userId, string roleId)
    {
        string roleUserId = Guid.NewGuid().ToString();
        
        const string createCommand = """
            INSERT INTO RoleUsers (Id, UserId, RoleId) VALUES (@Id, @UserId, @RoleId)
        """;
        
        var sqlCommand = new SqlCommand(createCommand, connection);
        
        sqlCommand.Parameters.AddWithValue("@Id", roleUserId);
        sqlCommand.Parameters.AddWithValue("@UserId", userId);
        sqlCommand.Parameters.AddWithValue("@RoleId", roleId);
        
        await sqlCommand.ExecuteNonQueryAsync();
    }
}