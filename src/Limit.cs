using System;
using System.Collections.Generic;
using System.Linq;

namespace Pingfan.Kit;

/// <summary>
/// 限制类
/// </summary>
public static class Limit
{
    static Limit()
    {
        Ticker.LoopWithTry(1000, () =>
        {
            // 倒着循环_limitList
            for (var i = _limitList.Count - 1; i >= 0; i--)
            {
                var item = _limitList.ElementAt(i);
                var list = item.Value;
                // 倒着循环list
                for (var j = list.Count - 1; j >= 0; j--)
                {
                    if (list[j] < DateTime.Now.AddSeconds(-60))
                    {
                        list.RemoveAt(j);
                    }
                }

                if (list.Count == 0)
                {
                    _limitList.TryRemove(item.Key, out _);
                }
            }
        });
    }

    private static ThreadSafeDictionary<string, List<DateTime>> _limitList = new();

    /// <summary>
    /// 如果处于限流状态, 就返回true, 否者返回false
    /// </summary>
    /// <param name="key">标识</param>
    /// <param name="count">最大次数, 包含此次数</param>
    /// <param name="maxMillisecondInterval">毫秒间隔时间内, 最大不能超过60秒</param>
    /// <returns></returns>
    public static bool On(string key, int count, int maxMillisecondInterval)
    {
        if (maxMillisecondInterval > 60000)
        {
            throw new ArgumentException("maxMillisecondInterval不能超过60秒");
        }

        if (_limitList.TryGetValue(key, out var list) == false)
        {
            _limitList.TryAdd(key, new List<DateTime>()
            {
                DateTime.Now
            });

            return false;
        }

        list.Add(DateTime.Now);
        // 判断是否超过了限制
        if (list.Count(x => x > DateTime.Now.AddMilliseconds(-maxMillisecondInterval)) > count)
        {
            return true;
        }

        return false;
    }
}