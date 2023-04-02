using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Pingfan.Kit
{
    /// <summary>
    /// 读写配置类
    /// </summary>
    public static class Config
    {
        private static readonly object _locker = new object();

        public static string MainConfigFilePath =>
            Path.Combine(PathEx.CurrentDirectory, "app.ini");


        /// <summary>
        /// 写入配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(string key, string value)
        {
            lock (_locker)
            {
                key = Regex.Escape(key);
                var lines = FileEx.ReadLines(MainConfigFilePath);
                var sb = new StringBuilder();
                var success = false;
                foreach (var line in lines)
                {
                    if (line.StartsWith(key + "="))
                    {
                        sb.Append(key).Append('=').AppendLine(value);
                        success = true;
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }
                }

                if (!success)
                    sb.Append(key).Append('=').AppendLine(value);

                FileEx.WriteAllText(MainConfigFilePath, sb.ToString());
            }
        }

        /// <summary>
        /// 读取配置, 环境变量->命令行->配置文件->默认值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string Get(string key, string defaultValue = "")
        {
            key = Regex.Escape(key);
            // 如果有环境变量, 则使用环境变量
            var value = Environment.GetEnvironmentVariable(key);
            if (value?.IsNullOrEmpty() == false)
            {
                return value;
            }

            // 如果有命令行参数, 则使用命令行参数
            value = GetByCmd(key);
            if (!value.IsNullOrEmpty())
            {
                return value;
            }

            // 没有配置文件, 默认写入有一个配置文件
            if (!File.Exists(MainConfigFilePath))
            {
                Set(key, defaultValue);
                return defaultValue;
            }

            lock (_locker)
            {
                var lines = FileEx.ReadAllLines(MainConfigFilePath);
                foreach (var line in lines)
                {
                    if (line.StartsWith(key + "="))
                    {
                        return line.Substring(key.Length + 1);
                    }
                }
            }

            Set(key, defaultValue);
            return defaultValue;
        }


        /// <summary>
        /// 从命令行中取参数, 格式为: key=value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetByCmd(string key, string defaultValue = "")
        {
            var m = Regex.Match(Environment.CommandLine, $@"{key}=(\S+)");
            if (m.Success)
            {
                return m.Groups[1].Value;
            }

            return defaultValue;
        }

        /// <summary>
        /// 判断命令行字符串中是否有指定的参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Has(string key)
        {
            key = Regex.Escape(key);
            // 如果有环境变量, 则使用环境变量
            var value = Environment.GetEnvironmentVariable(key);
            if (value?.IsNullOrEmpty() == false)
            {
                return true;
            }

            // 如果有命令行参数, 则使用命令行参数
            value = GetByCmd(key);
            if (!value.IsNullOrEmpty())
            {
                return true;
            }

            // 没有配置文件
            if (!File.Exists(MainConfigFilePath))
            {
                return false;
            }

            lock (_locker)
            {
                var lines = FileEx.ReadAllLines(MainConfigFilePath);
                if (lines.Any(line => line.StartsWith(key + "=")))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Clear()
        {
            return FileEx.Delete(MainConfigFilePath);
        }
    }
}