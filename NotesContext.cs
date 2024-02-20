using Microsoft.EntityFrameworkCore;

using Notes.Data.Models;

namespace Notes
{

  public partial class NotesContext : DbContext
  {
    public DbSet<User> UserModel { get; set; }
    public DbSet<Note> NoteModel { get; set; }

    public NotesContext(DbContextOptions<NotesContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      OnModelCreatingPartial(modelBuilder);
      // Max length
      modelBuilder.Entity<User>().Property(property => property.Username).HasMaxLength(40);
      modelBuilder.Entity<User>().Property(property => property.Email).HasMaxLength(120);
      modelBuilder.Entity<User>().Property(property => property.Password).HasMaxLength(120);
      // Index
      modelBuilder.Entity<User>(entity =>
      {
        entity.HasIndex(e => e.Username).IsUnique();
        entity.HasIndex(e => e.Email).IsUnique();
      });
      modelBuilder.Entity<User>().ToTable("tb_users"); // Table name
      // Notes
      modelBuilder.Entity<Note>().Property(property => property.Title).HasMaxLength(120);
      modelBuilder.Entity<Note>().Property(property => property.Content).HasMaxLength(255);
      modelBuilder.Entity<Note>().ToTable("tb_notes");
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
  }
}
