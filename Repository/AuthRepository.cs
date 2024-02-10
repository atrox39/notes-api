using Microsoft.EntityFrameworkCore;
using notes.DTOs;
using notes.Models;

namespace notes.Respository
{
  public interface IAuthRepository
  {
    public Task<bool> Create(Users registerDto);
    public Task<Users?> Login(LoginDto loginDto);
  }

  public class AuthRepository : IAuthRepository
  {
    private readonly NotesContext db;

    public AuthRepository(NotesContext db) { this.db = db; }

    public async Task<bool> Create(Users user) {
      try {
        await db.UserModel.AddAsync(user);
        await db.SaveChangesAsync();
        return true;
      } catch {
        return false;
      }
    }

    public async Task<Users?> Login(LoginDto loginDto)
    {
      var user = await db.UserModel.FirstOrDefaultAsync(u => u.Email.ToLower() == loginDto.Email.ToLower() && u.Password.Equals(loginDto.Password));
      return user;
    }
  }
}