using System.Net;
using System.Text.Json;
using API.Extensions;
using API.Errors;

namespace API.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;
        await using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        try
        {
            await next(context);

            // if (context.Response?.StatusCode == 400 || context.Response?.StatusCode == 200)
            // {
            //     // Set stream pointer position to 0 before reading
            //     memoryStream.Seek(0, SeekOrigin.Begin);


            //     if (context.Response?.StatusCode == 400)
            //     {
            //         // Read the body from the stream
            //         var responseBodyText = await new StreamReader(memoryStream).ReadToEndAsync();

            //         var _refid = context.Items["ref_id"]?.ToString() ?? "";
            //         var _userId = context.User.GetUserId();
            //         var _module = context.Items["module"]?.ToString() ?? "";
            //         var _data = context.Items["data"]?.ToString() ?? "";
            //         var _ipAddress = context.Items["ipAddress"]?.ToString() ?? "";
            //         var _action = context.Items["action"]?.ToString() ?? "";
            //         var _audit = CommonUtilities.AuditTrails(context, _userId, _module, _action, responseBodyText, _data, context.Request, _refid);
            //         await _unitOfWork.AuditTrailsRepo.Create(_audit);
            //     }

            //     // Reset the position to 0 after reading
            //     memoryStream.Seek(0, SeekOrigin.Begin);

            //     // Do this last, that way you can ensure that the end results end up in the response.
            //     // (This resulting response may come either from the redirected route or other special routes if you have any redirection/re-execution involved in the middleware.)
            //     // This is very necessary. ASP.NET doesn't seem to like presenting the contents from the memory stream.
            //     // Therefore, the original stream provided by the ASP.NET Core engine needs to be swapped back.
            //     // Then write back from the previous memory stream to this original stream.
            //     // (The content is written in the memory stream at this point; it's just that the ASP.NET engine refuses to present the contents from the memory stream.)
            //     context.Response!.Body = originalBodyStream;
            //     await context.Response.Body.WriteAsync(memoryStream.ToArray());
            // }
            // else if (context.Response?.StatusCode == 403)
            // {
            //     // Read the body from the stream
            //     var responseBodyText = await new StreamReader(memoryStream).ReadToEndAsync();

            //     var _refid = context.Items["ref_id"]?.ToString() ?? "";
            //     var _userId = context.User.GetUserId();
            //     var _module = context.Items["module"]?.ToString() ?? "";
            //     var _data = context.Items["data"]?.ToString() ?? "";
            //     var _ipAddress = context.Items["ipAddress"]?.ToString() ?? "";
            //     var _action = context.Items["action"]?.ToString() ?? "";
            //     var _audit = CommonUtilities.AuditTrails(context, _userId, _module, _action, "Your request for access has been denied due to insufficient permissions.", _data, context.Request, _refid);
            //     await _unitOfWork.AuditTrailsRepo.Create(_audit);
            // }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{message}", ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var reponse = env.IsDevelopment()
                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace)
                : new ApiException(context.Response.StatusCode, "Internal Server Error", "An error occured");

            int _userId = 0;
            if (context.User.Identity?.IsAuthenticated == true)
                _userId = context.User.GetUserId();

            // await ErrorService.create(_userId, ex.GetExceptionMessages(), context.GetIpAddress());

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(reponse, options);

            await context.Response.WriteAsync(json);
        }
    }
}
