using log4net;
using System.IO;
using System.Runtime.CompilerServices;


namespace EPIC.ClearView.Utilities.Logging
{
    public static class Log
    {
        // Use a ConcurrentDictionary to cache loggers for performance
        private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, ILog> _loggerCache = new();

        public static void Error(object message, Exception? ex = null, [CallerFilePath] string callerPath = "")
        {
            GetLogger(callerPath).Error(message, ex);
        }

        public static void Fatal(object message, Exception? ex = null, [CallerFilePath] string callerPath = "")
        {
            GetLogger(callerPath).Fatal(message, ex);
            App.Current.Shutdown();
        }

        public static void Debug(object message, Exception? ex = null, [CallerFilePath] string callerPath = "")
        {
            GetLogger(callerPath).Debug(message, ex);
        }

        private static ILog GetLogger(string filePath)
        {
            // Extracts the class name from the file path to use as the category
            string category = Path.GetFileNameWithoutExtension(filePath);
            return _loggerCache.GetOrAdd(category, LogManager.GetLogger);
        }
    }
}
