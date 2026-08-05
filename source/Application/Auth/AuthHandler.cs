namespace Architecture.Application;

public sealed class AuthHandler
(
    IConfiguration configuration,
    IAuthRepository authRepository,
    IHashService hashService,
    IStringLocalizer stringLocalizer
)
: IHandler<AuthRequest, AuthResponse>
{
    public async Task<Result<AuthResponse>> HandleAsync(AuthRequest request)
    {
        var auth = await authRepository.GetByLoginAsync(request.Login);

        return auth is null || !hashService.Validate(request.Password, auth.Salt.ToString(), auth.Password)
            ? new Result<AuthResponse>(Unauthorized, stringLocalizer[nameof(Unauthorized)])
            : new Result<AuthResponse>(OK, new AuthResponse(CreateToken(auth)));
    }

    private string CreateToken(Auth auth)
    {
        var claims = new List<Claim> { new("sub", auth.Id.ToString()) };

        claims.AddRange(auth.Roles.ToArray().Select(role => new Claim("role", role)));

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SigningKey"] ?? string.Empty));

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken("", "", claims, DateTime.UtcNow, DateTime.UtcNow.Add(TimeSpan.FromDays(1)), signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
