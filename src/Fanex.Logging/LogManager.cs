namespace Fanex.Logging
{
    using System;

    public class LogManager
    {
        private ILogEngine logEngine;
        internal string DefaultLogCategory;
        internal static readonly LogManager Instance = new LogManager();

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

        public static LogManager SetDefaultLogCategory(string defaultLogCategory)
        {
            Instance.DefaultLogCategory = defaultLogCategory;

            return Instance;
        }
    }
}