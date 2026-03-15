using System.Security.Cryptography;
using System.Text;

namespace API.Utilities;

public class CryptoUtils
{
    public static byte[] GenerateSalt(int size = 16)
    {
        return RandomNumberGenerator.GetBytes(size);
    }

    public static string HashPassword(string password, byte[] salt)
    {
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password: Encoding.UTF8.GetBytes(password),
            salt: salt,
            iterations: 100000,
            hashAlgorithm: HashAlgorithmName.SHA512,
            outputLength: 64
        );

        return Convert.ToHexString(hash);
    }

    public static string GenerateRandomKey()
    {
        byte[] key = RandomNumberGenerator.GetBytes(32);
        string result = Convert.ToBase64String(key);
        return result;
    }
}
