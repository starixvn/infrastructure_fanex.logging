namespace Fanex.Logging
{
    using System;

    public partial interface ILogger
    {
        void Debug(string message);

        void Debug(string message, string category);

        void Debug(string format, params object[] args);

        void Debug(string format, string category, params object[] args);

        void Info(string message);

        void Info(string message, string category);

        void Info(string format, params object[] args);

        void Info(string format, string category, params object[] args);

        void Warn(string message);

        void Warn(string message, string category);

        void Warn(string format, params object[] args);

        void Warn(string format, string category, params object[] args);

        void Error(string message);

        void Error(string message, string category);

        void Error(string message, Exception exception);

        void Error(string message, string category, Exception exception);

        void Critical(string message);

        void Critical(string message, string category);

        void Critical(string format, params object[] args);

        void Critical(string format, string category, params object[] args);
    }
}