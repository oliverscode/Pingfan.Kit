// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Pingfan.Kit;

Stopwatch sw = new Stopwatch();
sw.Start();

var ip = IpEx.GetWanIpLocation("94.103.4.121");

 ip = IpEx.GetWanIpLocation("61.144.142.165");

sw.Start();

Console.WriteLine(ip + " " + sw.ElapsedMilliseconds);