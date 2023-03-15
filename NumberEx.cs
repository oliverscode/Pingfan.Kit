using System;

namespace Pingfan.Kit
{
    public static class NumberEx
    {
        /// <summary>
        /// 计算2个数 大小之比
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <param name="errRate">默认1％</param>
        /// <returns></returns>
        public static bool ErrorRate(this decimal num1, decimal num2, decimal errRate = 0.01m)
        {
            var max = Math.Max(num1, num2);
            var min = Math.Min(num1, num2);
            return (max - min) / min <= errRate || (max - min) / max <= errRate;
        }

        /// <summary>
        /// 计算2个数 大小之比
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <param name="errRate">默认1％</param>
        /// <returns></returns>
        public static bool ErrorRate(this long num1, long num2, decimal errRate = 0.01m)
        {
            var max = Math.Max(num1, num2);
            var min = Math.Min(num1, num2);
            return (max - min) / min <= errRate || (max - min) / max <= errRate;
        }

        /// <summary>
        /// 计算2个数 大小之比
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <param name="errRate">默认1％</param>
        /// <returns></returns>
        public static bool ErrorRate(this int num1, int num2, decimal errRate = 0.01m)
        {
            var max = Math.Max(num1, num2);
            var min = Math.Min(num1, num2);
            return (max - min) / min <= errRate || (max - min) / max <= errRate;
        }

        /// <summary>
        /// 转成含有B KB MB GB的字符串
        /// </summary>
        /// <param name="size"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static string ToDataSize(this long size, int digits = 1)
        {
            var decimalPlaces = new string[] {"B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"};
            var mag = (int) Math.Max(0, Math.Log(size, 1024));
            var adjustedSize = Math.Round(size / Math.Pow(1024, mag), digits) + decimalPlaces[mag];
            return adjustedSize;
        }
    }
}