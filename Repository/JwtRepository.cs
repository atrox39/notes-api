using System.Text;
using System.IdentityModel.Tokens.Jwt;
using notes.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.DataProtection;

namespace notes.Repository
{
  public interface IJwtRepository
  {
    public string CreateToken(string key, Users user);
    public ClaimsPrincipal? GetPrincipal(string key, string token);
  }

  public class JwtRepository : IJwtRepository
  {
    public string CreateToken(string key, Users user)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var byteKey = Encoding.UTF8.GetBytes(key);
      var tokenDes = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
        {
        new Claim(ClaimTypes.Email, user.Email),
        new Claim("ID", user.Id.ToString()),
        }),
        Expires = DateTime.UtcNow.AddMonths(1),
        SigningCredentials = new SigningCredentials(
          new SymmetricSecurityKey(byteKey),
          SecurityAlgorithms.HmacSha256Signature
        ),
      };
      var token = tokenHandler.CreateToken(tokenDes);
      return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal? GetPrincipal(string key, string token)
    {
      try
      {
        var tokenHandler = new JwtSecurityTokenHandler();
        var byteKey = Encoding.UTF8.GetBytes(key);
        var parameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(byteKey),
          ValidateIssuer = false,
          ValidateAudience = false
        };
        var principal = tokenHandler.ValidateToken(token, parameters, out _);
        return principal;
      }
      catch
      {
        return null;
      }
    }
  }
}