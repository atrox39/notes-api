using FluentValidation;
using notes.DTOs;
using notes.DTOs.Note;
using notes.Validations.Note;

namespace notes.Validations
{
  public static class GlobalValidations
  {
    public static WebApplicationBuilder GlobalValidator(this WebApplicationBuilder builder)
    {
      // User
      builder.Services.AddScoped<IValidator<LoginDto>, LoginValidation>();
      builder.Services.AddScoped<IValidator<RegisterDto>, RegisterValidation>();
      builder.Services.AddScoped<IValidator<UserDTO>, UserValidation>();
      // Notes
      builder.Services.AddScoped<IValidator<NoteCreateDto>, NoteCreateValidation>();
      return builder;
    }
  }
}

