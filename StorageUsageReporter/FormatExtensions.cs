using System;

namespace StorageUsageReporter
{
    /// <summary>
    /// Helper extension methods to format strings, like convert bytes to appropriately sized units i.e. KB, MB, etc.
    /// </summary>
    internal static class FormatExtensions
    {
        private static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static string ToPrettySize(this long value)
        {
            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }
    }
}
