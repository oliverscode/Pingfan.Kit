// using System;
// using System.Runtime.CompilerServices;
// using System.Threading;
// using Pingfan.Kit;
//
// class Program
// {
//     static void Main()
//     {
//         unsafe
//         {
//           
//         
//             var path = "Q:\\泰版放羊的星星";
//             foreach (var file in DirectoryEx.GetFiles(path))
//             {
//                 //My.Lucky.Star.2023.S01E01.1080p.WEB-DL.H264.AAC-Huawei.mp4
//             
//                 var level = file.Between("S01E", ".1080p").ToInt();  
//                 // var level = file.ToInt();
//
//                 if (level > 0)
//                 {
//                     var newPath = $"{path}\\{level}.mp4";
//                     Console.WriteLine($"移动文件:{file} 到 {newPath}");
//                     File.Move(file, newPath);
//                 }
//             }
//         }
//     }
// }