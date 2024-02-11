using System.Data.SqlClient;
using Domic.Core.Infrastructure.Implementations;

namespace Domic.Init.Seeder.UserServiceCommands;

public class CreateRoleCommand
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="connection"></param>
    /// <returns></returns>
    public static async Task<string> ExecuteAsync(SqlConnection connection)
    {
        var now             = DateTime.Now;
        var persianDateTime = new DomicDateTime().ToPersianShortDate(now);
        string roleId       = Guid.NewGuid().ToString();
        
        const string createCommand = """
            INSERT INTO Roles (Id, Name, IsActive, CreatedAt_EnglishDate, CreatedAt_PersianDate, UpdatedAt_EnglishDate, 
                               UpdatedAt_PersianDate) 
            VALUES (@Id, @Name, @IsActive, @CreatedAt_EnglishDate, @CreatedAt_PersianDate, @UpdatedAt_EnglishDate, 
                               @UpdatedAt_PersianDate)
        """;
        
        var sqlCommand = new SqlCommand(createCommand, connection);
        
        sqlCommand.Parameters.AddWithValue("@Id", roleId);
        sqlCommand.Parameters.AddWithValue("@Name", "SuperAdmin");
        sqlCommand.Parameters.AddWithValue("@IsActive", 1);
        sqlCommand.Parameters.AddWithValue("@CreatedAt_EnglishDate", now);
        sqlCommand.Parameters.AddWithValue("@CreatedAt_PersianDate", persianDateTime);
        sqlCommand.Parameters.AddWithValue("@UpdatedAt_EnglishDate", now);
        sqlCommand.Parameters.AddWithValue("@UpdatedAt_PersianDate", persianDateTime);
        
        await sqlCommand.ExecuteNonQueryAsync();

        return roleId;
    }
}