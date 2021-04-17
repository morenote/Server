using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Common.ExtensionMethods
{
    public static class LinqExtensions
    {
        /// <summary>
        /// 判断集合是否有效
        /// 如果装数据的盒子是无效的 true
        /// 如果盒子是有效的，但是盒子里面什么也没有 true
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

    }
}
