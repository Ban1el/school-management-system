using System;

namespace API.Extensions;

public static class ExceptionExtensions
{
    public static string GetExceptionMessages(this Exception? exception, int messageCount = 1)
    {
        return
         exception == null
             ? string.Empty
             : $"Exception: {Environment.NewLine}{messageCount}: {exception.Message}{Environment.NewLine}{GetExceptionMessages(exception.InnerException, ++messageCount)}StackTrace: {exception.StackTrace}";
    }
}
