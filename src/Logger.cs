using System;
using System.IO;

namespace Pingfan.Kit
{
    /// <summary>
    /// 日志级别
    /// </summary>
    [Flags]
    public enum EnumLogLevel
    {
        /// <summary>
        /// 不记录日志
        /// </summary>
        Non = 0,

        /// <summary>
        /// 调试日志
        /// </summary>
        DBG = 1,

        /// <summary>
        /// 成功日志
        /// </summary>
        SUC = 2,

        /// <summary>
        /// 关键信息状态
        /// </summary>
        INF = 4,

        /// <summary>
        /// 警告日志
        /// </summary>
        WAR = 8,

        /// <summary>
        /// 错误日志
        /// </summary>
        ERR = 16,

        /// <summary>
        /// 宕机日志
        /// </summary>
        FAL = 32,
    }


    /// <summary>
    /// 日志记录器接口
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// 写一个调试日志
        /// </summary>
        /// <param name="logString"></param>
        void Debug(string logString);

        /// <summary>
        /// 写一个错误日志
        /// </summary>
        /// <param name="logString"></param>
        void Error(string logString);

        /// <summary>
        /// 写一个宕机的日志
        /// </summary>
        /// <param name="logString"></param>
        void Fatal(string logString);

        /// <summary>
        /// 写一个关键日志
        /// </summary>
        /// <param name="logString"></param>
        void Info(string logString);

        /// <summary>
        /// 写一个成功日志
        /// </summary>
        /// <param name="logString"></param>
        void Success(string logString);

        /// <summary>
        /// 写一个警告日志
        /// </summary>
        /// <param name="logString"></param>
        void Warning(string logString);

