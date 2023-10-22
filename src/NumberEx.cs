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
        
        /// <summary>
        /// 固定在一个范围内
        /// </summary>
        public static int Clamp(this int value, int min, int max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
        
        /// <summary>
        /// 固定在一个范围内
        /// </summary>
        public static uint Clamp(this uint value, uint min, uint max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
        
        /// <summary>
        /// 固定在一个范围内
        /// </summary>
        public static long Clamp(this long value, long min, long max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
        
        /// <summary>
        /// 固定在一个范围内
        /// </summary>
        public static ulong Clamp(this ulong value, ulong min, ulong max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
        
        /// <summary>
        /// 固定在一个范围内
        /// </summary>
        public static short Clamp(this short value, short min, short max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
        
        /// <summary>
        /// 固定在一个范围内
        /// </summary>
        public static ushort Clamp(this ushort value, ushort min, ushort max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
        
        /// <summary>
        /// 将数字转换成枚举, 如果失败, 则返回第一个枚举值
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ToEnum<T>(this int value) where T : Enum
        {
            if (Enum.IsDefined(typeof(T), value))
            {
                return (T)Enum.ToObject(typeof(T), value);
            }
            else
            {
                return (T)Enum.GetValues(typeof(T)).GetValue(0);
            }
        }
    }
}