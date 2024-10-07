using MoreNote.Common.Utils;

using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.PasswordSecurity
{
	/// <summary>
	/// 使用SHA256算法处理口令
	/// </summary>
	public class SHA256PasswordStore : IPasswordStore
	{
		public async Task<byte[]> Encryption(byte[] pass, byte[] salt, int iterations)
		{
			using (var sha256 = SHA256.Create())
			{
				byte[] hash = sha256.ComputeHash(pass);

				for (int i = 0; i < iterations; i++)
				{
					if (i % 2 == 0)
					{
						hash = sha256.ComputeHash(Sum(hash, salt));
					}
					else
					{
						hash = sha256.ComputeHash(Sum(salt, hash));
					}
				}
				return hash;
			}
		}

		public async Task<bool> VerifyPassword(byte[] encryData, byte[] pass, byte[] salt, int iterations)
		{
			var hash =await Encryption(pass, salt, iterations);
			return SecurityUtil.SafeCompareByteArray(hash, encryData);
		}

		private byte[] Sum(byte[] s1, byte[] s2)
		{
			byte[] result = new byte[s1.Length + s2.Length];
			Array.Copy(s1, 0, result, 0, s1.Length);
			Array.Copy(s2, 0, result, s1.Length, s2.Length);

			return result;
		}
	}
}