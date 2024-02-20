using FluentValidation;
using Notes.Data.DTOs;

namespace Notes.Validations
{
  public class UserValidation : AbstractValidator<UserDto>
  {
    public UserValidation()
    {
      RuleFor(u => u.Id).NotNull();
    }
  }
}
