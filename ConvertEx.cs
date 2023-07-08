using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Pingfan.Kit
{
    public static class ConvertEx
    {
        /// <summary>
        /// 将一个object对象序列化，返回一个byte[]
        /// </summary>
        /// <param name="obj">能序列化的对象</param>
        /// <returns></returns>
        public static byte[] ToBytes(object obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = GetFormatter();
                formatter.Serialize(ms, obj);
                return ms.GetBuffer();
            }
        }

        /// <summary>
        /// 将一个序列化后的byte[]数组还原
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static object ToObject(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var formatter = GetFormatter();
                return formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// 将一个序列化后的byte[]数组还原成指定类型
        /// </summary>
        /// <param name="bytes"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ToObject<T>(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var formatter = GetFormatter();
                return (T)ChangeType(formatter.Deserialize(ms), typeof(T));
            }
        }

        private static IFormatter GetFormatter()
        {
            return new BinaryFormatter { Binder = new VersionDeserializer() };
        }

        public static object ChangeType(object value, Type type)
        {
            if (value == null && type.IsGenericType) return Activator.CreateInstance(type);
            if (value == null) return null;
            if (type == value.GetType()) return value;
            if (type.IsEnum)
            {
                if (value is string)
                    return Enum.Parse(type, value as string);
                else
                    return Enum.ToObject(type, value);
            }

            if (!type.IsInterface && type.IsGenericType)
            {
                Type innerType = type.GetGenericArguments()[0];
                object innerValue = ChangeType(value, innerType);
                return Activator.CreateInstance(type, new object[] { innerValue });
            }

            if (value is string && type == typeof(Guid)) return new Guid(value as string);
            if (value is string && type == typeof(Version)) return new Version(value as string);
            if (!(value is IConvertible)) return value;
            return Convert.ChangeType(value, type);
        }
    }

    internal sealed class VersionDeserializer : SerializationBinder
    {
        private static string _thisAssembly;

        public override Type BindToType(string assemblyName, string typeName)
        {
            _thisAssembly = Assembly.GetEntryAssembly()?.FullName;
            return Type.GetType($"{typeName}, {_thisAssembly}");
        }
    }
}