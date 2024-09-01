using github.hyfree.SDFWrapper;
using github.hyfree.SDFWrapper.Model;

using MoreNote.Common.Utils;
using MoreNote.Models.Entity.ConfigFile;

namespace MoreNote.CryptographyProvider.EncryptionMachine.StandardCyptographicDevice
{
	public class SDFProvider : ICryptographyProvider
	{
		private static Object Locker = new object();
		private static bool isInitDo = false;

		private void CheckInitSDF()
		{
			if (!isInitDo)
			{
				lock (Locker)
				{
					if (!isInitDo)
					{
						//写入配置文件
						var configText = File.ReadAllText(sdfConfig.ConfigFilePath);
						var fileName = Path.GetFileName(sdfConfig.ConfigFilePath);
						if (File.Exists(fileName))
						{
							File.Delete(fileName);

						}
						File.WriteAllText(fileName, configText);
						isInitDo = true;
					}
				}
			}
		}

		private SDFConfig sdfConfig;

		private SDFHelper sdfHelper;

		public SDFProvider(SDFConfig sDFConfig)
		{
			this.sdfConfig = sDFConfig;
			CheckInitSDF();
			sdfHelper = new SDFHelper();
			sdfHelper.SDF_OpenDevice();
			sdfHelper.SDF_OpenSession();
			sdfHelper.SDF_ImportKey(HexUtil.HexToByteArray(sdfConfig.PucKey), 1);
		}
		private void CheckReconnection()
		{
			try
			{
				Hmac(HexUtil.HexToByteArray("0102030405060708"));

			}
			catch (Exception ex)
			{
				sdfHelper = new SDFHelper();
				sdfHelper.SDF_OpenDevice();
				sdfHelper.SDF_OpenSession();
				sdfHelper.SDF_ImportKey(HexUtil.HexToByteArray(sdfConfig.PucKey), 1);

			}

		}

		public byte[] Hmac(byte[] data)
		{
			var hex = HexUtil.ByteArrayToSHex(data);
			var hamc = sdfHelper.SDF_HMAC(data);

			return hamc;
		}

		public byte[] SM4Decrypt(byte[] data)
		{
			var dec = this.SM4Decrypt(data, new byte[16]);
			return dec;
		}

		public byte[] SM4Encrypt(byte[] data)
		{
			var enc = this.SM4Encrypt(data, new byte[16]);
			return enc;
		}


		public byte[] SM4Decrypt(byte[] data, byte[] iv)
		{
			var dec = sdfHelper.SM4_Decrypt(iv, data, true);
			return dec;
		}

		public byte[] SM4Encrypt(byte[] data, byte[] iv)
		{

			var enc = sdfHelper.SM4_Encrypt(iv, data, true);
			return enc;
		}

		public bool VerifyHmac(byte[] data, byte[] mac)
		{
			var temp = Hmac(data);

			return SecurityUtil.SafeCompareByteArray(temp, mac);
		}

		public byte[] TransEncrypted(byte[] data, byte[] iv)
		{
			if (iv.Length != 16)
			{
				throw new ArgumentException("iv len !=16");
			}

			var plain = SM2Decrypt(data);
			var hex = HexUtil.ByteArrayToSHex(plain);
			var hexIV = HexUtil.ByteArrayToSHex(iv);

			var enc = SM4Encrypt(plain, iv);

			var encHex = HexUtil.ByteArrayToSHex(enc);
			return enc;
		}

		public byte[] SM2Encrypt(byte[] data)
		{
			var sm2 = sdfHelper.SM2_Encrypt(data, 11);

			return sm2.ToByteArrayC1C3C2();
		}

		public byte[] SM2Decrypt(byte[] data)
		{
			var hex = HexUtil.ByteArrayToSHex(data);
			SM2Cipher sm2 = SM2Cipher.InsanceC1C3C2(data, false);

			var pucCipher = sm2.ToECCCipher();

			var dec = sdfHelper.SM2_Decrypt(sm2, 11);
			return dec;
		}

        public byte[] SM3(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}