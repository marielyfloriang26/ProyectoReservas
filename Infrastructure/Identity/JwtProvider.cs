using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity;

public class JwtProvider : IJwtProvider
{
    private readonly JwtSettings _settings;

    // usamos IOptions para traer la config de jwtsettings de forma limpia
    public JwtProvider(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }
    public string Generate(User usuario)
    {
        if (string.IsNullOrEmpty(_settings.SecretKey))
        {
            throw new Exception("ERROR CRÍTICO: La SecretKey sigue llegando vacía al Provider. Revisa el nombre en appsettings.json y Program.cs");
        }
        
        // creamos los claims (datos del usuario dentro del token)
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email)
        };

        // manejo de roles, recorremos la lista de roles que definimos en domain 
        foreach (var rol in usuario.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, rol.ToString()));
        }

        // crea la firma de seguridad
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // que son esos numeros

        // crea objeto de token
        var token = new JwtSecurityToken(
            _settings.Issuer,
            _settings.Audience,
            claims,
            null,
            DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
            creds
        );
        // Lo convierte a una cadena de texto (el token que ves en Swagger)
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}