using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;


namespace Pingfan.Kit
{
    /// <summary>
    /// 读写配置类
    /// </summary>
    public class Config
    {
        private static object _Locker = new object();

        public static string MainConfigFilePath =>
            Path.Combine(PathEx.CurrentDirectory, "app.ini");


        /// <summary>
        /// 写入配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(string key, string value)
        {
            lock (_Locker)
            {
                key = Regex.Escape(key);
                
                var lines = FileEx.ReadLines(MainConfigFilePath);
                var sb = new StringBuilder();
                var success = false;
                foreach (var line in lines)
                {
                    if (line.StartsWith(key + "="))
                    {
                        sb.AppendLine($"{key}={value}");
                        success = true;
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }
                }

                if (success == false)
                    sb.AppendLine($"{key}={value}");

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
            if (value.IsNullOrEmpty() == false)
            {
                return value;
            }

            // 没有配置文件, 默认写入有一个配置文件
            if (File.Exists(MainConfigFilePath) == false)
            {
                Set(key, defaultValue);
                return defaultValue;
            }

            lock (_Locker)
            {
                var lines = FileEx.ReadAllLines(MainConfigFilePath);
                foreach (var line in lines)
                {
                    if (line.StartsWith(key + "="))
                    {
                        value = line.Substring(key.Length + 1);
                        return value;
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
                var value = m.Groups[1].Value;
                return value;
            }

            return defaultValue;
        }

        /// <summary>
        /// 判断命令行字符串中是否有指定的参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool HasCmd(string key)
        {
            return Environment.CommandLine.ContainsIgnoreCase(key);
        }


        public static bool Clear()
        {
            return FileEx.Delete(MainConfigFilePath);
        }
    }
}