using System.Data.SqlClient;
using Karami.Core.Domain.Extensions;
using Karami.Core.Domain.Implementations;

namespace Karami.Init.Seeder.UserServiceCommands;

public class CreateUserCommand
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="connection"></param>
    /// <returns></returns>
    public static async Task<string> ExecuteAsync(SqlConnection connection)
    {
        var now             = DateTime.Now;
        var persianDateTime = new DotrisDateTime().ToPersianShortDate(now);
        string userId       = Guid.NewGuid().ToString();
        
        const string description = "من حسن کرمی محب ؛ برنامه نویس و عاشق معماری های برنامه نویسی ، 26 ساله ، کشور ایران و اهل شهرستان شهریار هستم";
        
        const string createCommand = """
            INSERT INTO Users (Id, FirstName, LastName, Description, Username, Password, PhoneNumber, Email,
                               IsActive, CreatedAt_EnglishDate, CreatedAt_PersianDate, UpdatedAt_EnglishDate,
                               UpdatedAt_PersianDate)
            VALUES (@Id, @FirstName, @LastName, @Description, @Username, @Password, @PhoneNumber, @Email,
                               @IsActive, @CreatedAt_EnglishDate, @CreatedAt_PersianDate, @UpdatedAt_EnglishDate,
                               @UpdatedAt_PersianDate)
        """;
        
        var sqlCommand = new SqlCommand(createCommand, connection);
        
        sqlCommand.Parameters.AddWithValue("@Id", userId);
        sqlCommand.Parameters.AddWithValue("@FirstName", "Hasan");
        sqlCommand.Parameters.AddWithValue("@LastName", "Karami Moheb");
        sqlCommand.Parameters.AddWithValue("@Description", description);
        sqlCommand.Parameters.AddWithValue("@Username", "Hasan_Karami_Moheb");
        sqlCommand.Parameters.AddWithValue("@Password", await "Hasan313@313!!".HashAsync());
        sqlCommand.Parameters.AddWithValue("@PhoneNumber", "09026676147");
        sqlCommand.Parameters.AddWithValue("@Email", "hasankarami2020313@gmail.com");
        sqlCommand.Parameters.AddWithValue("@IsActive", 1);
        sqlCommand.Parameters.AddWithValue("@CreatedAt_EnglishDate", now);
        sqlCommand.Parameters.AddWithValue("@CreatedAt_PersianDate", persianDateTime);
        sqlCommand.Parameters.AddWithValue("@UpdatedAt_EnglishDate", now);
        sqlCommand.Parameters.AddWithValue("@UpdatedAt_PersianDate", persianDateTime);

        await sqlCommand.ExecuteNonQueryAsync();

        return userId;
    }
}