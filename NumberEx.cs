using System;

namespace Pingfan.Kit
{
    /// <summary>
    /// 数字扩展
    /// </summary>
    public static class NumberEx
    {
        /// <summary>
        /// 计算2个数大小是否在小于等于一个比例, 默认1％
        /// </summary>
        public static bool ErrorRate(this decimal num1, decimal num2, decimal errRate = 0.01m)
        {
            var max = Math.Max(num1, num2);
            var min = Math.Min(num1, num2);
            return (max - min) / min <= errRate || (max - min) / max <= errRate;
        }

        /// <summary>
        /// 计算2个数大小是否在小于等于一个比例, 默认1％
        /// </summary>
        public static bool ErrorRate(this long num1, long num2, double errRate = 0.01d)
        {
            var max = Math.Max(num1, num2) * 1.0d;
            var min = Math.Min(num1, num2);
            return ((max - min) / min) <= errRate || (max - min) / max <= errRate;
        }

        /// <summary>
        /// 计算2个数大小是否在小于等于一个比例, 默认1％
        /// </summary>
        public static bool ErrorRate(this int num1, int num2, double errRate = 0.01d)
        {
            var max = Math.Max(num1, num2) * 1.0d;
            var min = Math.Min(num1, num2);
            return (max - min) / min <= errRate || (max - min) / max <= errRate;
        }

        /// <summary>
        /// 计算2个数大小是否在小于等于一个比例, 默认1％
        /// </summary>
        public static bool ErrorRate(this double num1, double num2, double errRate = 0.01d)
        {
            var max = Math.Max(num1, num2);
            var min = Math.Min(num1, num2);
            return (max - min) / min <= errRate || (max - min) / max <= errRate;
        }

        /// <summary>
        /// 转成含有B KB MB GB的字符串
        /// </summary>
        public static string ToDataSize(this long size, int digits = 1)
        {
            var decimalPlaces = new[] { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            var mag = (int)Math.Max(0, Math.Log(size, 1024));
            var adjustedSize = Math.Round(size / Math.Pow(1024, mag), digits) + decimalPlaces[mag];
            return adjustedSize;
        }
        
        /// <summary>
        /// 转成含有B KB MB GB的字符串
        /// </summary>
        public static string ToDataSize(this ulong size, int digits = 1)
        {
            var decimalPlaces = new[] { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            var mag = (int)Math.Max(0, Math.Log(size, 1024));
            var adjustedSize = Math.Round(size / Math.Pow(1024, mag), digits) + decimalPlaces[mag];
            return adjustedSize;
        }
    }
}