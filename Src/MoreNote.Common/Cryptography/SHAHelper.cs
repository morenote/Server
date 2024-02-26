using System;
using System.Security.Cryptography;
using System.Text;

namespace MoreNote.Common.Cryptography
{
	public class SHAHelper
	{
		public string MD5ncryptStr(string str)
		{
			MD5 m = new MD5CryptoServiceProvider();
			byte[] s = m.ComputeHash(UnicodeEncoding.UTF8.GetBytes(str));
			string resule = BitConverter.ToString(s);
			resule = resule.Replace("-", "");
			return resule.ToLower();
		}
		public string SHA1EncryptStr(string str)
		{
			var sha1 = new SHA1CryptoServiceProvider();
			byte[] s = Encoding.Default.GetBytes(str);
			byte[] hash = sha1.ComputeHash(s);
			string resule = BitConverter.ToString(s);
			resule = resule.Replace("-", "");
			return resule.ToLower();
		}
	}
}
