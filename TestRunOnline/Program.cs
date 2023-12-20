using System.Reflection;
using System.Runtime.CompilerServices;
using Pingfan.Kit;

class Program
{
    static void Main(string[] args)
    {
        // App.Online("http://10.0.0.198:5500/System.Data.SqlClient.dll");
        // App.Online("http://10.0.0.198:5500/HallWebApi.Web.dll");

        var a = new AssemblyLoader("http://10.0.0.198:5500/");
      a.LoadAssembly("http://10.0.0.198:5500/Pingfan.FreeSql.SqlServer.dll");
      
      a.LoadAssembly("http://10.0.0.198:5500/Microsoft.Data.SqlClient.dll");
       
        
      
        a.LoadAssembly("http://10.0.0.198:5500/HallWebApi.Web.dll");

        {
            Task.Run(async () =>
            {

                await Task.Delay(2000);
                var assemblies = GetAssemblyAndDescendants(Assembly.GetExecutingAssembly().FullName);
                foreach (var assembly in assemblies.OrderBy(p => p.FullName))
                {
                    if (assembly.GetName().FullName.ContainsIgnoreCase("SqlClient"))
                    {
                        
                    }
                    Console.WriteLine(assembly.GetName().Name);
                }

                Console.WriteLine($"一共:{assemblies.Count}");


                var type = Type.GetType("FreeSql.Provider.SqlServerForSystem");
                // 获取type的assembly
                var sss = type!.Assembly;
                var t= sss.GetReferencedAssemblies();
            });


        }
        
        App.Run();
    }
    
    // 给一个assembly名，返回这个assembly以及它的所有引用的assembly以及后代assembly
    static List<Assembly> GetAssemblyAndDescendants(string assemblyName)
    {
        var assembly = Assembly.Load(assemblyName);
        var result = new List<Assembly>();
        result.Add(assembly);
        var queue = new Queue<Assembly>();
        queue.Enqueue(assembly);
        while (queue.Count > 0)
        {
            var a = queue.Dequeue();
            if (a.FullName.ContainsIgnoreCase("Pingfan"))
            {
                
            }
            foreach (var r in a.GetReferencedAssemblies())
            {
                var a2 = Assembly.Load(r);
                if (result.Contains(a2) == false)
                {
                    result.Add(a2);
                    queue.Enqueue(a2);
                }
            }
        }

        return result;
    }
}