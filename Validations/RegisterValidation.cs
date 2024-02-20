using FluentValidation;
using Notes.Data.DTOs;

namespace Notes.Validations
{
  public class RegisterValidation : AbstractValidator<RegisterDto>
  {
    public RegisterValidation()
    {
      RuleFor(u => u.Username)
        .MinimumLength(6)
        .NotEmpty()
        .NotNull();
      RuleFor(u => u.Email)
        .MinimumLength(6)
        .EmailAddress()
        .NotNull()
        .NotEmpty();
      RuleFor(u => u.Password)
        .MinimumLength(6)
        .NotEmpty()
        .NotNull();
    }
  }
}

