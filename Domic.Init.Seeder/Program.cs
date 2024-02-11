using System.Data.SqlClient;
using Domic.Init.Seeder.UserServiceCommands;

using AuthCreateRoleCommand           = Domic.Init.Seeder.AuthServiceCommands.CreateRoleCommand;
using AuthCreatePermissionCommand     = Domic.Init.Seeder.AuthServiceCommands.CreatePermissionCommand;
using AuthCreateUserCommand           = Domic.Init.Seeder.AuthServiceCommands.CreateUserCommand;
using AuthCreateRoleUserCommand       = Domic.Init.Seeder.AuthServiceCommands.CreateRoleUserCommand;
using AuthCreatePermissionUserCommand = Domic.Init.Seeder.AuthServiceCommands.CreatePermissionUserCommand;

try
{
    var userConnection = new SqlConnection("Server=.;Database=UserService;Trusted_Connection=true;");
    var authConnection = new SqlConnection("Server=.;Database=AuthService;Trusted_Connection=true;");

    #region UserService Registrations

    await userConnection.OpenAsync();

    var roleId       = await CreateRoleCommand.ExecuteAsync(userConnection);
    var permissionId = await CreatePermissionCommand.ExecuteAsync(userConnection, roleId);
    var userId       = await CreateUserCommand.ExecuteAsync(userConnection);

    await CreateRoleUserCommand.ExecuteAsync(userConnection, userId, roleId);
    await CreatePermissionUserCommand.ExecuteAsync(userConnection, userId, permissionId);

    await userConnection.CloseAsync();

    #endregion

    #region AuthService Registrations

    await authConnection.OpenAsync();
    
    var authRoleId       = await AuthCreateRoleCommand.ExecuteAsync(authConnection);
    var authPermissionId = await AuthCreatePermissionCommand.ExecuteAsync(authConnection, authRoleId);
    var authUserId       = await AuthCreateUserCommand.ExecuteAsync(authConnection);

    await AuthCreateRoleUserCommand.ExecuteAsync(authConnection, authUserId, authRoleId);
    await AuthCreatePermissionUserCommand.ExecuteAsync(authConnection, authUserId, authPermissionId);

    await authConnection.CloseAsync();
    
    #endregion
    
    Console.WriteLine("Data are inserted successfully");
}
catch (Exception e)
{
    Console.WriteLine(e.StackTrace);
}