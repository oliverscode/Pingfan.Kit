using Pingfan.Kit;

namespace Test;

public class Pingfan_Kit测试
{
    [Fact]
    public void TestIniFile()
    {
        var ini = new IniFile("123.ini");

        var age = ini.GetValue("玩家", "年龄");
        
        ini.SetValue("玩家", "年龄", 120.ToString());
        
        ini.Save();
    }
}