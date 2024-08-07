﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Pingfan.Kit.Cache;

namespace Pingfan.Kit
{
    /// <summary>
    /// 读写配置类, 支持缓存
    /// </summary>
    public static class Config
    {
        private static readonly object Locker = new object();
        private static readonly ReaderWriterLockSlim CacheLock = new ReaderWriterLockSlim();
        private const string CachePrefixKey = "PingFan.Config.Cache";
        private static readonly ICache CacheMemory = new CacheMemory(CachePrefixKey);

        /// <summary>
        /// 当前配置文件绝对目录
        /// </summary>
        public static string MainConfigFilePath =>
            PathEx.CombineCurrentDirectory("app.ini");

        /// <summary>
        /// 缓存时间, 单位秒, 默认1秒
        /// </summary>
        public static float CacheSeconds { get; set; } = 1;

        /// <summary>
        /// 写入配置
        /// </summary>
        /// <param name="key">配置键</param>
        /// <param name="value">配置值</param>
        public static void Set(string key, string value)
        {
            lock (Locker)
            {
                key = Regex.Escape(key);
                ClearCache();
                var lines = ReadLinesCache();
                var sb = new StringBuilder();
                var success = false;
                foreach (var line in lines!)
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
                ClearCache();
            }
        }

        /// <summary>
        /// 读取配置, 环境变量->命令行->配置文件->默认值
        /// </summary>
        /// <param name="key">配置键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        public static string Get(string key, string defaultValue = "")
        {
            key = Regex.Escape(key);

            var value = Environment.GetEnvironmentVariable(key);
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            value = GetByCmd(key);
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            if (!File.Exists(MainConfigFilePath))
            {
                Set(key, defaultValue);
                return defaultValue;
            }

            var lines = ReadLinesCache();

            foreach (var line in lines!)
            {
                if (line.StartsWith(key + "="))
                {
                    return line.Substring(key.Length + 1);
                }
            }

            Set(key, defaultValue);
            return defaultValue;
        }
        
        
        /// <summary>
        /// 从命令行中取参数, 格式为: key=value
        /// </summary>
        /// <param name="key">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        public static string GetByCmd(string key, string defaultValue = "")
        {
            var m = Regex.Match(Environment.CommandLine, $@"{key}=(\S+)");
            return m.Success ? m.Groups[1].Value : defaultValue;
        }

        /// <summary>
        /// 判断命令行字符串中是否有指定的参数
        /// </summary>
        /// <param name="key">参数名</param>
        /// <returns>是否存在该参数</returns>
        public static bool Has(string key)
        {
            key = Regex.Escape(key);

            var value = Environment.GetEnvironmentVariable(key);
            if (!string.IsNullOrEmpty(value))
            {
                return true;
            }

            value = GetByCmd(key);
            if (!string.IsNullOrEmpty(value))
            {
                return true;
            }

            if (!File.Exists(MainConfigFilePath))
            {
                return false;
            }

            var lines = ReadLinesCache();
            return lines!.Any(line => line.StartsWith(key + "="));
        }

        private static IEnumerable<string>? ReadLinesCache()
        {
            CacheLock.EnterUpgradeableReadLock();
            try
            {
                return CacheMemory.GetOrSet(CachePrefixKey,
                    () =>
                    {
                        if (File.Exists(MainConfigFilePath) == false)
                            return Array.Empty<string>();
                        return FileEx.ReadLines(MainConfigFilePath).ToArray();
                    },
                    CacheSeconds);
            }
            finally
            {
                CacheLock.ExitUpgradeableReadLock();
            }
        }

        private static void ClearCache()
        {
            CacheMemory.Clear();
        }

        /// <summary>
        /// 清理缓存和配置文件
        /// </summary>
        /// <returns>配置文件是否删除成功</returns>
        public static bool Clear()
        {
            return FileEx.Delete(MainConfigFilePath);
        }
    }
}