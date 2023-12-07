using System.Data.SqlClient;
using Karami.Core.Domain.Implementations;

namespace Karami.Init.Seeder.UserServiceCommands;

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
        var now             = DateTime.Now;
        var persianDateTime = new DotrisDateTime().ToPersianShortDate(now);
        string permissionId = Guid.NewGuid().ToString();
        
        const string createCommand = """
            INSERT INTO Permissions (Id, RoleId, Name, IsActive, CreatedAt_EnglishDate, CreatedAt_PersianDate, 
                                     UpdatedAt_EnglishDate, UpdatedAt_PersianDate) 
            VALUES (@Id, @RoleId, @Name, @IsActive, @CreatedAt_EnglishDate, @CreatedAt_PersianDate, 
                                     @UpdatedAt_EnglishDate, @UpdatedAt_PersianDate)
        """;
        
        var SqlCommand = new SqlCommand(createCommand, connection);
        
        SqlCommand.Parameters.AddWithValue("@Id", permissionId);
        SqlCommand.Parameters.AddWithValue("@RoleId", roleId);
        SqlCommand.Parameters.AddWithValue("@Name", "Create-User");
        SqlCommand.Parameters.AddWithValue("@IsActive", 1);
        SqlCommand.Parameters.AddWithValue("@CreatedAt_EnglishDate", now);
        SqlCommand.Parameters.AddWithValue("@CreatedAt_PersianDate", persianDateTime);
        SqlCommand.Parameters.AddWithValue("@UpdatedAt_EnglishDate", now);
        SqlCommand.Parameters.AddWithValue("@UpdatedAt_PersianDate", persianDateTime);
        
        await SqlCommand.ExecuteNonQueryAsync();

        return permissionId;
    }
}