using ConsoleTest;
using Pingfan.Kit;

namespace TestWebServer;

class Program
{
    static void Main(string[] args)
    {
        App.Init();
        
        
        App.Run<Startup>();
    }
}