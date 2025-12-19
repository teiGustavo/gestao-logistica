using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestaoLogisticaAPI.Application.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace GestaoLogisticaAPI.Application.Services;

public class AuthService(IConfiguration config, IUsuarioRepository repo) : IAuthService
{
    public async Task<string?> LoginAsync(string apelido, string senha)
    {
        var user = await repo.LoginAsync(apelido, senha);
        if (user == null) return null;

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.CodUsuario.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Apelido),
            new Claim("role", user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? string.Empty));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(config["Jwt:ExpireMinutes"] ?? "60")),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}