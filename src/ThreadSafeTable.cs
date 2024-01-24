namespace Pingfan.Kit;

/// <summary>
/// 表结构
/// </summary>
public class ThreadSafeTable<T> : ThreadSafeList<ThreadSafeList<T>>
{
    /// <inheritdoc />
    public ThreadSafeTable()
    {
    }

    /// <inheritdoc />
    public ThreadSafeTable(int capacity) : base()
    {
    }
}