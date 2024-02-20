using Microsoft.EntityFrameworkCore;
using Notes.Data.Models;

namespace Notes.Repository
{
  public interface INoteRepository
  {
    Task<Note> Create(Note noteCreateDto, int userId);
    Task<List<Note>> ListAll(int userId);
    Task<Note?> GetById(int id, int userId);
    Task<bool> UpdateById(int id, int userId, Note noteUpdateDto);
    Task<bool> DeleteById(int id, int userId);
  }
  public class NoteRepository(NotesContext db) : INoteRepository
  {
    public async Task<Note> Create(Note noteCreateDto, int userId)
    {
      noteCreateDto.UserID = userId;
      var result = await db.NoteModel.AddAsync(noteCreateDto);
      await db.SaveChangesAsync();
      return  result.Entity;
    }

    public async Task<List<Note>> ListAll(int userId)
    {
      return await db.NoteModel.OrderByDescending(n => n.Id).Where(n => n.UserID == userId).ToListAsync();
    }

    public async Task<Note?> GetById(int id, int userId)
    {
      return await db.NoteModel.FirstOrDefaultAsync(n => n.Id == id && n.UserID == userId);
    }

    public async Task<bool> UpdateById(int id, int userId, Note noteUpdateDto)
    {
      var note = db.NoteModel.FirstOrDefault(n => n.Id == id && n.UserID == userId);
      if (note is null)
      {
        return false;
      }
      note.Content = noteUpdateDto.Content;
      await db.SaveChangesAsync();
      return true;
    }

    public async Task<bool> DeleteById(int id, int userId)
    {
      var note = await db.NoteModel.FirstOrDefaultAsync(n => n.Id == id && n.UserID == userId);
      if (note == null)
      {
        return false;
      }
      await db.NoteModel.Where(n => n.Id == id).ExecuteDeleteAsync();
      await db.SaveChangesAsync();
      return true;
    }
  }
}
