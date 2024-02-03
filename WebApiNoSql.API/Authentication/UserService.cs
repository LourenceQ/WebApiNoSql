namespace WebApiNoSql.API.Authentication;

public class UserService : IUserService
{
    public Task<User> Authenticate(string username, string password)
    {
        return username != "admin" || password != "P@$$w0rd"
            ? Task.FromResult<User>(null)
            : Task.FromResult(new User() { Username = username, Id = Guid.NewGuid().ToString("N") });

    }
}
