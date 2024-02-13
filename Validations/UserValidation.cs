using FluentValidation;
using notes.DTOs;

namespace notes.Validations
{
  public class UserValidation : AbstractValidator<UserDTO>
  {
    public UserValidation()
    {
      RuleFor(u => u.Id).NotNull();
    }
  }
}
