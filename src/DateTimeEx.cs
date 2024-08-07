﻿using System;

namespace Pingfan.Kit
{
    /// <summary>
    /// DateTime扩展类
    /// </summary>
    public static class DateTimeEx
    {
        /// <summary>
        /// 当前时区转换成unix秒时间戳, 会忽略本地时区
        /// </summary>
        public static long ToUnixTimeSeconds(this DateTime datetime)
        {
        
            long unixTime = ((DateTimeOffset) datetime).ToUnixTimeSeconds();
            return unixTime;
        }

        /// <summary>
        /// 当前时区转换成unix毫秒时间戳, 会忽略本地时区
        /// </summary>
        public static long ToUnixTimeMilliseconds(this DateTime datetime)
        {
            long unixTime = ((DateTimeOffset) datetime).ToUnixTimeMilliseconds();
            return unixTime;
        }

        /// <summary>
        /// 把秒级unix时间戳转成本地时间
        /// </summary>
        public static DateTime FromUnixTimeSeconds(this long timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).LocalDateTime;
        }

        /// <summary>
        /// 把秒级unix毫秒时间戳转成本地时间
        /// </summary>
        public static DateTime FromUnixTimeMilliseconds(this long timestamp)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).LocalDateTime;
        }

        /// <summary>
        /// 转换成yyyy/MM/dd HH:mm:ss的格式
        /// </summary>
        public static string ToDateTimeString(this DateTime datetime)
        {
            return datetime.ToString("yyyy/MM/dd HH:mm:ss");
        }
    }
}