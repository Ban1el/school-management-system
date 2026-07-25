using API.Attributes;
using API.Constants;
using API.DTOs.AudiTrail;
using API.Extensions;
using API.Options;
using API.Services;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace API.Middleware;

public class AuditTrailMiddleware
{
    private const string Unknown = "Unknown";
    private const string RedactedValue = "***";

    private readonly RequestDelegate _next;
    private readonly HashSet<string> _sensitiveFields;
    private readonly HashSet<string> _excludedPaths;

    public AuditTrailMiddleware(
        RequestDelegate next,
        IOptions<AuditTrailOptions> options)
    {
        _next = next;

        var auditOptions = options.Value;

        _sensitiveFields = new HashSet<string>(
            auditOptions.RedactFields ?? [],
            StringComparer.OrdinalIgnoreCase);

        _excludedPaths = new HashSet<string>(
            auditOptions.ExcludedPaths ?? [],
            StringComparer.OrdinalIgnoreCase);
    }

    public async Task InvokeAsync(
        HttpContext context,
        AuditTrailService auditService)
    {
        if (ShouldSkipAudit(context))
        {
            await _next(context);
            return;
        }

        var requestBody = await ReadRequestBodyAsync(context);
        var parsedRequest = ParseBody(requestBody);

        var originalResponseBody = context.Response.Body;

        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        try
        {
            await _next(context);
        }
        finally
        {
            context.Response.Body = originalResponseBody;
        }

        var responseBody = await ReadResponseBodyAsync(
            memoryStream,
            originalResponseBody);

        var endpoint = context.GetEndpoint();
        var auditAttribute = endpoint?.Metadata.GetMetadata<AuditTrailAttribute>();

        if (auditAttribute?.IsIgnore == true)
            return;

        var module = auditAttribute?.Module ?? Unknown;
        var action = auditAttribute?.Action ?? Unknown;

        var userId = GetUserId(context);

        var parsedResponse = ParseBody(responseBody);

        var refId = context.Items[AuditTrailConstants.ReferenceId]?.ToString() ?? string.Empty;

        await auditService.CreateAsync(CreateAuditDto(
            userId,
            module,
            action,
            context,
            parsedRequest,
            true));

        await auditService.CreateAsync(CreateAuditDto(
            userId,
            module,
            action,
            context,
            parsedResponse,
            false,
            refId));
    }

    private bool ShouldSkipAudit(HttpContext context)
    {
        return _excludedPaths.Any(path =>
            context.Request.Path.StartsWithSegments(path))
            || HttpMethods.IsGet(context.Request.Method);
    }

    private static async Task<string> ReadRequestBodyAsync(HttpContext context)
    {
        context.Request.EnableBuffering();

        using var reader = new StreamReader(
            context.Request.Body,
            leaveOpen: true);

        var body = await reader.ReadToEndAsync();

        context.Request.Body.Position = 0;

        return body;
    }

    private static async Task<string> ReadResponseBodyAsync(
        MemoryStream memoryStream,
        Stream originalResponseBody)
    {
        memoryStream.Position = 0;

        using var reader = new StreamReader(
            memoryStream,
            leaveOpen: true);

        var responseBody = await reader.ReadToEndAsync();

        memoryStream.Position = 0;
        await memoryStream.CopyToAsync(originalResponseBody);

        return responseBody;
    }

    private static int GetUserId(HttpContext context)
    {
        try
        {
            return context.User?.GetUserId() ?? 0;
        }
        catch
        {
            return 0;
        }
    }

    private AuditTrailCreateDto CreateAuditDto(
        int userId,
        string module,
        string action,
        HttpContext context,
        object? data,
        bool isRequest,
        string refId = "")
    {
        return new AuditTrailCreateDto
        {
            UserId = userId,
            Module = module,
            Action = action,
            Path = context.Request.Path,
            Method = context.Request.Method,
            Data = data != null
                ? JsonSerializer.Serialize(data)
                : string.Empty,
            ClientIpAddress = context.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
            IsRequest = isRequest,
            DateCreated = DateTime.UtcNow,
            RefId = refId
        };
    }

    private object? ParseBody(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
            return null;

        try
        {
            var json = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            if (json == null)
                return body;

            foreach (var key in json.Keys.ToList())
            {
                if (_sensitiveFields.Contains(key))
                {
                    json[key] = RedactedValue;
                }
            }

            return json;
        }
        catch
        {
            return body;
        }
    }
}