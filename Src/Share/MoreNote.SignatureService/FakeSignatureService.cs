﻿using MoreNote.SecurityProvider.Core;

namespace MoreNote.SignatureService
{
	/// <summary>
	/// 虚假签名服务（仅供测试用途）
	/// </summary>
	public class FakeSignatureService : ISignatureService
	{
		public async Task<string> rawSignature(string data)
		{
			await Task.Delay(1);
			return "QeZYnN6JtPjIEKggwvtvCthr8sE2oZz7OlwINfWY4Hs=";
		}

		public async Task<bool> rawVerify(string data, string sign, string cer, bool usbKey, string pubKeyModulusInHex, string pubKeyExpInHex)
		{
			await Task.Delay(1);
			return true;
		}

        public bool GMT0009_VerifySign(byte[] m, byte[] signData, byte[] pubkey, byte[] userId)
        {
            throw new NotImplementedException();
        }
    }
}
