using Microsoft.EntityFrameworkCore;
using Notes.Utils;
using Notes.Data.DTOs;
using Notes.Data.Models;

namespace Notes.Repository
{
  public interface IAuthRepository
  {
    public Task<bool> Create(User registerDto);
    public Task<User?> Login(LoginDto loginDto);
  }

  public class AuthRepository(NotesContext db) : IAuthRepository
  {
    public async Task<bool> Create(User user) {
      try {
        await db.UserModel.AddAsync(user);
        await db.SaveChangesAsync();
        return true;
      } catch {
        return false;
      }
    }

    public async Task<User?> Login(LoginDto loginDto)
    {
      var user = await db.UserModel.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
      if (user is null) {
        return null;
      }
      if (user.Password.ToLower() == Methods.EncryptSHA256Text(loginDto.Password).ToLower())
      {
        return user;
      }
      return null;
    }
  }
}