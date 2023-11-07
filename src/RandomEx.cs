using System;
using System.Collections.Generic;
using System.Text;

namespace Pingfan.Kit
{
    /// <summary>
    /// 随机数扩展
    /// </summary>
    public class RandomEx
    {
        private double _probability;
        private double _step;
      
        /// <summary>
        /// 概率随机数
        /// </summary>
        /// <param name="probability">最终概率</param>
        /// <param name="step">有偏差时,调整大小</param>
        public RandomEx(double probability, double step = 0.05)
        {
            this._probability = probability;
            this._step = step;
        }

        /// <summary>
        /// 生成一个随机数
        /// </summary>
        public bool Next()
        {
            var isSuccessful = Rd.NextDouble() < _probability;
            if (isSuccessful)
            {
                _probability -= _step;
            }
            else
            {
                _probability += _step;
            }
            return isSuccessful;
        }

        #region 静态部分
        private static readonly Random Rd = new Random();

        /// <summary>
        /// 生成一个随机数
        /// </summary>
        /// <param name="min">返回值会大于等于此值</param>
        /// <param name="max">返回值会小于此值</param>
        /// <returns></returns>
        public static int Next(int min, int max)
        {
            return Rd.Next(min, max);
        }

        /// <summary>
        /// 生成一个密码, 会排除容易混淆的字符
        /// </summary>
        /// <param name="length"></param>
        /// <param name="isNumber">是否包含数字, 默认包含</param>
        /// <param name="isLower">是否包含小写字母, 默认包含</param>
        /// <param name="isUpper">是否包含大写字母, 默认包含</param>
        /// <param name="isSpecial">是否包含特殊字符, 默认不包含</param>
        /// <returns></returns>
        public static string GeneratePassword(int length, bool isNumber = true, bool isLower = true,
            bool isUpper = true, bool isSpecial = false)
        {
            var sb = new StringBuilder();
            if (isNumber)
            {
                sb.Append("23456789");
            }
            if (isLower)
            {
                sb.Append("abdefghjnpqrty");
            }
            if (isUpper)
            {
                sb.Append("ABDEFGHJLNPQRTY");
            }
            if (isSpecial)
            {
                sb.Append("!@#$%^&*_+-=.");
            }

            var chars = sb.ToString();
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[Rd.Next(0, chars.Length)];
            }
            return new string(result);
        }

        /// <summary>
        /// 正态分布随机数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="sigma">方差，值越小 峰值越尖</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int Next(int min, int max, int sigma)
        {
            int miu = min + (max - min) / 2;
            if (miu <= 0 || miu >= max)
                throw new ArgumentException("Invalid 'min' or 'max' value for the normal distribution.");
            if (sigma >= miu)
                throw new ArgumentException("Sigma value should be less than the mean value (miu).");
            if (sigma <= 0)
                throw new ArgumentException("Sigma value should be greater than 0.");

            double u1, u2, z, x;
            int result;

            while (true)
            {
                u1 = Rd.NextDouble();
                u2 = Rd.NextDouble();

                z = Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);
                x = miu + sigma * z;
                result = (int)x;
                if (result >= min && result < max)
                    return result;
            }
        }

        /// <summary>
        /// 判断是否满足一个概率
        /// </summary>
        /// <param name="probability">比如0.2 返回True的概率就是20%</param>
        /// <returns></returns>
        public static bool IsTrue(double probability)
        {
            return Rd.NextDouble() < probability;
        }

        /// <summary>
        /// 判断是否满足一个概率
        /// </summary>
        /// <param name="probability">比如0.2 返回True的概率就是20%</param>
        /// <returns></returns>
        public static bool IsTrue(float probability)
        {
            return Rd.NextDouble() < probability;
        }

        /// <summary>
        /// 判断是否满足一个概率
        /// </summary>
        /// <param name="probability">比如0.2 返回True的概率就是20%</param>
        /// <returns></returns>
        public static bool IsTrue(decimal probability)
        {
            return (decimal)Rd.NextDouble() < probability;
        }

        /// <summary>
        /// 从一个集合中随机取出一个元素
        /// </summary>
        /// <param name="objs"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Next<T>(params T[] objs)
        {
            var index = RandomEx.Next(0, objs.Length);
            return objs[index];
        }
        #endregion
    }

    /// <summary>
    /// 根据权重生成随机对象类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RandomEx<T>
    {
        private readonly List<KeyValuePair<T, int>> _list = new List<KeyValuePair<T, int>>();
        private int _maxWeights;

        public RandomEx()
        {
        }
        public RandomEx(IDictionary<T, int> items)
        {
            foreach (var item in items)
            {
                Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// 添加随机参数
        /// </summary>
        /// <param name="obj">随机的对象</param>
        /// <param name="weights">相对权重</param>
        /// <exception cref="ArgumentException"></exception>
        public void Add(T obj, int weights)
        {
            if (weights < 0)
                throw new ArgumentException(nameof(weights));

            _list.Add(new KeyValuePair<T, int>(obj, weights));
            _maxWeights += weights;
        }

        /// <summary>
        /// 根据权重生成一个随机对象
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T Next()
        {
            if (_list.Count <= 0)
                throw new Exception("获取不到随机对象");

            var index = RandomEx.Next(0, _maxWeights);
            var currentMax = _list[0].Value;
            for (var i = 0; i < _list.Count; i++)
            {
                var row = _list[i];
                if (index < currentMax)
                {
                    return row.Key;
                }

                currentMax += _list[i + 1].Value;
            }

            throw new Exception("获取不到权重");
        }
    }

}