using Pingfan.Kit;

namespace Test;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Log.Default.Warning("13");
        
        Thread.Sleep(100*1000);
    }
}