using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Pingfan.Kit
{
    public static class StringEx
    {
        /// <summary>
        /// 忽略大小写判断是否相同
        /// </summary> 
        public static bool EqualsIgnoreCase(this string s1, string s2)
        {
            return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 判断是否包含另外一个字符串
        /// </summary>
        public static bool ContainsIgnoreCase(this string s1, string s2)
        {
            return s1.IndexOf(s2, StringComparison.OrdinalIgnoreCase) >= 0;
        }


        /// <summary>
        /// 是否为NULL或者空字符串
        /// </summary>
        public static bool IsNullOrEmpty(this string s1)
        {
            return string.IsNullOrEmpty(s1);
        }


        /// <summary>
        /// 是否为NULL或者空白字符串
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string s1)
        {
            return string.IsNullOrWhiteSpace(s1);
        }

        /// <summary>
        /// 按分隔符组合一个数组成为一个新的字符串
        /// </summary>
        public static string Join(this string s1, params object[] values)
        {
            return string.Join(s1, values);
        }

        /// <summary>
        /// 格式化一个字符串
        /// </summary>
        public static string Format(this string s1, params object[] values)
        {
            return string.Format(s1, values);
        }


        public static int ToInt(this string str, int defaultValue = 0)
        {
            if (int.TryParse(str, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        public static uint ToUInt(this string str, uint defaultValue = 0)
        {
            if (uint.TryParse(str, out var result))
            {
                return (uint)result;
            }

            return defaultValue;
        }

        public static long ToLong(this string str, long defaultValue = 0)
        {
            if (long.TryParse(str, out var result))
            {
                return (long)result;
            }

            return (long)defaultValue;
        }

        public static ulong ToULong(this string str, ulong defaultValue = 0)
        {
            if (ulong.TryParse(str, out var result))
            {
                return (ulong)result;
            }

            return (ulong)defaultValue;
        }

        public static float ToFloat(this string str, float defaultValue = 0)
        {
            if (float.TryParse(str, out var result))
            {
                return (float)result;
            }

            return (float)defaultValue;
        }

        public static double ToDouble(this string str, double defaultValue = 0)
        {
            if (double.TryParse(str, out var result))
            {
                return (double)result;
            }

            return (double)defaultValue;
        }

        public static decimal ToDecimal(this string str, decimal defaultValue = 0)
        {
            if (decimal.TryParse(str, out var result))
            {
                return (decimal)result;
            }

            return (decimal)defaultValue;
        }

        public static bool ToBool(this string str)
        {
            if (str.IsNullOrWhiteSpace())
                return false;
            if (Regex.IsMatch(str, "null|false|0|undefined|NaN", RegexOptions.IgnoreCase))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 隐藏一部分字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="count">隐藏个数</param>
        /// <returns></returns>
        public static string Hide(this string str, int startIndex, int count)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (startIndex < 0)
            {
                startIndex = 0;
            }

            if (startIndex >= str.Length)
            {
                return str;
            }

            if (count <= 0)
            {
                return str;
            }

            if (startIndex + count > str.Length)
            {
                count = str.Length - startIndex;
            }

            var sb = new StringBuilder(str);
            for (var i = 0; i < count; i++)
            {
                sb[startIndex + i] = '*';
            }

            return sb.ToString();
        }

        /// <summary>
        /// 正则匹配
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static bool IsMatch(this string input, string pattern, RegexOptions options = RegexOptions.None)
        {
            return Regex.IsMatch(input, pattern, options);
        }

        /// <summary>
        /// 正则匹配一个字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="parttern"></param>
        /// <param name="defaultValue"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static List<string> Match(this string input, string pattern, RegexOptions options = RegexOptions.None)
        {
            var list = new List<string>();
            var m = Regex.Match(input, pattern, options);
            if (m.Success == false)
                return list;

            // 除了第一个元素全部返回
            for (var i = 1; i < m.Groups.Count; i++)
            {
                list.Add(m.Groups[i].Value);
            }

            return list;
        }

        /// <summary>
        /// 正则匹配一个字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static List<List<string>> Matches(this string input, string pattern,
            RegexOptions options = RegexOptions.None)
        {
            var list = new List<List<string>>();
            var ms = Regex.Matches(input, pattern, options);
            if (ms.Count == 0)
                return list;


            foreach (Match m in ms)
            {
                if (m.Success == false)
                {
                    continue;
                }

                var item = new List<string>();
                // 除了第一个元素全部返回
                for (var i = 1; i < m.Groups.Count; i++)
                {
                    item.Add(m.Groups[i].Value);
                }

                list.Add(item);
            }

            return list;
        }
    }
}