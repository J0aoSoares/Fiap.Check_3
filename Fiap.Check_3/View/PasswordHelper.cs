using System.Security.Cryptography;
using System.Text;

public static class PasswordHelper
{
    public static void HashPassword(string password, out string hash, out string salt)
    {
        using var rng = RandomNumberGenerator.Create();
        byte[] saltBytes = new byte[16];
        rng.GetBytes(saltBytes);
        salt = Convert.ToBase64String(saltBytes);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256);
        hash = Convert.ToBase64String(pbkdf2.GetBytes(32));
    }

    public static bool VerifyPassword(string password, string hash, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256);
        string hashToCompare = Convert.ToBase64String(pbkdf2.GetBytes(32));
        return hash == hashToCompare;
    }
}