using System.Text;
using System.Security.Cryptography;

namespace notes.Utils
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
  }
}
