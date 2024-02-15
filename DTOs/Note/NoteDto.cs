namespace notes.DTOs.Note
{
  public class NoteDto
  {
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;

    public int? UserID { get; set; }
  }
}
