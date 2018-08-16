namespace Fanex.Logging
{
    using System;

    public class LogManager
    {
        private ILogEngine logEngine;
        internal string DefaultLogCategory;
        internal static LogManager Instance;

        public static LogManager CreateInstance()
        {
            if (Instance == null)
            {
                Instance = new LogManager();
            }

            return Instance;
        }

        internal ILogEngine LogEngine
        {
            get
            {
                if (logEngine == null)
                {
                    throw new NotImplementedException("Please set log engine before using");
                }

                return logEngine;
            }
        }

        public LogManager Use(ILogEngine logEngine)
        {
            this.logEngine = logEngine;

            return this;
        }

        public LogManager SetDefaultLogCategory(string defaultLogCategory)
        {
            DefaultLogCategory = defaultLogCategory;

            return this;
        }
    }
}