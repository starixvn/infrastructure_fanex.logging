namespace Fanex.Logging
{
    using System;

    /// <summary>
    /// The log services.
    /// </summary>
    public static class LogService
    {
        #region Debug

        public static void Debug(string message)
        {
            Logger.Log.Debug(message);
        }

        public static void Debug(string message, string category)
        {
            Logger.Log.Debug(message, category);
        }

        public static void Debug(string format, params object[] args)
        {
            Logger.Log.Debug(format, args);
        }

        public static void Debug(string format, string category, params object[] args)
        {
            Logger.Log.Debug(format, category, args);
        }

        #endregion

        #region Info

        public static void Info(string message)
        {
            Logger.Log.Info(message);
        }

        public static void Info(string message, string category)
        {
            Logger.Log.Info(message, category);
        }

        public static void Info(string format, params object[] args)
        {
            Logger.Log.Info(format, args);
        }

        public static void Info(string format, string category, params object[] args)
        {
            Logger.Log.Debug(format, category, args);
        }

        #endregion

        #region Warn

        public static void Warn(string message)
        {
            Logger.Log.Warn(message);
        }

        public static void Warn(string message, string category)
        {
            Logger.Log.Warn(message, category);
        }

        public static void Warn(string format, params object[] args)
        {
            Logger.Log.Warn(format, args);
        }

        public static void Warn(string format, string category, params object[] args)
        {
            Logger.Log.Warn(format, category, args);
        }

        #endregion

        #region Error

        public static void Error(string message)
        {
            Logger.Log.Error(message);
        }

        public static void Error(string message, string category)
        {
            Logger.Log.Error(message, category);
        }

        public static void Error(string message, Exception exception)
        {
            Logger.Log.Error(message, exception);
        }

        public static void Error(string message, string category, Exception exception)
        {
            Logger.Log.Error(message, category, exception);
        }

        #endregion

        #region Critical

        public static void Critical(string message)
        {
            Logger.Log.Critical(message);
        }

        public static void Critical(string message, string category)
        {
            Logger.Log.Critical(message, category);
        }

        public static void Critical(string format, params object[] args)
        {
            Logger.Log.Critical(format, args);
        }

        public static void Critical(string format, string category, params object[] args)
        {
            Logger.Log.Critical(format, category, args);
        }

        #endregion


    }
}
