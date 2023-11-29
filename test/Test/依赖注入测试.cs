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

    class Cat
    {
        public string Name { get; set; }

        public Cat([Inject("f1")] string firstname, [Inject("f2")] string lastname)
        {
            Name = $"{firstname} {lastname}";
        }
    }

    class Pig
    {
        public string Name => $"{Firstname} {Lastname}";

        [Inject("f1")] public string Firstname { get; set; } = null!;

        [Inject("f2")] public string Lastname { get; set; } = null!;
    }

    class Fish
    {
        public int Cost => Length + Size;

        [Inject("f1")] public int Length { get; set; }

        [Inject("f2")] public int Size { get; set; }
    }


    class Car
    {
        public string Type => "Benz";
        public Person Person { get; set; }

        public Car(Person person)
        {
            this.Person = person;
        }
    }

    class Taxi
    {
        [Inject] public Person Person { get; set; }
    }

    class A
    {
        public A(B b)
        {
        }
    }

    class B
    {
        public B(C c)
        {
        }
    }

    class C
    {
        public C(A a)
        {
        }
    }

    [Fact] // 注入接口和实例, 要实例的情况
    public void 注入1个接口和实例_要实例()
    {
        var container = new Container();

        container.Push<IAnimal, Person>();
        var result = container.Get<Person>();

        Assert.Equal("Person", result.Name);
    }

    [Fact] // 注入接口和实例, 要接口的情况
    public void 注入1个接口和实例_要接口()
    {
        var container = new Container();

        container.Push<IAnimal, Person>();
        var result = container.Get<IAnimal>();

        Assert.Equal("Person", result.Name);
    }

    [Fact] // 注入2个接口和实例, 要接口的情况
    public void 注入2个接口和实例_要接口()
    {
        var container = new Container();
        container.Push<IAnimal, Dog>();
        container.Push<IAnimal, Person>();
        var result = container.Get<IAnimal>();

        Assert.Equal("Dog", result.Name);
    }

    [Fact] // 注入2个接口和实例, 要名字的情况
    public void 注入2个接口和实例_要名字()
    {
        var container = new Container();
        container.Push<IAnimal, Dog>();
        container.Push<IAnimal, Person>("p");
        var result = container.Get<IAnimal>("p");

        Assert.Equal("Person", result.Name);
    }

    [Fact] // 注入2个接口和实例, 要实例的情况
    public void 注入2个接口和实例_要实例()
    {
        var container = new Container();
        container.Push<IAnimal, Dog>();
        container.Push<IAnimal, Person>("p");
        var result = container.Get<Person>();

        Assert.Equal("Person", result.Name);
    }

    [Fact] // 循环依赖
    public void 循环依赖()
    {
        var container = new Container();
        container.Push<A>();
        container.Push<B>();
        container.Push<C>();
        var ex = Assert.Throws<Exception>(() => container.Get<A>()); // 假设你的方法是同步的


        Assert.Equal($"递归深度超过{container.MaxDeep}层, 可能存在循环依赖", ex.Message); // 检查异常消息是否为 "依赖循环"
    }

    [Fact] // 属性注入
    public void 属性注入()
    {
        var container = new Container();
        container.Push<Person>();
        container.Push<Taxi>();
        var result = container.Get<Taxi>();

        Assert.Equal("Person", result.Person.Name);
    }

    [Fact] // 在父级中寻找
    public void 在父级中寻找()
    {
        var container = new Container();
        container.Push<Person>();

        var child = container.CreateChild();


        var result = child.Get<Person>();

        Assert.Equal("Person", result.Name);
    }

    [Fact] // 在祖级中寻找
    public void 在祖级中寻找()
    {
        var container = new Container();
        container.Push<Person>();

        var child = container.CreateChild();
        var child2 = child.CreateChild();
        var result = child2.Get<Person>();
        Assert.Equal("Person", result.Name);
    }

    [Fact] // 销毁子容器
    public void 销毁子容器()
    {
        var container = new Container();


        var child = container.CreateChild();
        child.Push<Person>();
        var result = child.Get<Person>();
        Assert.Equal("Person", result.Name);

        // 销毁
        child.Dispose();

        var ex = Assert.Throws<Exception>(() => child.Get<Person>());
        Assert.Equal("无法创建实例", ex.Message);
    }

    [Fact] // 注入字符串
    public void 注入字符串()
    {
        var container = new Container();
        container.Push("123");
        var result = container.Get<string>();
        Assert.Equal("123", result);
    }

    [Fact] // 注入多个字符串
    public void 注入多个字符串()
    {
        var container = new Container();
        container.Push("f1", "123");
        container.Push("f2", "456");
        var result = container.Get<string>("f2");
        Assert.Equal("456", result);
    }

    [Fact] // 构造注入多个字符串,根据特性指定名字
    public void 构造注入多个字符串_根据特性指定名字()
    {
        var container = new Container();
        container.Push("f1", "123");
        container.Push("f2", "456");
        container.Push<Cat>();

        var result = container.Get<Cat>();
        Assert.Equal("123 456", result.Name);
    }

    [Fact] // 属性注入多个字符串,根据特性指定名字
    public void 属性注入多个字符串_根据特性指定名字()
    {
        var container = new Container();
        container.Push("f1", "123");
        container.Push("f2", "456");
        container.Push<Pig>();

        var result = container.Get<Pig>();
        Assert.Equal("123 456", result.Name);
    }
    
    [Fact] // 属性注入多个整数,根据特性指定名字
    public void 属性注入多个整数_根据特性指定名字()
    {
        var container = new Container();
        container.Push("f1", 123);
        container.Push("f2", 456);
        container.Push<Fish>();
        
        var result = container.Get<Fish>();
        Assert.Equal(579, result.Cost);
    }
}