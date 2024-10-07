using System;
using System.Security.Cryptography;
using System.Text;

namespace MoreNote.Common.Utils
{
	/// <summary>
	/// 安全相关
	/// </summary>
	public class SecurityUtil
	{
		/// <summary>
		/// 安全的比较数组 阻止计时攻击
		/// </summary>
		/// <param name="a1"></param>
		/// <param name="a2"></param>
		/// <returns></returns>
		public static bool SafeCompareByteArray(byte[] a1, byte[] a2)
		{
			if (a1 == null || a2 == null || a1.Length != a2.Length)
			{
				return false;
			}
			//防止计时攻击
			int result = 0;

			for (int i = 0; i < a1.Length; i++)
			{
				result |= a1[i] ^ a2[i];
			}
			return result == 0;
		}

		public static byte[] SignHamc256(byte[] messageBytes, byte[] keyByte)
		{
			using (var hmacsha256 = new HMACSHA256(keyByte))
			{
				byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
				return hashmessage;
			}
		}

		/// <summary>
		/// HAMC256签名 Hex格式
		/// </summary>
		/// <param name="message"></param>
		/// <param name="secret"></param>
		/// <returns></returns>
		public static string SignHamc256Hex(string message, string secret)
		{
			if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(secret))
			{
				throw new Exception("message or secret is null ,in Hamc256");
			}
			var encoding = new System.Text.UTF8Encoding();
			byte[] keyByte = encoding.GetBytes(secret);
			byte[] messageBytes = encoding.GetBytes(message);
			var sign = SignHamc256(messageBytes, keyByte);
			return HexUtil.ByteArrayToHex(sign);
		}

		public static string SignHamc256Base64(string message, string secret)
		{
			if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(secret))
			{
				throw new Exception("message or secret is null ,in Hamc256");
			}
			var encoding = new System.Text.UTF8Encoding();
			byte[] keyByte = Convert.FromBase64String(secret);
			byte[] messageBytes = Convert.FromBase64String(message);
			var sign = SignHamc256(messageBytes, keyByte);
			return Convert.ToBase64String(sign);
		}

		/// <summary>
		/// HAMCSHA1签名
		/// </summary>
		/// <param name="message"></param>
		/// <param name="secret"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static string SignHamcSHA1(string message, string secret)
		{
			if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(secret))
			{
				throw new Exception("message or secret is null ,in Hamc256");
			}
			var encoding = new System.Text.UTF8Encoding();
			byte[] keyByte = encoding.GetBytes(secret);
			byte[] messageBytes = encoding.GetBytes(message);
			using (var hmacsha256 = new HMACSHA1(keyByte))
			{
				byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < hashmessage.Length; i++)
				{
					builder.Append(hashmessage[i].ToString("x2"));
				}
				return builder.ToString();
			}
		}

		/// <summary>
		/// 验证HAMC256
		/// </summary>
		/// <param name="message"></param>
		/// <param name="secret"></param>
		/// <param name="hmac"></param>
		/// <returns></returns>
		public static bool VerifyHamc256Hex(string message, string secret, string hmac)
		{
			if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(secret))
			{
				throw new Exception("message or secret is null ,in Hamc256");
			}
			var encoding = new UTF8Encoding();
			byte[] keyByte = encoding.GetBytes(secret);
			byte[] messageBytes = encoding.GetBytes(message);
			using (var hmacsha256 = new HMACSHA256(keyByte))
			{
				byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
				byte[] hmacByte = HexUtil.HexToByteArray(hmac);

				return SafeCompareByteArray(hashmessage, hmacByte);
			}
		}

		/// <summary>
		/// 验证HMACSHA1
		/// </summary>
		/// <param name="message"></param>
		/// <param name="secret"></param>
		/// <param name="hmac"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static bool VerifyHMACSHA1(string message, string secret, string hmac)
		{
			if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(secret))
			{
				throw new Exception("message or secret is null ,in Hamc256");
			}
			var encoding = new UTF8Encoding();
			byte[] keyByte = encoding.GetBytes(secret);
			byte[] messageBytes = encoding.GetBytes(message);
			using (var hmacsha256 = new HMACSHA1(keyByte))
			{
				byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
				byte[] hmacByte = HexUtil.HexToByteArray(hmac);

				return SafeCompareByteArray(hashmessage, hmacByte);
			}
		}
	}
}