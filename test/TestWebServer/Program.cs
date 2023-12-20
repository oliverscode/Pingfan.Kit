using ConsoleTest;
using Pingfan.Kit;

namespace TestWebServer;

class Program
{
    static void Main(string[] args)
    {
        App.Init();

        var files = DirectoryEx.GetFiles(@"G:\甄嬛传");

        foreach (var file in files)
        {
            //[www.domp4.cc]后宫·甄嬛传.E01.HD1080p
            var level = file.Between("甄嬛传.E", ".HD1080p").ToInt();
            if (level > 0)
            {
                File.Move(file, $"G:\\甄嬛传\\第{level}集.mp4");
            }
            // Console.WriteLine($"文件名:{file} 第{level}集");

           
        }

        // App.Run<Startup>();
        App.Run();
    }
}