using System;

namespace Pingfan.Kit
{
    /// <summary>
    /// 日志级别
    /// </summary>
    [Flags]
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


    /// <summary>
    /// 日志记录器接口
    /// </summary>
    public interface ILog
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
        /// <param name="e"></param>
        void Error(string logString, Exception e = null);
        
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
        /// <param name="logLevel"></param>
        /// <param name="logString"></param>
        void WriteLine(LogLevel logLevel, string logString);
    }

    /// <summary>
    /// 日志记录器
    /// </summary>
    public class Log : ILog
    {
        private static readonly string RootPath = PathEx.CombineFromCurrentDirectory("log");

        /// <summary>
        /// 默认的日志记录对象
        /// </summary>
        public static Log Default { get; } = new Log("");
        
        
        
        /// <summary>
        /// 日志文件名, /日期/{*}.log
        /// </summary>
        public string LogFileName { get; set; }

        public Log()
        {
        }

        public Log(string logFileName)
        {
            LogFileName = logFileName;
        }

        /// <summary>
        /// 输出到控制台的级别
        /// </summary>
        public LogLevel ConsoleLevel { get; set; } =
            LogLevel.DBG | LogLevel.SUC | LogLevel.INF | LogLevel.WAR | LogLevel.ERR | LogLevel.FAL;

        /// <summary>
        /// 输出到磁盘的级别
        /// </summary>
        public LogLevel FileLevel { get; set; } = (LogLevel)(Config.Get("LogLevel",
            (LogLevel.SUC | LogLevel.INF | LogLevel.WAR | LogLevel.ERR | LogLevel.FAL).ToString()).ToInt());


        /// <summary>
        /// 日志回调, 如果返回true, 则不再输出到控制台以及磁盘
        /// </summary>
        public Func<LogLevel, string, bool> OnHandler;

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
                    var result = this.OnHandler?.Invoke(logLevel, logString);
                    if (result == true)
                        return;

                    var str = $"[{logLevel.ToString()}]{DateTime.Now:yyyy-MM-dd HH:mm:ss} {logString}\n";

                    // 判断是否要输出到控制台
                    if ((logLevel & this.ConsoleLevel) != 0)
                    {
                        if (logLevel == LogLevel.DBG)
                        {
                            ConsoleEx.Write(str, ConsoleColor.Cyan);
                        }
                        else if (logLevel == LogLevel.SUC)
                        {
                            ConsoleEx.Write(str, ConsoleColor.Green);
                        }
                        else if (logLevel == LogLevel.INF)
                        {
                            ConsoleEx.Write(str, ConsoleColor.Blue);
                        }
                        else if (logLevel == LogLevel.WAR)
                        {
                            ConsoleEx.Write(str, ConsoleColor.Yellow);
                        }
                        else if (logLevel == LogLevel.ERR)
                        {
                            ConsoleEx.Write(str, ConsoleColor.Red);
                        }
                        else if (logLevel == LogLevel.FAL)
                        {
                            ConsoleEx.Write(str, ConsoleColor.DarkMagenta);
                        }
                        else
                        {
                            Console.Write(str);
                        }
                    }


                    // 判断是否要输出到磁盘
                    if ((logLevel & this.FileLevel) != 0)
                    {
                        var logPath = PathEx.Combine(RootPath, $"{DateTime.Now:yyyy-MM-dd}{LogFileName}.log");
                        FileEx.AppendAllText(logPath, str);
                    }
                }
            }
        }
    }
}