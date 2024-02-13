using FluentValidation;
using notes.DTOs;

namespace notes.Validations
{
  public class LoginValidation : AbstractValidator<LoginDto>
  {
    public LoginValidation()
    {
      RuleFor(u => u.Email)
        .MinimumLength(6)
        .EmailAddress()
        .NotEmpty()
        .NotNull();
      RuleFor(u => u.Password)
        .MinimumLength(6)
        .NotEmpty()
        .NotNull();
    }
  }
}

