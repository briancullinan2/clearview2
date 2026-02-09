using System;

namespace log4net
{
    public interface ILog
    {
        void Error(string msg);
        void Error(string msg, Exception ex);
        void Debug(string msg);
        void Debug(string msg, Exception ex);
    }

    public static class LogManager { public static ILog GetLogger(Type t) => new ConsoleLogger(); }

    internal class ConsoleLogger : ILog
    {
        public void Error(string msg) { }
        public void Error(string msg, Exception ex) { }
        public void Debug(string msg) { }
        public void Debug(string msg, Exception ex) { }
    }
}
