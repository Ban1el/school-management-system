using System.Security.Cryptography;
using System.Text;

namespace API.Utilities;

public class CommonUtils
{
    public static string RandomString(int length = 8)
    {
        if (length <= 0) throw new ArgumentException("Length must be greater than zero.");

        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var result = new StringBuilder(length);

        byte[] data = RandomNumberGenerator.GetBytes(length);

        for (int i = 0; i < length; i++)
        {
            int index = data[i] % chars.Length;
            result.Append(chars[index]);
        }

        return result.ToString();
    }
}
