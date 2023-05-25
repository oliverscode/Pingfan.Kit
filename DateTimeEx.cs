using System;

namespace Pingfan.Kit
{
    public static class DateTimeEx
    {
        /// <summary>
        /// 当前时区转换成unix时间戳
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static long ToUnixTimeSeconds(this DateTime datetime)
        {
            long unixTime = ((DateTimeOffset) datetime).ToUnixTimeSeconds();
            return unixTime;
        }

        /// <summary>
        /// 当前时区转换成unix时间戳
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static long ToUnixTimeMilliseconds(this DateTime datetime)
        {
            long unixTime = ((DateTimeOffset) datetime).ToUnixTimeMilliseconds();
            return unixTime;
        }

        /// <summary>
        /// 当前时区unix时间戳转换成DateTime
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimeSeconds(this long timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).LocalDateTime;
        }

        /// <summary>
        /// 当前时区unix时间戳转换成DateTime
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimeMilliseconds(this long timestamp)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).LocalDateTime;
        }

        /// <summary>
        /// 转换成yyyy/MM/dd HH:mm:ss的格式
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string ToDateTimeString(this DateTime datetime)
        {
            return datetime.ToString("yyyy/MM/dd HH:mm:ss");
        }
    }
}