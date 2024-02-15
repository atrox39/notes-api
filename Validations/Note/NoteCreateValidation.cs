using FluentValidation;
using notes.DTOs.Note;

namespace notes.Validations.Note
{
  public class NoteCreateValidation : AbstractValidator<NoteCreateDto>
  {
    public NoteCreateValidation()
    {
      RuleFor(n => n.Title)
        .MinimumLength(3)
        .MaximumLength(120)
        .NotEmpty();
      RuleFor(n => n.Content)
        .MinimumLength(1)
        .MaximumLength (255)
        .NotEmpty();
    }
  }
}
