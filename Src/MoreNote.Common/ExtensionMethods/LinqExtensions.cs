using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Common.ExtensionMethods
{
    public static class LinqExtensions
    {
        /// <summary>
        /// 判断集合是否有效
        /// 如果装数据的容器是无效的 true
        /// 如果容器是有效的，但是容器里面什么也没有 true
        /// </summary>
        /// <typeparam name="TSource">实现IEnumerable的集合</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrNothing<TSource>(this IEnumerable<TSource> source)
        {
            if (source==null)
            {
                return true;
                
            }
            return !source.Any();
        }
        /// <summary>
        /// string is not null or empty
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValid(this string value) 
        {
                return  !string.IsNullOrEmpty(value);
        }
        public static bool IsValidTrue(this bool? value)
        {
            if (value==null)
            {
                return false;
            }
            return value.Value;
        }

        /// <summary>
        /// 深克隆
        /// </summary>
        /// <param name="obj">原始版本对象</param>
        /// <returns>深克隆后的对象</returns>
        //public static object DepthClone(this object obj)
        //{
        //    object clone = new object();
        //    using (Stream stream = new MemoryStream())
        //    {
        //        IFormatter formatter = new BinaryFormatter();
        //        try
        //        {
        //            formatter.Serialize(stream, obj);
        //            stream.Seek(0, SeekOrigin.Begin);
        //            clone = formatter.Deserialize(stream);
        //        }
        //        catch (SerializationException e)
        //        {
        //            Console.WriteLine("Failed to serialize. Reason: " + e.Message);
        //            throw;
        //        }
        //    }
        //    return clone;
        //}
    }
}
