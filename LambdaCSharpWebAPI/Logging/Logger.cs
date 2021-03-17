using Amazon.Lambda.Core;

namespace LambdaCSharpWebAPI.Logging
{
    public static class Logger
    {
        public static string LogLevel { get; set; }

        public static void LogDebug(string message, string method, string className)
        {
            if (LogLevel.ToUpper() == "DEBUG")
            {
                LambdaLogger.Log($"DEBUG: |ClassName: {className} |Method: {method} |Message: {message}");
            }
        }
        public static void LogInformation(string message, string method, string className)
        {
            LambdaLogger.Log($"INFORMATION: |ClassName: {className} |Method: {method} |Message: {message}");
        }
        public static void LogError(string message, string method, string className, string exceptionMessage)
        {
            LambdaLogger.Log($"ERROR: |ClassName: {className} |Method: {method} |Message: {message} |Exception: {exceptionMessage}");
        }
    }
}
