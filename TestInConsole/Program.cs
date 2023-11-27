// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Pingfan.Kit;
using Timer = Pingfan.Kit.Timer;


Timer.SetTime(-1, 16, -1, () =>
{
    Console.WriteLine("当前时间是: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
});
Console.ReadLine();