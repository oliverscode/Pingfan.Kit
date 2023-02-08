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

        private static readonly string MainConfigFilePath =
            Path.Combine(PathEx.CurrentDirectory, "app.ini");

        // private static readonly string BakConfigFilePath =
        //     Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "application.ini");

        /// <summary>
        /// 写入配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="configString"></param>
        public static void Set(string key, string configString)
        {
            lock (_Locker)
            {
                var configIni = String.Empty;
                if (File.Exists(MainConfigFilePath))
                {
                    configIni = File.ReadAllText(MainConfigFilePath, Encoding.UTF8);
                }

                var rowData = $"{key}={Regex.Escape(configString)}";
                //判断是否需要更新
                var isMatch = Regex.IsMatch(configIni, rowData);
                if (isMatch)
                    return;


                //判断原来的值是否存在
                rowData = $"{key}=(.*?)\n";
                isMatch = Regex.IsMatch(configIni, rowData);
                if (isMatch)
                {
                    configIni = Regex.Replace(configIni, rowData, $"{key}={configString}\n");
                }
                else
                {
                    configIni += $"{key}={configString}\n";
                }

                File.WriteAllText(MainConfigFilePath, configIni, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string Get(string key, string defaultValue = "")
        {
            // 如果有环境变量, 则使用环境变量
            var value = Environment.GetEnvironmentVariable(key);
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
            
            var configIni = File.ReadAllText(MainConfigFilePath, Encoding.UTF8);
            var m = Regex.Match(configIni, $"{key}=(.*?)\n");
            if (m.Success)
            {
                try
                {
                    var configString = m.Groups[1].Value;
                    configString = Regex.Unescape(configString);
                    return configString;
                }
                catch (Exception e)
                {
                    Set(key, defaultValue);
                    return defaultValue;
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
                try
                {
                    var configString = m.Groups[1].Value;
                    // configString = Regex.Unescape(configString);
                    return configString;
                }
                catch (Exception e)
                {
                    return defaultValue;
                }
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
    }
}