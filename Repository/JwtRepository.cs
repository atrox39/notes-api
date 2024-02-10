using System.Text;
using System.IdentityModel.Tokens.Jwt;
using notes.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace notes.Respository
{
  public interface IJwtRepository
  {
    public string CreateToken(string key, Users user);
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
  }
}