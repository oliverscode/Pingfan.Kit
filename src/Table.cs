using System.Collections;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable CollectionNeverQueried.Local

namespace Pingfan.Kit;

/// <summary>
/// 表结构
/// </summary>
public class Table<T> : List<List<T>>
{
    /// <inheritdoc />
    public Table()
    {
    }

    /// <inheritdoc />
    public Table(int capacity) : base(capacity)
    {
    }
}