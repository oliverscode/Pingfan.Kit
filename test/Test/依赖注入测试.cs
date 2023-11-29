using Pingfan.Kit.Inject;
using Pingfan.Kit.Inject.Attributes;
using Xunit.Abstractions;

namespace Test;

public class 依赖注入测试
{
    interface IAnimal
    {
        string Name { get; }
    }

    class Person : IAnimal
    {
        public string Name => "Person";
    }

    class Dog : IAnimal
    {
        public string Name => "Dog";
    }

    class Car
    {
        public string Type => "Benz";
    }


    [Fact] // 注入接口和实例, 要实例的情况
    public void TestPush1()
    {
        var container = new Container(null);

        container.Push<IAnimal, Person>();
        var result = container.Get<Person>();

        Assert.Equal("Person", result.Name);
    }

    [Fact] // 注入接口和实例, 要接口的情况
    public void TestPush2()
    {
        var container = new Container(null);

        container.Push<IAnimal, Person>();
        var result = container.Get<IAnimal>();

        Assert.Equal("Person", result.Name);
    }

    [Fact] // 注入2个接口和实例, 要接口的情况
    public void TestPush3()
    {
        var container = new Container(null);
        container.Push<IAnimal, Dog>();
        container.Push<IAnimal, Person>();
        var result = container.Get<IAnimal>();

        Assert.Equal("Dog", result.Name);
    }

    [Fact] // 注入2个接口和实例, 要名字的情况
    public void TestPush4()
    {
        var container = new Container(null);
        container.Push<IAnimal, Dog>();
        container.Push<IAnimal, Person>("p");
        var result = container.Get<IAnimal>("p");

        Assert.Equal("Person", result.Name);
    }
    
    [Fact] // 注入2个接口和实例, 要实例的情况
    public void TestPush5()
    {
        var container = new Container(null);
        container.Push<IAnimal, Dog>();
        container.Push<IAnimal, Person>("p");
        var result = container.Get<Person>();

        Assert.Equal("Person", result.Name);
    }
}