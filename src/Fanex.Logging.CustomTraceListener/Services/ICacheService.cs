namespace Fanex.Logging.CustomTraceListener.Services
{
    using System;
    using System.Collections.Generic;

    internal interface ICacheService : IDisposable
    {
        int Count { get; }

        HashSet<string> Keys { get; }

        bool Contains(string key);

        T Get<T>(string key);

        bool TryGet<T>(string key, out T value);

        void Set<T>(string key, T value, int timeout = 120);

        void Set<T>(string key, T value, DateTime absoluteExpiration);

        void Set<T>(string key, T value, TimeSpan slidingExpiration);

        void Remove(string key);

        void Clear();
    }
}