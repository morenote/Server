namespace MoreNote.Common.Utils
{
	/// <summary>
	/// 字符串过滤工具
	/// </summary>
	public class TextFilterUtil
	{
		/// <summary>
		/// 过滤一些特殊符号
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string DelUnSafeChar(string str)
		{
			string result = str;
			string[] strQuota = { "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "`", ";", "'", ",", "/", ":", "/,", "<", ">", "?", "|" };
			foreach (string item in strQuota)
			{
				if (result.Contains(item))
				{
					result = result.Replace(item, "");
				}
			}
			return result;
		}
	}
}
