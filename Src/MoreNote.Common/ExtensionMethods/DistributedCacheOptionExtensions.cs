using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Distributed;

namespace MoreNote.Common.ExtensionMethods
{
    public static class DistributedCacheOptionExtensions
    {
        /// <summary>
        /// 设置字符串
        /// </summary>
        /// <param name="distributedCache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="secondTimeout"></param>
        public static void SetString(this IDistributedCache distributedCache,string key,string value,int secondTimeout)
        {
             DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            //设置绝对过期时间 两种写法
            //options.AbsoluteExpiration = DateTime.Now.AddMinutes(30);
            //options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(30));
            //设置滑动过期时间 两种写法
            //options.SlidingExpiration = TimeSpan.FromSeconds(30);
            options.SetSlidingExpiration(TimeSpan.FromSeconds(secondTimeout));
            distributedCache.SetString(key, value,options);
        }
    }
}
