namespace Fanex.Logging.CustomTraceListener.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.Practices.EnterpriseLibrary.Caching;
    using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

    internal class CacheService : ICacheService
    {
        private static volatile ICacheManager cacheManager;
        private static volatile HashSet<string> listKey;
        private static object syncRoot = new object();

        public int Count => CacheManager.Count;

        public HashSet<string> Keys
        {
            get
            {
                if (listKey == null)
                {
                    lock (syncRoot)
                    {
                        if (listKey == null)
                        {
                            listKey = new HashSet<string>();
                        }
                    }
                }

                return listKey;
            }
        }

        private static ICacheManager CacheManager
        {
            get
            {
                if (cacheManager == null)
                {
                    lock (syncRoot)
                    {
                        if (cacheManager == null)
                        {
                            try
                            {
#if NET40
                                cacheManager = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();
#else
                                cacheManager = CacheFactory.GetCacheManager();
#endif
                            }
                            catch (Exception ex)
                            {
                                LogToWindowsEvent(ex.Message);
                            }
                        }
                    }
                }

                return cacheManager;
            }
        }

        private static void LogToWindowsEvent(string message)
        {
            var logSource = "Fanex Custom Email Trace Listener";
            var logName = "Application";

            if (!EventLog.SourceExists(logSource))
            {
                EventLog.CreateEventSource(logSource, logName);
            }

            EventLog.WriteEntry(logSource, message, EventLogEntryType.Warning, 234);
        }

        public void Clear()
        {
            CacheManager.Flush();

            Keys.Clear();
        }

        public bool Contains(string key)
            => CacheManager?.Contains(key) ?? false;

        public void Dispose()
        {
            cacheManager = null;
            listKey = null;
        }

        public T Get<T>(string key)
            => (T)CacheManager.GetData(key);

        public void Remove(string key)
        {
            if (Contains(key))
            {
                CacheManager.Remove(key);
                Keys.Remove(key);
            }
        }

        public void Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            var ansoluteTime = new AbsoluteTime(absoluteExpiration);

            Set(key, value, new ICacheItemExpiration[] { ansoluteTime });
        }

        public void Set<T>(string key, T value, TimeSpan slidingExpiration)
        {
            var slidingTime = new SlidingTime(slidingExpiration);

            Set(key, value, slidingTime);
        }

        public void Set<T>(string key, T value, int timeout = 120)
        {
            var absoluteExpiration = DateTime.Now.AddSeconds(timeout);

            Set(key, value, absoluteExpiration);
        }

        public void Set<T>(string key, T value, params ICacheItemExpiration[] expiration)
        {
            if (CacheManager != null)
            {
                CacheManager.Add(key, value, CacheItemPriority.Normal, null, expiration);

                lock (syncRoot)
                {
                    Keys.Add(key);
                }
            }
        }

        public bool TryGet<T>(string key, out T value)
        {
            bool contains = Contains(key);

            value = (T)CacheManager.GetData(key);

            return contains;
        }
    }
}