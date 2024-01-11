using System;
using System.Runtime.CompilerServices;
using System.Threading;


class Program
{
    static void Main()
    {

        var url = "http://www.baidu.com";
        var postData = "a=b";
        // 使用HttpClient提交post
        var httpClient = new HttpClient();
        var response = httpClient.PostAsync(url, new StringContent(postData)).Result;
        
    }
}