using System.Data.SqlClient;

namespace Karami.Init.Seeder.AuthServiceCommands;

public class CreatePermissionCommand
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public static async Task<string> ExecuteAsync(SqlConnection connection, string roleId)
    {
        string permissionId = Guid.NewGuid().ToString();
        
        const string createCommand = """
            INSERT INTO Permissions (Id, RoleId, Name) VALUES (@Id, @RoleId, @Name)
        """;
        
        var SqlCommand = new SqlCommand(createCommand, connection);
        
        SqlCommand.Parameters.AddWithValue("@Id", permissionId);
        SqlCommand.Parameters.AddWithValue("@RoleId", roleId);
        SqlCommand.Parameters.AddWithValue("@Name", "Create-User");
        
        await SqlCommand.ExecuteNonQueryAsync();

        return permissionId;
    }
}