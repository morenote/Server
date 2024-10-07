using BCrypt.Net;

using System;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.PasswordSecurity
{
	/// <summary>
	/// 实现BCrypt算法处理口令
	/// </summary>
	public class BCryptPasswordStore : IPasswordStore
	{
		public  async Task<byte[]>  Encryption(byte[] pass, byte[] salt, int iterations)
		{
			if (salt.Length != 16)
			{
				throw new ArgumentException("Salt must be equal to 16 byte");
			}
			if (iterations < 4 || iterations > 31)
				throw new ArgumentException("Bad number of rounds", "logRounds");
			return BCryptHlper.HashPassword(pass, salt, iterations);



		}

		public async Task<bool> VerifyPassword(byte[] encryData, byte[] pass, byte[] salt, int iterations)
		{

			return BCryptHlper.Verify(encryData, pass, salt, iterations);
		}
	}
}
