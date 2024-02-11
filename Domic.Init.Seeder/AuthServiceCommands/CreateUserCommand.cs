using System.Data.SqlClient;
using Domic.Core.Domain.Extensions;

namespace Domic.Init.Seeder.AuthServiceCommands;

public class CreateUserCommand
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="connection"></param>
    /// <returns></returns>
    public static async Task<string> ExecuteAsync(SqlConnection connection)
    {
        string userId = Guid.NewGuid().ToString();
        
        const string createCommand = """
            INSERT INTO Users (Id, FirstName, LastName, Username, Password, IsActive)
            VALUES (@Id, @FirstName, @LastName, @Username, @Password, @IsActive)
        """;
        
        var sqlCommand = new SqlCommand(createCommand, connection);
        
        sqlCommand.Parameters.AddWithValue("@Id", userId);
        sqlCommand.Parameters.AddWithValue("@FirstName", "Hasan");
        sqlCommand.Parameters.AddWithValue("@LastName", "Domic Moheb");
        sqlCommand.Parameters.AddWithValue("@Username", "Hasan_Domic_Moheb");
        sqlCommand.Parameters.AddWithValue("@Password", await "Hasan313@313!!".HashAsync());
        sqlCommand.Parameters.AddWithValue("@IsActive", 1);

        await sqlCommand.ExecuteNonQueryAsync();

        return userId;
    }
}