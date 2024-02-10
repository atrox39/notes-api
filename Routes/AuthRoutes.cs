using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using notes.DTOs;
using notes.Models;
using notes.Respository;
using notes.Utils;

namespace notes
{
  public static class AuthRoutes
  {
    public static RouteGroupBuilder MapAuth(this RouteGroupBuilder group) {
      group.MapPost("/register", Create);
      group.MapPost("/login", Login);
      return group;
    }

    private static async Task<Results<Created, BadRequest>> Create(RegisterDto registerDto, IAuthRepository authRepository, IMapper mapper) {
      registerDto.Password = Methods.EncryptSHA256Text(registerDto.Password); // Password to SHA256
      var result = await authRepository.Create(mapper.Map<Users>(registerDto));
      if (!result) {
        return TypedResults.BadRequest();
      }
      return TypedResults.Created();
    }

    private static async Task<Results<Ok<TokenDTO>, UnauthorizedHttpResult>> Login(LoginDto loginDto, IAuthRepository authRepository, IJwtRepository jwtRepository) {
      loginDto.Password = Methods.EncryptSHA256Text(loginDto.Password); // Password to SHA256
      var user = await authRepository.Login(loginDto);
      if (user is null) {
        TypedResults.Unauthorized();
      }
      return TypedResults.Ok(new TokenDTO{
        Token = jwtRepository.CreateToken(Environment.GetEnvironmentVariable("JWT_SECRET")!, user!),
      });
    } 
  }
}
