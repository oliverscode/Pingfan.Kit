// See https://aka.ms/new-console-template for more information

using Pingfan.Kit;

var total = 200;


var pe = new Progress();
pe.Total = total;


var p = new ParallelProcessor<int>(async (i) =>
{
    pe.Next();
    await Task.Delay(100);
}, 2);
for (int i = 0; i < total; i++)
{
    p.Add(RandomEx.Next(1, 9));
}

// Task.Run(async () =>
// {
//     while (true)
//     {
//         for (int i = 0; i < total; i++)
//         {
//             p.Add(RandomEx.Next(1, 9));
//         }
//
//         await Task.Delay(2000);
//         pe.Total += total;
//     }
// });

// show
Task.Run(async () =>
{
    while (pe.IsComplete == false)
    {
        // Console.WriteLine($"处理速度:{pe.Speed}, 预计剩余时间:{Math.Round(pe.LeftTime.TotalSeconds, 1)}, 进度:{pe.Percent}%");
        Console.WriteLine(pe);
        await Task.Delay(500);
        
    }
});
Console.ReadLine();