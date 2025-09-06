﻿using github.hyfree.GM;

using MoreNote.Common.Utils;
using MoreNote.Config.ConfigFile;
using MoreNote.Logic.Service;
using MoreNote.SecurityProvider.Core;

using System.Text;

namespace MoreNote.CryptographyProvider
{
	public class SoftCryptographyProvider : ICryptographyProvider
	{
		WebSiteConfig webSiteConfig;
		byte[] masterKey;
		byte[] hmacKey;
		byte[] enckey;
		byte[] sm2Pubky;
		byte[] sm2PriKey;

		public SoftCryptographyProvider(ConfigFileService configFileService)
		{
			GMService gm = new GMService();
			this.webSiteConfig = configFileService.ReadConfig();
			//todo:实际上，我们必须理解由于Secret无法被保护，所以如果宿主机本身被攻击，一切加密手段都是毫无意义的，只能充当安慰剂。
			//这样做的唯一价值就是通过派生子密钥，减少Secret暴露的机会。
			masterKey = gm.PBKDF2_SM3(Convert.FromBase64String(webSiteConfig.SecurityConfig.Secret), Encoding.UTF8.GetBytes("masterKey"), 10000, 16);
			hmacKey = gm.PBKDF2_SM3(masterKey, Encoding.UTF8.GetBytes("hmacKey"), 10000, 16);
			enckey = gm.PBKDF2_SM3(masterKey, Encoding.UTF8.GetBytes("encKey"), 10000, 16);

			var str=HexUtil.ByteArrayToHex(enckey);
            sm2Pubky = HexUtil.HexToByteArray(webSiteConfig.SecurityConfig.PublicKey);
			sm2PriKey = HexUtil.HexToByteArray(webSiteConfig.SecurityConfig.PrivateKey);
		}

        public async Task<byte[]> Hmac(byte[] data)
		{
			GMService gm = new GMService();
			return gm.Hmac(data, hmacKey);
		}

		public async Task<byte[]> SM2Decrypt(byte[] data)
		{
			GMService gm = new GMService();
			return gm.SM2Decrypt(data, sm2PriKey);
		}

		public async Task<byte[]> SM2Encrypt(byte[] data)
		{
			GMService gm = new GMService();
			return gm.SM2Encrypt(data, sm2PriKey);
		}

        public  async Task<byte[]> SM3(byte[] data)
        {
            GMService gm = new GMService();
			return gm.SM3(data);

        }

        public async Task<byte[]> SM4Decrypt(byte[] data, byte[] iv)
		{
            await Task.Delay(0);
            GMService gm = new GMService();
			return gm.SM4_Decrypt_CBC(data, enckey, iv);
		}

        public async Task<byte[]> SM4Decrypt(byte[] data)
		{
            await Task.Delay(0);
            GMService gm = new GMService();
			byte[] iv = new byte[16];//固定值
			return gm.SM4_Decrypt_CBC(data, enckey, iv);
		}

        public async Task<byte[]> SM4Encrypt(byte[] data, byte[] iv)
		{
			await Task.Delay(0);
			GMService gm = new GMService();
			return gm.SM4_Encrypt_CBC(data, enckey, iv);
		}

        public async Task<byte[]> SM4Encrypt(byte[] data)
		{
            await Task.Delay(0);
            GMService gm = new GMService();
			byte[] iv = new byte[16];//固定值
			return gm.SM4_Encrypt_CBC(data, enckey, iv);
		}

		public async Task<byte[]> TransEncrypted(byte[] cipher, byte[] salt)
		{

			GMService gm = new GMService();


			byte[] data = new byte[cipher.Length + 1];
			Array.Copy(cipher, 0, data, 1, cipher.Length);
			data[0] = 0x04;
			var Hexkey = HexUtil.ByteArrayToHex(sm2PriKey);
			var Hexsalt = HexUtil.ByteArrayToHex(salt);
			var HexPass = HexUtil.ByteArrayToHex(cipher);
			var plainText = gm.SM2Decrypt(data, sm2PriKey);
			byte[] iv = new byte[16];//固定值
			var enc = gm.SM4_Encrypt_CBC(plainText, enckey, iv);
			return enc;

		}

        public async Task<bool> VerifyHmac(byte[] data, byte[] mac)
		{
			GMService gm = new GMService();
			var temp = gm.Hmac(data, hmacKey);
			return SecurityUtil.SafeCompareByteArray(temp, mac);
		}
	}
}