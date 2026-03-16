using System.Security.Cryptography;
using System.Text;

namespace API.Utilities;

public class CommonUtils
{
    // public static AuditTrailsCreateDTO AuditTrails(
    //         HttpContext _context, int _userId, string _module, string _action, string _description, string _data, HttpRequest _request, string _refId = "")
    // {
    //     const string HeaderKeyName = "ip-add";
    //     string? _ipAddress = "";
    //     _request.Headers.TryGetValue(HeaderKeyName, out StringValues headerValue);
    //     if (headerValue.Count() > 0)
    //     {
    //         _ipAddress = headerValue;
    //     }
    //     _context.Items["module"] = _module;
    //     _context.Items["action"] = _action;
    //     _context.Items["data"] = _data;
    //     _context.Items["ipAddress"] = _ipAddress;
    //     _context.Items["ref_id"] = _refId;

    //     return new AuditTrailsCreateDTO
    //     {
    //         userId = _userId,
    //         module = _module,
    //         action = _action,
    //         description = _description,
    //         data = _data,
    //         reqIpAddress = _context.GetIpAddress(),
    //         //ipAddress = _ipAddress!,
    //         ipAddress = _context.GetIpAddress(),
    //         path = _context.Request.Path.Value ?? "",
    //         refId = _refId
    //     };

    // }

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
