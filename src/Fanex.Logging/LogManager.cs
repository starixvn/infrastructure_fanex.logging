[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Fanex.Logging.Sentry")]

namespace Fanex.Logging
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class LogManager
    {
        private IEnumerable<ILogEngine> logEngines;
        internal string DefaultLogCategory;
        internal static readonly LogManager Instance = new LogManager();

        internal IEnumerable<ILogEngine> LogEngines
        {
            get
            {
                if (logEngines == null)
                {
                    throw new NotImplementedException("Please set log engines before using");
                }

                return logEngines;
            }
        }

        public LogManager Use(ILogEngine logEngine)
        {
            logEngines = new List<ILogEngine>
            {
                logEngine ?? throw new ArgumentNullException(nameof(logEngine))
            };

            return this;
        }

        public LogManager Use(IEnumerable<ILogEngine> logEngines)
        {
            this.logEngines = logEngines ?? throw new ArgumentNullException(nameof(logEngines));

            return this;
        }

        public static LogManager SetDefaultLogCategory(string defaultLogCategory)
        {
            Instance.DefaultLogCategory = defaultLogCategory;

            return Instance;
        }

        public static string RenderJavascriptLogger()
            => string.Join(
                string.Empty,
                Instance.LogEngines.Select(engine => engine.RenderJavascriptLogger()));
    }
}