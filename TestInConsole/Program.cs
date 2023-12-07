// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Pingfan.Kit;
using Timer = Pingfan.Kit.Timer;

App.Init();

Timer.SetInterval(2000, () => { Log.Info("正在执行定时任务"); });

App.Run();