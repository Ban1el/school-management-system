using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

public class IdempotencyFilter : IAsyncActionFilter
{
    private readonly IMemoryCache _cache;

    // One lock per idempotency key, not one global lock for everyone
    private static readonly ConcurrentDictionary<Guid, SemaphoreSlim> _locks = new();

    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);

    public IdempotencyFilter(IMemoryCache cache) => _cache = cache;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("Idempotency-Key", out var keyHeader)
            || !Guid.TryParse(keyHeader, out var key))
        {
            context.Result = new BadRequestObjectResult("Missing or invalid Idempotency-Key header.");
            return;
        }

        string cacheKey = $"idempotency:{key}";

        // Fast path: already cached, no need to lock at all
        if (_cache.TryGetValue(cacheKey, out CachedResponse cached))
        {
            WriteCachedResult(context, cached);
            return;
        }

        // Get (or create) a lock specific to this key only
        var keyLock = _locks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));

        await keyLock.WaitAsync();
        try
        {
            // Double-check in case another request with the same key finished while we waited
            if (_cache.TryGetValue(cacheKey, out cached))
            {
                WriteCachedResult(context, cached);
                return;
            }

            var executedContext = await next();

            if (executedContext.Exception != null && !executedContext.ExceptionHandled)
            {
                // Don't cache failures caused by exceptions — let retries actually retry
                return;
            }

            if (executedContext.Result is IStatusCodeActionResult statusResult)
            {
                object? body = executedContext.Result switch
                {
                    ObjectResult objectResult => objectResult.Value,
                    _ => null // e.g. NoContentResult (204) has no body
                };

                int statusCode = statusResult.StatusCode ?? 200;

                // Only cache successful responses (2xx). Don't lock in 4xx/5xx as "the" result.
                if (statusCode is >= 200 and < 300)
                {
                    _cache.Set(cacheKey, new CachedResponse
                    {
                        StatusCode = statusCode,
                        Body = body
                    }, CacheDuration);
                }
            }
        }
        finally
        {
            keyLock.Release();

            // Clean up the lock entry if no one else is waiting on it, to avoid the
            // dictionary growing forever with locks for keys nobody will reuse
            if (keyLock.CurrentCount == 1)
            {
                _locks.TryRemove(key, out _);
            }
        }
    }

    private static void WriteCachedResult(ActionExecutingContext context, CachedResponse cached)
    {
        context.Result = cached.Body is null
            ? new StatusCodeResult(cached.StatusCode)
            : new ObjectResult(cached.Body) { StatusCode = cached.StatusCode };
    }
}

public class CachedResponse
{
    public int StatusCode { get; set; }
    public object? Body { get; set; }
}