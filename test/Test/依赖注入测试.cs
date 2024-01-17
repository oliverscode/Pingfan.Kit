using Pingfan.Kit;
using Pingfan.Kit.Cache;
using Pingfan.Kit.Inject;
using Xunit.Abstractions;

namespace Test;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }


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

    class Bear<T> where T : IAnimal
    {
        [Inject] public T Animal { get; set; }
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


    class AA : IAnimal, IContainerReady
    {
        [Inject] public IContainer Container { get; set; }


        public string Name { get; set; }


        public IAnimal BB { get; set; }

        public AA(string name)
        {
            this.Name = name;
        }

        public void OnContainerReady()
        {
            // var subContainer = this.Container.CreateContainer();
            //
            // subContainer.Push("BB");
            // subContainer.Push<BB>();
            //
            //
            // this.BB = subContainer.Get<BB>();
        }
    }

    class BB : IAnimal
    {
        [Inject] public string Name { get; set; }
    }


    [Fact] // 注入接口和实例, 要实例的情况
    public void 注入1个接口和实例_要实例()
    {
        var container = new Container();

        container.Register<IAnimal, Person>();
        var result = container.Get<Person>();

        Assert.Equal("Person", result.Name);
    }

    [Fact] // 注入接口和实例, 要接口的情况
    public void 注入1个接口和实例_要接口()
    {
        var container = new Container();

        container.Register<IAnimal, Person>();
        var result = container.Get<IAnimal>();

        Assert.Equal("Person", result.Name);
    }

    [Fact] // 注入2个接口和实例, 要接口的情况
    public void 注入2个接口和实例_要接口()
    {
        var container = new Container();
        container.Register<IAnimal, Dog>();
        container.Register<IAnimal, Person>();
        var result = container.Get<IAnimal>();

        Assert.Equal("Person", result.Name);
    }

    [Fact] // 注入2个接口和实例, 要名字的情况
    public void 注入2个接口和实例_要名字()
    {
        var container = new Container();
        container.Register<IAnimal, Dog>();
        container.Register<IAnimal, Person>("p");
        var result = container.Get<IAnimal>("p");

        Assert.Equal("Person", result.Name);
    }

    [Fact] // 注入2个接口和实例, 要实例的情况
    public void 注入2个接口和实例_要实例()
    {
        var container = new Container();
        container.Register<IAnimal, Dog>();
        container.Register<IAnimal, Person>();
        var result = container.Get<Person>();

        Assert.Equal("Person", result.Name);
    }

    [Fact] // 循环依赖
    public void 循环依赖()
    {
        var container = new Container();
        container.Register<A>();
        container.Register<B>();
        container.Register<C>();
        var ex = Assert.Throws<Exception>(() => container.Get<A>()); // 假设你的方法是同步的


        Assert.Equal($"递归深度超过{container.MaxDeep}层, 可能存在循环依赖", ex.Message); // 检查异常消息是否为 "依赖循环"
    }

    [Fact] // 属性注入
    public void 属性注入()
    {
        var container = new Container();
        container.Register<Person>();
        container.Register<Taxi>();
        var result = container.Get<Taxi>();

        Assert.Equal("Person", result.Person.Name);
    }

    [Fact] // 在父级中寻找
    public void 在父级中寻找()
    {
        var container = new Container();
        container.Register<Person>();

        var child = container.CreateContainer();


        var result = child.Get<Person>();

        Assert.Equal("Person", result.Name);
    }

    [Fact] // 在祖级中寻找
    public void 在祖级中寻找()
    {
        var container = new Container();
        container.Register<Person>();

        var child = container.CreateContainer();
        var child2 = child.CreateContainer();
        var result = child2.Get<Person>();
        Assert.Equal("Person", result.Name);
    }

    [Fact] // 销毁子容器
    public void 销毁子容器()
    {
        var container = new Container();


        var child = container.CreateContainer();
        child.Register<Person>();
        var result = child.Get<Person>();
        Assert.Equal("Person", result.Name);

        // 销毁
        child.Dispose();

        var ex = Assert.Throws<InjectNotRegisteredException>(() => child.Get<Person>());
        // $"{pop.Name}未被注册, 类型:{pop.Type}"   Person未被注册, 类型:Test.依赖注入测试+Person
        Assert.Equal(typeof(Person), ex.Pop.Type);
    }

    [Fact] // 注入字符串
    public void 注入字符串()
    {
        var container = new Container();
        container.Register("123");
        var result = container.Get<string>();
        Assert.Equal("123", result);
    }

    [Fact] // 注入多个字符串
    public void 注入多个字符串()
    {
        var container = new Container();
        container.Register("123", "f1");
        container.Register("456", "f2");
        var result = container.Get<string>("f2");
        Assert.Equal("456", result);
    }

    [Fact] // 构造注入多个字符串,根据特性指定名字
    public void 构造注入多个字符串_根据特性指定名字()
    {
        var container = new Container();
        container.Register("123", "f1");
        container.Register("456", "f2");
        container.Register<Cat>();

        var result = container.Get<Cat>();
        Assert.Equal("123 456", result.Name);
    }

    [Fact] // 属性注入多个字符串,根据特性指定名字
    public void 属性注入多个字符串_根据特性指定名字()
    {
        var container = new Container();
        container.Register("123", "f1");
        container.Register("456", "f2");
        container.Register<Pig>();

        var result = container.Get<Pig>();
        Assert.Equal("123 456", result.Name);
    }

    [Fact] // 属性注入多个整数,根据特性指定名字
    public void 属性注入多个整数_根据特性指定名字()
    {
        var container = new Container();
        container.Register(123, "f1");
        container.Register(456, "f2");
        container.Register<Fish>();

        var result = container.Get<Fish>();
        Assert.Equal(579, result.Cost);
    }

    [Fact] // 属性注入泛型
    public void 属性注入泛型()
    {
        var container = new Container();
        container.Register<IAnimal, Dog>();
        container.Register<Bear<IAnimal>>();

        var result = container.Get<Bear<IAnimal>>();
        Assert.Equal("Dog", result.Animal.Name);
    }

    [Fact] // 生命周期
    public void 生命周期()
    {
        IContainer container = new Container();
        container.Register<AA>();
        container.Register("AA");
        Assert.True(container.Get<AA>().Name == "AA");

        var subContainer = container.CreateContainer();
        subContainer.Register<BB>();
        Assert.True(subContainer.Get<BB>().Name == "AA");

        subContainer.Delete<BB>();

        container.Register("BB");
        Assert.True(subContainer.Get<BB>().Name == "BB");


        subContainer.Dispose();
        Assert.True(container.Get<AA>().Name == "AA");
    }

    [Fact] // 实力或者接口是否存在
    public void 实力或者接口是否存在()
    {
        IContainer container = new Container();
        container.Register<AA>();
        container.Register("AA");


        Assert.True(container.Has<AA>());
        Assert.False(container.Has<IAnimal>());
        Assert.False(container.Has<BB>());
    }


    [Fact] // 默认值测试
    public void 默认值测试()
    {
        IContainer container = new Container();

        // container.Push("AA");
        var result = container.Get<string>(null, "不是");
        Assert.Equal("不是", result);
    }

    [Fact] // 属性注入测试
    public void 属性注入测试()
    {
        IContainer container = new Container();


        container.Register<DD>();
        container.Register<string>("平凡", null);


        var result = container.New<DD>();
        Assert.Equal("平凡", result.Name);


        result = new DD();
        container.InjectProperties(result);
        Assert.Equal("平凡", result.Name);


        var cache = new CacheMemory();
       
    }

    class DD
    {
        [Inject] public string Name { get; set; }

        public DD()
        {
        }
    }
}