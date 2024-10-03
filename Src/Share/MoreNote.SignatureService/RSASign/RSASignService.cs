using MoreNote.Common.Utils;
using MoreNote.SecurityProvider.Core;

namespace MoreNote.SignatureService.RSASign
{
	public class RSASignService : ISignatureService
	{
		public Task<string> rawSignature(string data)
		{
			throw new NotImplementedException("Software modules do not support signatures");
		}
        public bool GMT0009_VerifySign(byte[] m, byte[] signData, byte[] pubkey, byte[] userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> rawVerify(String data, String sign, String cer, bool usbKey, String pubKeyModulusInHex, String pubKeyExpInHex)
		{
			RSAHelper rsaHelper = new RSAHelper();
			var signBuffer = Convert.FromBase64String(sign);
			var dataBuff = Convert.FromBase64String(data);
			var modulus = Convert.FromBase64String(pubKeyModulusInHex);
			var exponent = HexUtil.HexToByteArray(pubKeyExpInHex);
			await Task.Delay(1);
			bool result = rsaHelper.Verify(signBuffer, dataBuff, modulus, exponent);
			return result;
		}
	}
}
