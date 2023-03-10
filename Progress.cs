using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Pingfan.Kit
{
    public class Progress : IDisposable
    {
        private object locker = new object();

        private DateTime _startTime = DateTime.Now;
        private DateTime _endTime;

        //每秒处理
        private double _perSecondsCount;

        /// <summary>
        /// 已用时间
        /// </summary>
        public TimeSpan Elapsed => ((_endTime.Ticks > 0) ? _endTime : DateTime.Now) - _startTime;


        private long _total;

        /// <summary>
        /// 任务总数
        /// </summary>
        public long Total => _total;

        private long _currentIndex;

        /// <summary>
        /// 当前任务进度
        /// </summary>
        public long Index => _currentIndex;


        /// <summary>
        /// 是否已经执行完成
        /// </summary>
        public bool IsComplete => Index >= _total;

        // 最近的速率
        private List<long> _speedSum = new List<long>();
        private const int maxSpeedSum = 20; // 取最近10条的平均值

        /// <summary>
        /// 预计剩余秒数
        /// </summary>
        public long EstSeconds
        {
            get
            {
                if (!(PerSecondsCount <= 0.0))
                {
                    return (long) Math.Round((double) (_total - Index) / PerSecondsCount);
                }

                return 0L;
            }
        }

        /// <summary>
        /// 预计剩余时间
        /// </summary>

        public TimeSpan EstElapsed => new TimeSpan(EstSeconds * 1000 * 10000);

        /// <summary>
        /// 进度百分比
        /// </summary>
        public float Percent
        {
            get
            {
                if (_total > 0)
                {
                    return (float) Math.Round(100.0 * (double) Index / (double) _total, 2);
                }

                return 0f;
            }
        }

        /// <summary>
        /// 每秒处理量
        /// </summary>
        public double PerSecondsCount
        {
            get
            {
                if (IsComplete)
                {
                    return 0.0;
                }

                lock (locker)
                {
                    var ticks = DateTime.Now.Ticks;
                    var timeCount = ticks - _startTime.Ticks;

                    if (timeCount < 100 * 10000)
                    {
                        return _perSecondsCount;
                    }

                    var avg = 0d;
                    if (_speedSum.Count > 0)
                        avg = _speedSum.Average();

                    // var speed = 10000000.0d * avg / timeCount;
                    // _perSecondsCount = Math.Round(speed, 2);

                    _perSecondsCount = avg;

                    return _perSecondsCount;
                }
            }
        }

        /// <summary>
        /// 增加处理量进度
        /// </summary>
        /// <param name="count"></param>
        public void Next(long count = 1L)
        {
            Interlocked.Add(ref _currentIndex, count);

            // 增加每秒处理量
            lock (locker)
            {
                _speedSum.Add(count);
                if (_speedSum.Count > maxSpeedSum)
                {
                    _speedSum.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// 设置当前进度
        /// </summary>
        /// <param name="index"></param>
        public void SetIndex(long index)
        {
            if (_currentIndex != 0)
            {
                // 增加每秒处理量
                lock (locker)
                {
                    _speedSum.Add(index - _currentIndex);
                    if (_speedSum.Count > maxSpeedSum)
                    {
                        _speedSum.RemoveAt(0);
                    }
                }
            }


            //设置当前位置
            Interlocked.Exchange(ref _currentIndex, index);


            if (index >= _total)
            {
                _endTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 设置总量进度
        /// </summary>
        /// <param name="total"></param>
        public void SetTotal(long total)
        {
            Interlocked.Exchange(ref _total, total);
            if (Index >= _total)
            {
                _endTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 设置当前和总量进度
        /// </summary>
        /// <param name="index"></param>
        /// <param name="total"></param>
        public void Set(long index, long total)
        {
            Interlocked.Exchange(ref _currentIndex, index);
            Interlocked.Exchange(ref _total, total);
            if (_currentIndex >= _total)
            {
                _endTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 清空全部进度
        /// </summary>
        public void Clear()
        {
            _total = 0L;
            _currentIndex = 0L;
            _startTime = DateTime.Now;
            _endTime = DateTime.MinValue;
        }

        public override string ToString()
        {
            string yiGuoTime = ((int) Elapsed.TotalHours).ToString("D2")
                               + ":"
                               + (Elapsed.Minutes).ToString("D2")
                               + ":"
                               + (Elapsed.Seconds).ToString("D2");
            string shengYuTime = ((int) EstElapsed.TotalHours).ToString("D2")
                                 + ":"
                                 + (EstElapsed.Minutes).ToString("D2")
                                 + ":"
                                 + (EstElapsed.Seconds).ToString("D2");


            if (IsComplete)
            {
                return $"{Index}/{_total} [{(int) Percent}%] in {shengYuTime} (0/s, ETA:0s)";
            }

            return $"{Index}/{_total} [{(int) Percent}%] in {shengYuTime} ({PerSecondsCount:F1}/s, ETA:{yiGuoTime})";
        }

        public void Dispose()
        {
            Clear();
        }
    }
}