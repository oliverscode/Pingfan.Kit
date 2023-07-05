using System;

namespace Pingfan.Kit
{

    /// <summary>
    /// 日志级别
    /// </summary>
    public enum LogLevel
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


    public interface ILog
    {

        void Debug(string logString);
        void Error(string logString, Exception e = null);
        void Fatal(string logString);
        void Info(string logString);
        void Success(string logString);
        void Warning(string logString);
        void WriteLine(LogLevel logLevel, string logString);
    }

    /// <summary>
    /// 日志记录器
    /// </summary>
    public class Log : ILog
    {
        /// <summary>
        /// 日志文件名, /日期/{*}.log
        /// </summary>
        public string LogFileName { get; set; }

        private static readonly string _RootPath = PathEx.CombineFromCurrentDirectory("log");

        /// <summary>
        /// 默认的日志记录对象
        /// </summary>
        public static Log Default { get; } = new Log("");

        public Log()
        {
#if DEBUG
            ConsoleLevel = LogLevel.DBG | LogLevel.SUC | LogLevel.INF | LogLevel.WAR | LogLevel.ERR | LogLevel.FAL;
#endif
        }

        public Log(string logFileName)
        {
            LogFileName = logFileName;
        }

        /// <summary>
        /// 输出到控制台的级别
        /// </summary>
        public LogLevel ConsoleLevel { get; set; }

        /// <summary>
        /// 输出到磁盘的级别
        /// </summary>
        public LogLevel FileLevel { get; set; } =
            LogLevel.SUC | LogLevel.INF | LogLevel.WAR | LogLevel.ERR | LogLevel.FAL;

        /// <summary>
        /// 日志回调
        /// </summary>
        public event Action<LogLevel, string> OnHandler;

        /// <summary>
        /// 写一个调试日志
        /// </summary>
        public void Debug(string logString)
        {
            WriteLine(LogLevel.DBG, logString);
        }

        /// <summary>
        /// 写一个错入日志
        /// </summary>
        public void Error(string logString, Exception e = null)
        {
            WriteLine(LogLevel.ERR, logString + Environment.NewLine + e);
        }

        /// <summary>
        /// 写一个宕机日志
        /// </summary>
        public void Fatal(string logString)
        {
            WriteLine(LogLevel.FAL, logString);
        }

        /// <summary>
        /// 写一个关键信息日志
        /// </summary>
        public void Info(string logString)
        {
            WriteLine(LogLevel.INF, logString);
        }

        /// <summary>
        /// 写一个成功日志
        /// </summary>
        public void Success(string logString)
        {
            WriteLine(LogLevel.SUC, logString);
        }

        /// <summary>
        /// 写一个警告日志
        /// </summary>
        public void Warning(string logString)
        {
            WriteLine(LogLevel.WAR, logString);
        }

        /// <summary>
        /// 写一行日志
        /// </summary>
        /// <param name="logLevel">日志级别</param>
        /// <param name="logString">日志内容</param>
        public void WriteLine(LogLevel logLevel, string logString)
        {
            if (!string.IsNullOrWhiteSpace(logString))
            {
                lock (this)
                {
                    // 先全局处理
                    this.OnHandler?.Invoke(logLevel, logString);

                    var str = $"[{logLevel.ToString()}]{DateTime.Now:yyyy-MM-dd HH:mm:ss} {logString}\n";

                    // 判断是否要输出到控制台
                    if ((logLevel & this.ConsoleLevel) != 0)
                    {
                        if (logLevel == LogLevel.DBG)
                        {
                            ConsoleEx.Write(logString, ConsoleColor.Cyan);
                        }
                        else if (logLevel == LogLevel.SUC)
                        {
                            ConsoleEx.Write(logString, ConsoleColor.Green);
                        }
                        else if (logLevel == LogLevel.INF)
                        {
                            ConsoleEx.Write(logString, ConsoleColor.Blue);
                        }
                        else if (logLevel == LogLevel.WAR)
                        {
                            ConsoleEx.Write(logString, ConsoleColor.Yellow);
                        }
                        else if (logLevel == LogLevel.ERR)
                        {
                            ConsoleEx.Write(logString, ConsoleColor.Red);
                        }
                        else if (logLevel == LogLevel.FAL)
                        {
                            ConsoleEx.Write(logString, ConsoleColor.DarkMagenta);
                        }
                        else
                        {
                            Console.Write(logString);
                        }
                    }


                    // 判断是否要输出到磁盘
                    if ((logLevel & this.FileLevel) != 0)
                    {
                        var logPath = PathEx.Combine(_RootPath, $"{DateTime.Now:yyyy-MM-dd}{LogFileName}.log");
                        FileEx.AppendAllText(logPath, str);
                    }
                }
            }
        }
    }
}