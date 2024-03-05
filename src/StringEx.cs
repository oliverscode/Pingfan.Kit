using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Pingfan.Kit
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
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
        /// 判断2个字符串相似百分比, 乱序不影响, 返回结果为0-1之间
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        public static double Similarity(this string s1, string s2)
        {
            var shortStr = s1.Length > s2.Length ? s2 : s1;
            var longStr = s1.Length > s2.Length ? s1 : s2;

            double value = 1;
            var per = 1.0 / shortStr.Length;
            foreach (var c in shortStr)
            {
                if (!longStr.Contains(c.ToString()))
                {
                    value -= per;
                }
            }

            return value;
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
        public static bool IsNullOrEmpty(this string? s1) => string.IsNullOrEmpty(s1);

        /// <summary>
        /// 不为NULL或者空字符串
        /// </summary>
        public static bool NotNull(this string? s1) => !string.IsNullOrEmpty(s1);

        /// <summary>
        /// 是否为NULL或者空白字符串
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string? s1) => string.IsNullOrWhiteSpace(s1);

        /// <summary>
        /// 是否为数字, 包含浮点数
        /// </summary>
        public static bool IsNumber(this string s1) => Regex.IsMatch(s1, @"^[-+]?[0-9]*\.?[0-9]+$");

        /// <summary>
        /// 是否为整数, 包含负数, 以及大数
        /// </summary>
        public static bool IsInt(this string s1) => Regex.IsMatch(s1, @"^[-+]?[0-9]+$");

        /// <summary>
        /// 格式化一个字符串
        /// </summary>
        public static string Format(this string s1, params object[] values) => string.Format(s1, values);

        /// <summary>
        /// 转成整数, 如果转换失败, 返回默认值
        /// </summary>
        public static int ToInt(this string str, int defaultValue = 0)
        {
            if (string.IsNullOrEmpty(str)) return defaultValue;

            // 提取str中所有的数字部分
            var match = Regex.Match(str, @"[-+]?[0-9]+");
            if (match.Success)
                str = match.Value;


            if (int.TryParse(str, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// 转成无符号整数, 如果转换失败, 返回默认值
        /// </summary>
        public static uint ToUInt(this string str, uint defaultValue = 0)
        {
            if (string.IsNullOrEmpty(str)) return defaultValue;

            // 提取str中所有的数字部分
            var match = Regex.Match(str, @"[-+]?[0-9]+");
            if (match.Success)
                str = match.Value;

            if (uint.TryParse(str, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// 转成长整数, 如果转换失败, 返回默认值
        /// </summary>
        public static long ToLong(this string str, long defaultValue = 0)
        {
            if (string.IsNullOrEmpty(str)) return defaultValue;

            // 提取str中所有的数字部分
            var match = Regex.Match(str, @"[-+]?[0-9]+");
            if (match.Success)
                str = match.Value;

            if (long.TryParse(str, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// 转成无符号长整数, 如果转换失败, 返回默认值
        /// </summary>
        public static ulong ToULong(this string str, ulong defaultValue = 0)
        {
            if (string.IsNullOrEmpty(str)) return defaultValue;

            // 提取str中所有的数字部分
            var match = Regex.Match(str, @"[-+]?[0-9]+");
            if (match.Success)
                str = match.Value;

            if (ulong.TryParse(str, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// 转成单精度浮点数, 如果转换失败, 返回默认值
        /// </summary>
        public static float ToFloat(this string str, float defaultValue = 0)
        {
            if (string.IsNullOrEmpty(str)) return defaultValue;

            if (float.TryParse(str, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// 转成双精度浮点数, 如果转换失败, 返回默认值
        /// </summary>
        public static double ToDouble(this string str, double defaultValue = 0)
        {
            if (string.IsNullOrEmpty(str)) return defaultValue;
            if (double.TryParse(str, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// 转成decimal, 如果转换失败, 返回默认值
        /// </summary>
        public static decimal ToDecimal(this string str, decimal defaultValue = 0)
        {
            if (string.IsNullOrEmpty(str)) return defaultValue;

            if (decimal.TryParse(str, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// 先转成int, 再判断是否为0
        /// </summary>
        public static bool ToBool(this string str) => str.ToInt() != 0;


        /// <summary>
        /// 转成时间, 如果转换失败, 返回默认值
        /// </summary>
        public static DateTime ToDatetime(this string str, DateTime defaultValue = default)
        {
            if (string.IsNullOrEmpty(str)) return defaultValue;
            if (DateTime.TryParse(str, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// 隐藏一部分字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="count">隐藏个数</param>
        /// <param name="defaultChar">默认*</param>
        /// <returns></returns>
        public static string Hide(this string str, int startIndex, int count, char defaultChar = '*')
        {
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
                sb[startIndex + i] = defaultChar;
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取一段文本的中间部分, 如果没有找到, 则返回空字符串
        /// </summary>
        public static string Between(this string str, string start, string end,
            StringComparison stringComparison = StringComparison.Ordinal)
        {
            var startIndex = str.IndexOf(start, stringComparison);
            if (startIndex < 0)
            {
                return string.Empty;
            }

            startIndex += start.Length;
            var endIndex = str.IndexOf(end, startIndex, stringComparison);
            if (endIndex < 0)
            {
                return string.Empty;
            }

            return str.Substring(startIndex, endIndex - startIndex);
        }

        /// <summary>
        /// 获取一段文本的中间部分, 但是从后面找, 如果没有找到, 则返回空字符串
        /// </summary>
        public static string BetweenLast(this string str, string start, string end,
            StringComparison stringComparison = StringComparison.Ordinal)
        {
            var startIndex = str!.LastIndexOf(start, stringComparison);
            if (startIndex < 0)
            {
                return string.Empty;
            }

            startIndex += start.Length;
            var endIndex = str.LastIndexOf(end, startIndex, stringComparison);
            if (endIndex < 0)
            {
                return string.Empty;
            }

            return str.Substring(startIndex, endIndex - startIndex);
        }

        /// <summary>
        /// 正则匹配
        /// </summary>
        public static bool IsMatch(this string input, string pattern, RegexOptions options = RegexOptions.None)
        {
            return Regex.IsMatch(input, pattern, options);
        }

        /// <summary>
        /// 条件成立时, 返回字符串, 否则返回空字符串
        /// </summary>
        public static string If(this string str, bool condition)
        {
            return condition ? str : string.Empty;
        }

        /// <summary>
        /// 正则匹配一个字符串
        /// </summary>
        public static List<string> Match(this string input, string pattern, RegexOptions options = RegexOptions.None)
        {
            var list = new List<string>();
            var m = Regex.Match(input, pattern, options);
            if (!m.Success)
                return list;

            // 除了第一个元素全部返回, 除非只有一个元素
            if (m.Groups.Count == 1)
            {
                list.Add(m.Groups[0].Value);
                return list;
            }
            else
            {
                for (var i = 1; i < m.Groups.Count; i++)
                {
                    list.Add(m.Groups[i].Value);
                }

                return list;
            }
        }

        /// <summary>
        /// 正则匹配一个字符串
        /// </summary>
        public static Table<string> Matches(this string input, string pattern,
            RegexOptions options = RegexOptions.None)
        {
            var list = new Table<string>();
            var ms = Regex.Matches(input, pattern, options);
            if (ms.Count == 0)
                return list;
            foreach (Match m in ms)
            {
                if (m.Success == false)
                {
                    continue;
                }

                // ReSharper disable once CollectionNeverQueried.Local
                var item = new List<string>();
                // 除了第一个元素全部返回, 除非只有一个元素
                if (m.Groups.Count == 1)
                {
                    item.Add(m.Groups[0].Value);
                }
                else
                {
                    for (var i = 1; i < m.Groups.Count; i++)
                    {
                        item.Add(m.Groups[i].Value);
                    }
                }

                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// 拆分字符串
        /// </summary>
        public static string[] Split(this string str, params string[] args)
        {
            return str.Split(args, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 拆分字符串
        /// </summary>
        public static string[] Split(this string str, params char[] args)
        {
            return str.Split(args, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}