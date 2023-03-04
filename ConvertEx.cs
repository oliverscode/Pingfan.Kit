using System;
using System.IO;
using System.Reflection;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Pingfan.Kit
{
    public class ConvertEx
    {
        /// <summary> 
        /// 将一个object对象序列化，返回一个byte[]         
        /// </summary> 
        /// <param name="obj">能序列化的对象</param>         
        /// <returns></returns> 
        public static byte[] ToBytes(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Binder = new VersionDeserializer();
                formatter.Serialize(ms, obj);
                return ms.GetBuffer();
            }
        }

        /// <summary> 
        /// 将一个序列化后的byte[]数组还原         
        /// </summary>
        /// <param name="Bytes"></param>         
        /// <returns></returns> 
        public static object ToObject(byte[] Bytes)
        {
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Binder = new VersionDeserializer();
                return formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// 将一个序列化后的byte[]数组还原成指定类型
        /// </summary>
        /// <param name="Bytes"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ToObject<T>(byte[] Bytes)
        {
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Binder = new VersionDeserializer();
                return (T) ChangeType(formatter.Deserialize(ms), typeof(T));
            }
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
                return Activator.CreateInstance(type, new object[] {innerValue});
            }

            if (value is string && type == typeof(Guid)) return new Guid(value as string);
            if (value is string && type == typeof(Version)) return new Version(value as string);
            if (!(value is IConvertible)) return value;
            return System.Convert.ChangeType(value, type);
        }
    }

    internal sealed class VersionDeserializer : SerializationBinder
    {
        private static string _thisAssembly = null;

        public override Type BindToType(string assemblyName, string typeName)
        {
            Type deserializeType = null;
            _thisAssembly = Assembly.GetEntryAssembly()?.FullName;
            deserializeType = Type.GetType($"{typeName}, {_thisAssembly}");

            return deserializeType;
        }
    }
}