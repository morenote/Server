using MoreNote.SecurityProvider.Core;

using System.Security.Cryptography;

namespace MoreNote.CryptographyProvider
{
	public class FakeCryptographyProvider : ICryptographyProvider
	{
		/// <summary>
		/// 虚假的hamc
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public byte[] Hmac(byte[] data)
		{

			var hash = SHA256.Create().ComputeHash(data);


			return hash;
		}

		public byte[] SM2Decrypt(byte[] data)
		{
			throw new NotImplementedException();
		}

		public byte[] SM2Encrypt(byte[] data)
		{
			throw new NotImplementedException();
		}

        public byte[] SM3(byte[] data)
        {
            throw new NotImplementedException();
        }

        public byte[] SM4Decrypt(byte[] data, byte[] iv)
		{
			throw new NotImplementedException();
		}

		public byte[] SM4Decrypt(byte[] data)
		{
			throw new NotImplementedException();
		}

		public byte[] SM4Encrypt(byte[] data, byte[] iv)
		{
			throw new NotImplementedException();
		}

		public byte[] SM4Encrypt(byte[] data)
		{
			throw new NotImplementedException();
		}

		public byte[] TransEncrypted(byte[] cipher, byte[] salt)
		{
			throw new NotImplementedException();
		}

		public bool VerifyHmac(byte[] data, byte[] mac)
		{
			throw new NotImplementedException();
		}
	}
}
