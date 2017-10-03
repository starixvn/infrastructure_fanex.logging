namespace Fanex.Logging.CustomTraceListener.Services
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal interface ICacheService : IDisposable
    {
        /// <summary>
        /// Gets the number of item in current cache instance.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        int Count { get; }

        /// <summary>
        /// Determines whether cache contains the specified key.
        /// </summary>
        /// <param name="key">The string key.</param>
        /// <returns>
        ///   <c>True</c> if [contains] [the specified key]; otherwise, <c>false</c>.
        /// </returns>
        bool Contains(string key);

        /// <summary>
        /// Gets cache item by the specified key.
        /// </summary>
        /// <typeparam name="T">Type of cache item.</typeparam>
        /// <param name="key">The string key.</param>
        /// <returns>Cache item.</returns>
        T Get<T>(string key);

        /// <summary>
        /// Get cache item by specified key, and cast to type T
        /// if cast failed return out T.
        /// </summary>
        /// <typeparam name="T">Type of cache item.</typeparam>
        /// <param name="key">The string key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True/false to indicate whether getting cache item and casting successful or not.</returns>
        bool TryGet<T>(string key, out T value);

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <typeparam name="T">Type of cache item.</typeparam>
        /// <param name="key">The string key.</param>
        /// <param name="value">The value.</param>
        /// <param name="timeOut">The second time out, default is 120s.</param>
        void Set<T>(string key, T value, int timeOut = 120);

        /// <summary>
        /// <para>Let’s say, the cache item is created at 8:05:00.</para>
        /// <para>and the lifetime is given to 5 seconds. </para>
        /// <para>Then the lifetime of the cache item expires at 8:05:05.</para>
        /// </summary>
        /// <typeparam name="T">Type of cache item.</typeparam>
        /// <param name="key">The string key.</param>
        /// <param name="value">The value.</param>
        /// <param name="absoluteExpiration">DateTime AbsoluteExpiration.</param>
        void Set<T>(string key, T value, DateTime absoluteExpiration);

        /// <summary>
        /// <para>Let’s say, the cache item is created at 8:05:00.</para>
        /// <para>and the lifetime is given to 5 seconds.</para>
        /// <para>If an access to the cache item happen at 8:05:04.</para>
        /// <para>the expiring time of the cache item will be updated to 8:05:09.</para>
        /// </summary>
        /// <typeparam name="T">Type of cache item.</typeparam>
        /// <param name="key">The string key.</param>
        /// <param name="value">The value.</param>
        /// <param name="slidingExpiration">Sliding Expiration.</param>
        void Set<T>(string key, T value, TimeSpan slidingExpiration);

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The string key.</param>
        void Remove(string key);

        /// <summary>
        /// Clears all.
        /// </summary>
        void Clear();
    }
}