        /// <summary>
        /// 写一行日志
        /// </summary>
        /// <param name="enumLogLevel"></param>
        /// <param name="logString"></param>
        void WriteLine(EnumLogLevel enumLogLevel, string logString);
    }

    /// <summary>
    /// 日志记录器
    /// </summary>
    public class Logger : ILogger
    {
        internal static readonly string RootPath = PathEx.CombineFromCurrentDirectory("log");

        /// <summary>
        /// 日志保留天数
        /// </summary>
        public static int LogKeepDays = Config.Get("LogKeepDays", "365").ToInt(365);


        /// <summary>
        /// 日志文件名, /日期/{*}.log
        /// </summary>
        public string LogFileName { get; set; }

        static Logger()
        {
            // 每60分钟清理一次日志
            Timer.SetIntervalWithTry(1000 * 60 * 60, () =>
            {
                var now = DateTime.Now;
                var dir = PathEx.CombineFromCurrentDirectory("log");
                var paths = DirectoryEx.GetFiles(dir, "*.log");
                foreach (var path in paths)
                {
                    var date = path.Substring(path.LastIndexOfAny(new[] { '\\', '/' }) + 1, 10);
                    if (DateTime.TryParse(date, out var dt))
                    {
                        if (now.Subtract(dt).TotalDays >= LogKeepDays)
                        {
                            FileEx.Delete(path);
                        }
                    }
                }
            });
        }

        /// <inheritdoc />
        public Logger() : this("")
        {
        }

        /// <summary>
        /// 日志文件名
        /// </summary>
        /// <param name="logFileName">{DateTime.Now:yyyy-MM-dd}{logFileName}.log</param>
        public Logger(string logFileName)
        {
            LogFileName = logFileName;
            ConsoleLevel = Config.Get("ConsoleLevel",
                string.Join(",",
                    EnumLogLevel.DBG,
                    EnumLogLevel.SUC,
                    EnumLogLevel.INF,
                    EnumLogLevel.WAR,
                    EnumLogLevel.ERR,
                    EnumLogLevel.FAL
                ));

            FileLevel = Config.Get("FileLevel",
                string.Join(",",
                    EnumLogLevel.SUC,
                    EnumLogLevel.INF,
                    EnumLogLevel.WAR,
                    EnumLogLevel.ERR,
                    EnumLogLevel.FAL
                ));
        }


        /// <summary>
        /// 输出到控制台的级别
        /// </summary>
        public string ConsoleLevel;

        /// <summary>
        /// 输出到磁盘的级别
        /// </summary>
        public string FileLevel;


        /// <summary>
        /// 日志回调, 如果返回true, 则不再输出到控制台以及磁盘
        /// </summary>
        public Func<EnumLogLevel, string, bool>? OnHandler { get; set; }

        /// <summary>
        /// 写一个调试日志
        /// </summary>
        public void Debug(string logString)
        {
            WriteLine(EnumLogLevel.DBG, logString);
        }

        /// <summary>
        /// 写一个错入日志
        /// </summary>
        public void Error(string logString)
        {
            WriteLine(EnumLogLevel.ERR, logString);
        }

        /// <summary>
        /// 写一个宕机日志
        /// </summary>
        public void Fatal(string logString)
        {
            WriteLine(EnumLogLevel.FAL, logString);
        }

        /// <summary>
        /// 写一个关键信息日志
        /// </summary>
        public void Info(string logString)
        {
            WriteLine(EnumLogLevel.INF, logString);
        }

        /// <summary>
        /// 写一个成功日志
        /// </summary>
        public void Success(string logString)
        {
            WriteLine(EnumLogLevel.SUC, logString);
        }

        /// <summary>
        /// 写一个警告日志
        /// </summary>
        public void Warning(string logString)
        {
            WriteLine(EnumLogLevel.WAR, logString);
        }

        /// <summary>
        /// 写一行日志
        /// </summary>
        /// <param name="enumLogLevel">日志级别</param>
        /// <param name="logString">日志内容</param>
        public void WriteLine(EnumLogLevel enumLogLevel, string logString)
        {
            if (!string.IsNullOrWhiteSpace(logString))
            {
                lock (Locker.Get(LogFileName))
                {
                    // 先全局处理
                    var result = this.OnHandler?.Invoke(enumLogLevel, logString);
                    if (result == true)
                        return;

                    var str = $"[{enumLogLevel.ToString()}]{DateTime.Now:yyyy-MM-dd HH:mm:ss} {logString}\n";

                    // 判断是否要输出到控制台
                    if (ConsoleLevel.Contains(enumLogLevel.ToString()))
                    {
                        if (enumLogLevel == EnumLogLevel.DBG)
                        {
                            ConsoleEx.Write(str, ConsoleColor.Cyan);
                        }
                        else if (enumLogLevel == EnumLogLevel.SUC)
                        {
                            ConsoleEx.Write(str, ConsoleColor.Green);
                        }
                        else if (enumLogLevel == EnumLogLevel.INF)
                        {
                            ConsoleEx.Write(str, ConsoleColor.Blue);
                        }
                        else if (enumLogLevel == EnumLogLevel.WAR)
                        {
                            ConsoleEx.Write(str, ConsoleColor.Yellow);
                        }
                        else if (enumLogLevel == EnumLogLevel.ERR)
                        {
                            ConsoleEx.Write(str, ConsoleColor.Red);
                        }
                        else if (enumLogLevel == EnumLogLevel.FAL)
                        {
                            ConsoleEx.Write(str, ConsoleColor.DarkMagenta);
                        }
                        else
                        {
                            Console.Write(str);
                        }
                    }


                    // 判断是否要输出到磁盘
                    if (FileLevel.Contains(enumLogLevel.ToString()))
                    {
                        var logPath = PathEx.Combine(RootPath, $"{DateTime.Now:yyyy-MM-dd}{LogFileName}.log");
                        FileEx.AppendAllText(logPath, str);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 默认的日志记录器
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// 默认的日志记录对象
        /// </summary>
        public static readonly Logger Default = new Logger("");

        /// <summary>
        /// 写一个调试日志
        /// </summary>
        public static void Debug(string logString)
        {
            Default.Debug(logString);
        }

        /// <summary>
        /// 写一个错入日志
        /// </summary>
        public static void Error(string logString)
        {
            Default.Error(logString);
        }

        /// <summary>
        /// 写一个宕机日志
        /// </summary>
        public static void Fatal(string logString)
        {
            Default.Fatal(logString);
        }

        /// <summary>
        /// 写一个关键信息日志
        /// </summary>
        public static void Info(string logString)
        {
            Default.Fatal(logString);
        }

        /// <summary>
        /// 写一个成功日志
        /// </summary>
        public static void Success(string logString)
        {
            Default.Success(logString);
        }

        /// <summary>
        /// 写一个警告日志
        /// </summary>
        public static void Warning(string logString)
        {
            Default.Warning(logString);
        }
    }
}