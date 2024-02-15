using Microsoft.EntityFrameworkCore;
using notes.DTOs;
using notes.Models;
using notes.Utils;

namespace notes.Repository
{
  public interface IAuthRepository
  {
    public Task<bool> Create(Users registerDto);
    public Task<Users?> Login(LoginDto loginDto);
  }

  public class AuthRepository(NotesContext db) : IAuthRepository
  {
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