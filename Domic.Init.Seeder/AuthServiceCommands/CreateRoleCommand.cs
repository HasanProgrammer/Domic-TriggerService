using System.Data.SqlClient;

namespace Domic.Init.Seeder.AuthServiceCommands;

public class CreateRoleCommand
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="connection"></param>
    /// <returns></returns>
    public static async Task<string> ExecuteAsync(SqlConnection connection)
    {
        string roleId = Guid.NewGuid().ToString();
        
        const string createCommand = """
            INSERT INTO Roles (Id, Name) VALUES (@Id, @Name)
        """;
        
        var sqlCommand = new SqlCommand(createCommand, connection);
        
        sqlCommand.Parameters.AddWithValue("@Id", roleId);
        sqlCommand.Parameters.AddWithValue("@Name", "SuperAdmin");
        
        await sqlCommand.ExecuteNonQueryAsync();

        return roleId;
    }
}