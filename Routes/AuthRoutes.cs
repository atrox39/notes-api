using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using FluentValidation;
using Notes.Data.DTOs;
using Notes.Data.Models;
using Notes.Repository;
using Notes.Utils;

namespace Notes.Routes
{
  public static class AuthRoutes
  {
    public static RouteGroupBuilder MapAuth(this RouteGroupBuilder group) {
      group.MapPost("/register", Create);
      group.MapPost("/login", Login);
      return group;
    }

    private static async Task<Results<Created, BadRequest, ValidationProblem>> Create(
      RegisterDto registerDto,
      IAuthRepository authRepository,
      IMapper mapper,
      IValidator<RegisterDto> validator
    ) {
      var results = await validator.ValidateAsync(registerDto);
      if (!results.IsValid)
      {
        return TypedResults.ValidationProblem(results.ToDictionary());
      }
      registerDto.Password = Methods.EncryptSHA256Text(registerDto.Password); // Password to SHA256
      var result = await authRepository.Create(mapper.Map<User>(registerDto));
      if (!result) {
        return TypedResults.BadRequest();
      }
      return TypedResults.Created();
    }

    private static async Task<Results<Ok<TokenDto>, UnauthorizedHttpResult, ValidationProblem>> Login(
      LoginDto loginDto,
      IValidator<LoginDto> validator,
      IAuthRepository authRepository,
      IJwtRepository jwtRepository,
      IConfiguration configuration
    ) {
      var results = await validator.ValidateAsync(loginDto);
      if (!results.IsValid)
      {
        return TypedResults.ValidationProblem(results.ToDictionary());
      }
      var user = await authRepository.Login(loginDto);
      if (user is null) {
        return TypedResults.Unauthorized();
      }
      return TypedResults.Ok(new TokenDto{
        Token = jwtRepository.CreateToken(configuration["JWTSecret"]!, user!),
      });
    } 
  }
}
