namespace notes.Models
{
  public class Note
  {
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;

    public int? UserID { get; set; }
    public virtual Users User { get; set; } = null!;
  }
}
