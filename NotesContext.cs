﻿using Microsoft.EntityFrameworkCore;

namespace notes.Models
{

  public partial class NotesContext : DbContext
  {
    public DbSet<Users> UserModel { get; set; }

    public NotesContext(DbContextOptions<NotesContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      OnModelCreatingPartial(modelBuilder);
      // Max length
      modelBuilder.Entity<Users>().Property(property => property.Username).HasMaxLength(40);
      modelBuilder.Entity<Users>().Property(property => property.Email).HasMaxLength(120);
      modelBuilder.Entity<Users>().Property(property => property.Password).HasMaxLength(120);
      // Index
      modelBuilder.Entity<Users>(entity =>
      {
        entity.HasIndex(e => e.Username).IsUnique();
        entity.HasIndex(e => e.Email).IsUnique();
      });
      modelBuilder.Entity<Users>().ToTable("tb_users"); // Table name
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
  }
}