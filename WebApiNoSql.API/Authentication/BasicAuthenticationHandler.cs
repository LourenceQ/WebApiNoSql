using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace WebApiNoSql.API.Authentication;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IUserService _userService;

    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options
        , ILoggerFactory logger
        , UrlEncoder encoder
        , ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _userService = new UserService();
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        User user;

        try
        {
            AuthenticationHeaderValue authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            byte[] credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? string.Empty);
            string[] credentials = Encoding.UTF8.GetString(credentialBytes).Split([':'], 2);
            string username = credentials[0];
            string password = credentials[1];
            user = await _userService.Authenticate(username, password);
        }
        catch
        {
            return AuthenticateResult.Fail("Error Ocurred.Authorization failed.");
        }

        Claim[] claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Username)
        ];

        ClaimsIdentity identity = new(claims, Scheme.Name);
        ClaimsPrincipal principal = new(identity);
        AuthenticationTicket ticket = new(principal, Scheme.Name);


        return user == null
            ? AuthenticateResult.Fail("Invalid Credentials")
            : AuthenticateResult.Success(ticket);
    }
}
