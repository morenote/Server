using Microsoft.AspNetCore.Http;

namespace Morenote.Framework.Http
{
	public static class MySessionExtensions
	{
		/// <summary>
		/// 获取bool
		/// </summary>
		/// <param name="session"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool? GetBool(this ISession session, string key)
		{
			var value = session.GetInt32(key);
			if (value == null)
			{
				return null;
			}
			if (value.Value == 1)
			{
				return true;


			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 在session中设置bool
		/// </summary>
		/// <param name="session"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void SetBool(this ISession session, string key, bool value)
		{
			session.SetInt32(key, value ? 1 : 0);
		}
	}
}
