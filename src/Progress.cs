using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Pingfan.Kit
{
    /// <summary>
    /// 实时进度
    /// </summary>
    public class Progress
    {
        private long _current;
        private DateTime _lastSpeedTime = DateTime.Now;
        private long _lastSpeedCurrent = 0;
        private double _lastSpeed = 0;
        private List<double> _speedBuffer = new List<double>(10);

        /// <summary>
        /// 任务总量
        /// </summary>
        public long Total { get; set; }


        /// <summary>
        /// 当前进度
        /// </summary>
        public long Current
        {
            get => _current;
            set => _current = value;
        }


        /// <summary>
        /// 当前进度百分比, 2位小数
        /// </summary>
        public float Percent => (float)Math.Floor(Current * 10000d / Total) / 100;


        /// <summary>
        /// 是否已经完成
        /// </summary>
        public bool IsComplete => _current >= Total;

        /// <summary>
        /// 增加进度
        /// </summary>
        /// <param name="count"></param>
        public void Next(long count = 1)
        {
            Interlocked.Add(ref this._current, count);
        }



        /// <summary>
        /// 当前每秒处理的数据量
        /// </summary>
        public double Speed
        {
            get
            {
                var now = DateTime.Now;
                var timeSpan = now - _lastSpeedTime;
                // 100毫秒内不计算
                if (timeSpan.TotalMilliseconds < 100)
                {
                    return _lastSpeed;
                }

                // 2次调用期间增加的进度
                var index = _current - _lastSpeedCurrent;
                // 1位小数
                _lastSpeed = Math.Round(index / timeSpan.TotalSeconds, 1);
                if (index != 0)
                {
                    if (_speedBuffer.Count >= _speedBuffer.Capacity)
                        _speedBuffer.RemoveAt(0);
                    _speedBuffer.Add(_lastSpeed);
                }

                // 最近平均值
                _lastSpeed = Math.Round(_speedBuffer.Average(), 1);

                _lastSpeedTime = now;
                _lastSpeedCurrent = _current;
                return _lastSpeed;
            }
        }

        /// <summary>
        /// 预计剩余时间
        /// </summary>
        public TimeSpan LeftTime
        {
            get
            {
                if (IsComplete)
                    return TimeSpan.Zero;

                if (Speed <= 0)
                    return TimeSpan.MaxValue;

                var left = (Total - _current) / Speed;
                return TimeSpan.FromSeconds(left);
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var leftTime = "";
            if (LeftTime == TimeSpan.MaxValue)
                leftTime = "00:00";
            else if (LeftTime.TotalHours >= 1)
                leftTime = string.Format("{0:D2}:{1:D2}:{2:D2}", LeftTime.TotalHours, LeftTime.Minutes,
                    LeftTime.Seconds);
            else
                leftTime = string.Format("{0:D2}:{1:D2}", LeftTime.Minutes, LeftTime.Seconds);


            return $"{_current}/{Total} [{(int)Percent}%] left {leftTime} ({Speed:F1}/s)";
        }
    }
}