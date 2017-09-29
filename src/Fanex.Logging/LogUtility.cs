namespace Fanex.Logging
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using static Constants;

    public static class LogUtility
    {
        /// <summary>
        /// Combine information with format:
        /// <para>Info group:</para>
        ///     <para>_____Info item 1</para>
        ///     <para>_____Info item 2</para>
        ///     <para>_____Info item n</para>
        /// </summary>
        /// <returns></returns>
        public static string CombineInfo(string infoGroup, params string[] infoItems)
            => string.Join(
                NewLine,
                $"{infoGroup}:",
                $"{Tab}{string.Join(Segment, infoItems)}");

       
    }
}