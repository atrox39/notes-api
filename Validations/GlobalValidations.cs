using FluentValidation;
using notes.DTOs;

namespace notes.Validations
{
  public static class GlobalValidations
  {
    public static WebApplicationBuilder GlobalValidator(this WebApplicationBuilder builder)
    {
      builder.Services.AddScoped<IValidator<LoginDto>, LoginValidation>();
      builder.Services.AddScoped<IValidator<RegisterDto>, RegisterValidation>();
      builder.Services.AddScoped<IValidator<UserDTO>, UserValidation>();
      return builder;
    }
  }
}

