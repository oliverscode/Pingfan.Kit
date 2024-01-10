using System;
using System.Collections.Generic;

public class TextAnalyzer
{
    public string FindLongestSimilarSubstring(string text, int maxErrors)
    {
        int n = text.Length;
        string longestSimilarPart = "";
        int longestLength = 0;

        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                int length = Math.Min(n - i, n - j);
                int errors = 0;
                int k = 0;
                for (; k < length && errors <= maxErrors; k++)
                {
                    if (text[i + k] != text[j + k])
                    {
                        errors++;
                    }
                }

                // Subtract one to get the length of the substring before the error exceeded maxErrors
                k--;

                if (k > longestLength && errors <= maxErrors)
                {
                    longestLength = k;
                    longestSimilarPart = text.Substring(i, k);
                }
            }
        }

        return longestSimilarPart;
    }
}


public class Program
{
    public static void Main()
    {
        // 使用方法：
        var text = @"电视格式下载地址：  电视 神盾局特工[第一季] - 第22集       迅雷下载  电视 神盾局特工[第一季] - 第21集       迅雷下载  电视 神盾局特工[第一季] - 第20集       迅雷下载  电视 神盾局特工[第一季] - 第19集       迅雷下载  电视 神盾局特工[第一季] - 第18集       迅雷下载  电视 神盾局特工[第一季] - 第17集       迅雷下载  电视 神盾局特工[第一季] - 第16集       迅雷下载  电视 神盾局特工[第一季] - 第15集       迅雷下载  电视 神盾局特工[第一季] - 第14集      局特工[第一季]MP4视频截图
神盾局特工[第一季]-影片截图";
        var textAnalyzer = new TextAnalyzer();
        var similarParts = textAnalyzer.FindLongestSimilarSubstring(text, 2); // 允许2个错误字符
        foreach (var part in similarParts)
        {
            Console.WriteLine(part);
        }

    }
}