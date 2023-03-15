﻿using System;
using System.Text;
using System.Xml;

namespace Pingfan.Kit
{
    public static class ConsoleEx
    {
        private static readonly object _locker = new object();

        public static string Input(string text,
            ConsoleColor outColor = ConsoleColor.Cyan,
            ConsoleColor inColor = ConsoleColor.Yellow)
        {
            lock (_locker)
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
                return result;
            }
        }

        public static string InputPassword(string text, ConsoleColor outColor = ConsoleColor.Cyan)
        {
            lock (_locker)
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

        public static void Write(string text, ConsoleColor outColor = ConsoleColor.DarkGray)
        {
            lock (_locker)
            {
                var foregroundColor = Console.ForegroundColor;
                Console.ForegroundColor = outColor;
                Console.Write(text);
                Console.ForegroundColor = foregroundColor;
            }
        }

        public static void WriteLine(string text, ConsoleColor outColor = ConsoleColor.DarkGray)
        {
            lock (_locker)
            {
                var foregroundColor = Console.ForegroundColor;
                Console.ForegroundColor = outColor;
                Console.WriteLine(text);
                Console.ForegroundColor = foregroundColor;
            }
        }
    }
}