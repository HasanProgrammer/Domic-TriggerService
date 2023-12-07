using System.Data.SqlClient;
using Karami.Core.Domain.Implementations;

namespace Karami.Init.Seeder.UserServiceCommands;

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
        var now                 = DateTime.Now;
        var persianDateTime     = new DotrisDateTime().ToPersianShortDate(now);
        string permissionUserId = Guid.NewGuid().ToString();
        
        const string createCommand = """
            INSERT INTO PermissionUsers (Id, UserId, PermissionId, IsActive, CreatedAt_EnglishDate, CreatedAt_PersianDate, 
                               UpdatedAt_EnglishDate, UpdatedAt_PersianDate) 
            VALUES (@Id, @UserId, @PermissionId, @IsActive, @CreatedAt_EnglishDate, @CreatedAt_PersianDate, 
                               @UpdatedAt_EnglishDate, @UpdatedAt_PersianDate) 
        """;
        
        var sqlCommand = new SqlCommand(createCommand, connection);
        
        sqlCommand.Parameters.AddWithValue("@Id", permissionUserId);
        sqlCommand.Parameters.AddWithValue("@UserId", userId);
        sqlCommand.Parameters.AddWithValue("@PermissionId", permissionId);
        sqlCommand.Parameters.AddWithValue("@IsActive", 1);
        sqlCommand.Parameters.AddWithValue("@CreatedAt_EnglishDate", now);
        sqlCommand.Parameters.AddWithValue("@CreatedAt_PersianDate", persianDateTime);
        sqlCommand.Parameters.AddWithValue("@UpdatedAt_EnglishDate", now);
        sqlCommand.Parameters.AddWithValue("@UpdatedAt_PersianDate", persianDateTime);
        
        await sqlCommand.ExecuteNonQueryAsync();
    }
}