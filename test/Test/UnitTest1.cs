using Pingfan.Kit;
using Xunit.Abstractions;

namespace Test;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Test1()
    {
        Log.Warning("13");

        Thread.Sleep(1 * 1000);
        Assert.True(true);
    }

    [Fact]
    public void TestProgress()
    {
        // var total = 10;
        // var pe = new Progress();
        // pe.Total = total;
        // var task = new Task(async () =>
        // {
        //     for (int i = 0; i < total; i++)
        //     {
        //         await Task.Delay(1000);
        //         pe.Next();
        //     }
        // });
        // task.Start();
        //
        //
        // // show
        // Task.Run(async () =>
        // {
        //     while (true)
        //     {
        //         Console.WriteLine($"处理速度:{pe.Speed}");
        //         await Task.Delay(500);
        //     }
        // });
        //
        // Thread.Sleep(12 * 1000);
    }
}