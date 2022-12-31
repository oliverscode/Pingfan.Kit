using System;
using System.Text;

namespace Pingfan.Kit
{
    public class ConsoleEx
    {
        private static object Locker = new object();

        public static string ReadString(string write,
            ConsoleColor outColor = ConsoleColor.Cyan,
            ConsoleColor inColor = ConsoleColor.Yellow)
        {
            lock (Locker)
            {
                var foregroundColor = Console.ForegroundColor;
                Console.ForegroundColor = outColor;
                Console.Write(write);
                Console.ForegroundColor = inColor;
                var result = Console.ReadLine();
                Console.ForegroundColor = foregroundColor;
                return result;
            }
        }

        public static string ReadPassword(string write, ConsoleColor outColor = ConsoleColor.Cyan)
        {
            lock (Locker)
            {
                var foregroundColor = Console.ForegroundColor;
                Console.ForegroundColor = outColor;
                Console.Write(write);
                var stringBuilder = new StringBuilder();
                while (true)
                {
                    var consoleKeyInfo = Console.ReadKey(intercept: true);

                    if (consoleKeyInfo.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                        break;
                    }

                    //判断不是功能键
                    var charString = consoleKeyInfo.KeyChar.ToString();
                    if (string.IsNullOrEmpty(charString) == false)
                        stringBuilder.Append(consoleKeyInfo.KeyChar);
                }


                var result = stringBuilder.ToString();
                Console.ForegroundColor = foregroundColor;
                return result;
            }
        }

        public static T Read<T>(string write,
            ConsoleColor outColor = ConsoleColor.Cyan,
            ConsoleColor inColor = ConsoleColor.Yellow)
        {
            while (true)
            {
                try
                {
                    return (T) Pingfan.Kit.Convert.ConvertEx.ChangeType(ReadString(write, outColor, inColor), typeof(T));
                }
                catch
                {
                    // ignored
                }
            }
        }

        public static void Write(string write, ConsoleColor outColor = ConsoleColor.Green)
        {
            lock (Locker)
            {
                var foregroundColor = Console.ForegroundColor;
                Console.ForegroundColor = outColor;
                Console.Write(write);
                Console.ForegroundColor = foregroundColor;
            }

        }

        public static void WriteLine(string write = null, ConsoleColor outColor = ConsoleColor.Green)
        {
            lock (Locker)
            {
                var foregroundColor = Console.ForegroundColor;
                Console.ForegroundColor = outColor;
                Console.WriteLine(write);
                Console.ForegroundColor = foregroundColor;
            }

        }
    }
}