using System.Data.SqlClient;
using Karami.Core.Domain.Implementations;

namespace Karami.Init.Seeder.UserServiceCommands;

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
        var now             = DateTime.Now;
        var persianDateTime = new DotrisDateTime().ToPersianShortDate(now);
        string roleUserId   = Guid.NewGuid().ToString();
        
        const string createCommand = """
            INSERT INTO RoleUsers (Id, UserId, RoleId, IsActive, CreatedAt_EnglishDate, CreatedAt_PersianDate, 
                               UpdatedAt_EnglishDate, UpdatedAt_PersianDate) 
            VALUES (@Id, @UserId, @RoleId, @IsActive, @CreatedAt_EnglishDate, @CreatedAt_PersianDate, 
                               @UpdatedAt_EnglishDate, @UpdatedAt_PersianDate) 
        """;
        
        var sqlCommand = new SqlCommand(createCommand, connection);
        
        sqlCommand.Parameters.AddWithValue("@Id", roleUserId);
        sqlCommand.Parameters.AddWithValue("@UserId", userId);
        sqlCommand.Parameters.AddWithValue("@RoleId", roleId);
        sqlCommand.Parameters.AddWithValue("@IsActive", 1);
        sqlCommand.Parameters.AddWithValue("@CreatedAt_EnglishDate", now);
        sqlCommand.Parameters.AddWithValue("@CreatedAt_PersianDate", persianDateTime);
        sqlCommand.Parameters.AddWithValue("@UpdatedAt_EnglishDate", now);
        sqlCommand.Parameters.AddWithValue("@UpdatedAt_PersianDate", persianDateTime);
        
        await sqlCommand.ExecuteNonQueryAsync();
    }
}