using FluentValidation;
using Notes.Data.DTOs;
using Notes.Data.DTOs.Note;
using Notes.Validations.Note;

namespace Notes.Validations
{
  public static class GlobalValidations
  {
    public static WebApplicationBuilder GlobalValidator(this WebApplicationBuilder builder)
    {
      // User
      builder.Services.AddScoped<IValidator<LoginDto>, LoginValidation>();
      builder.Services.AddScoped<IValidator<RegisterDto>, RegisterValidation>();
      builder.Services.AddScoped<IValidator<UserDto>, UserValidation>();
      // Notes
      builder.Services.AddScoped<IValidator<NoteCreateDto>, NoteCreateValidation>();
      return builder;
    }
  }
}

