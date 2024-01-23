using System;
using System.Text;

namespace Pingfan.Kit
{
    /// <summary>
    /// 控制台扩展类
    /// </summary>
    public static class ConsoleEx
    {
        private static readonly object Locker = new object();

        /// <summary>
        /// 输入文本
        /// </summary>
        public static string Input(string text,
            ConsoleColor outColor = ConsoleColor.Cyan,
            ConsoleColor inColor = ConsoleColor.Yellow)
        {
            lock (Locker)
            {
                var foregroundColor = Console.ForegroundColor;

                if (!string.IsNullOrEmpty(text))
                {
                    Console.ForegroundColor = outColor;
                    Console.Write(text);
                }

                Console.ForegroundColor = inColor;
                var result = Console.ReadLine();
                Console.ForegroundColor = foregroundColor;
                return result!;
            }
        }

        /// <summary>
        /// 要求输入密码
        /// </summary>
        public static string InputPassword(string text, ConsoleColor outColor = ConsoleColor.Cyan)
        {
            lock (Locker)
            {
                var foregroundColor = Console.ForegroundColor;
                Console.ForegroundColor = outColor;
                Console.Write(text);
                var stringBuilder = new StringBuilder();
                while (true)
                {
                    var consoleKeyInfo = Console.ReadKey(true);

                    if (consoleKeyInfo.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                        break;
                    }
                    else if (consoleKeyInfo.Key == ConsoleKey.Backspace)
                    {
                        if (stringBuilder.Length <= 0) continue;
                        stringBuilder.Remove(stringBuilder.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                    else if (char.IsLetterOrDigit(consoleKeyInfo.KeyChar)
                             || char.IsPunctuation(consoleKeyInfo.KeyChar))
                    {
                        stringBuilder.Append(consoleKeyInfo.KeyChar);
                        Console.Write("*"); // 显示星号
                    }
                }

                var result = stringBuilder.ToString();
                Console.ForegroundColor = foregroundColor;
                return result;
            }
        }

        /// <summary>
        /// 当前行输出
        /// </summary>
        public static void Write(string text, ConsoleColor outColor = ConsoleColor.DarkGray)
        {
            lock (Locker)
            {
                var foregroundColor = Console.ForegroundColor;
                Console.ForegroundColor = outColor;
                Console.Write(text);
                Console.ForegroundColor = foregroundColor;
            }
        }

        /// <summary>
        /// 输出一行
        /// </summary>
        public static void WriteLine(string? text = null, ConsoleColor outColor = ConsoleColor.DarkGray)
        {
            lock (Locker)
            {
                var foregroundColor = Console.ForegroundColor;
                Console.ForegroundColor = outColor;
                Console.WriteLine(text);
                Console.ForegroundColor = foregroundColor;
            }
        }
    }
}