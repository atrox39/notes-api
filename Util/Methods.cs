using System.Text;
using System.Security.Cryptography;

namespace Notes.Utils
{
  public static class Methods
  {
    public static string EncryptSHA256Text(string text)
    {
      var crypt = SHA256.Create();
      var hash = new StringBuilder();
      byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(text));
      foreach(byte b in crypto)
      {
        hash.Append(b.ToString("x2"));
      }
      return hash.ToString();
    }

    public static int GetUserID(HttpContext httpContext)
    {
      try
      {
        return int.Parse(httpContext.User.Claims.FirstOrDefault(u => u.Type == "ID")!.Value);
      } catch
      {
        return -1;
      }
    }
  }
}
