using Microsoft.Extensions.Caching.Distributed;

using System;
using System.Text;

namespace MoreNote.Common.ExtensionMethods
{
	public static class DistributedCacheOptionExtensions
	{
		/// <summary>
		/// 设置字符串
		/// </summary>
		/// <param name="distributedCache"></param>
		/// <param name="key">键</param>
		/// <param name="value">值</param>
		/// <param name="secondTimeout">超时时间 秒</param>
		public static void SetString(this IDistributedCache distributedCache, string key, string value, int secondTimeout)
		{
			DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
			//设置绝对过期时间 两种写法
			//options.AbsoluteExpiration = DateTime.Now.AddMinutes(30);
			//options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(30));
			//设置滑动过期时间 两种写法
			//options.SlidingExpiration = TimeSpan.FromSeconds(30);
			options.SetSlidingExpiration(TimeSpan.FromSeconds(secondTimeout));
			distributedCache.SetString(key, value, options);
		}

		public static void SetInt(this IDistributedCache distributedCache, string key, int value)
		{
			distributedCache.SetString(key, value.ToString());

		}



		public static int? GetInt(this IDistributedCache distributedCache, string key)
		{
			var value = distributedCache.GetString(key);
			if (value == null)
			{
				return null;
			}
			var number = Int32.Parse(value);
			return number;

		}

		public static void SetBool(this IDistributedCache distributedCache, string key, bool value)
		{
			distributedCache.SetInt(key, value ? 1 : 0);
		}


		public static bool? GetBool(this IDistributedCache distributedCache, string key)
		{
			var number = distributedCache.GetInt(key);
			if (number == null)
			{
				return null;
			}
			return number.Value == 1;
		}

		public static bool GetBool(this IDistributedCache distributedCache, string key, bool defaultValue)
		{
			var number = distributedCache.GetInt(key);
			if (number == null)
			{
				return defaultValue;
			}
			return number.Value == 1;
		}
	}
}
