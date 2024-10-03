namespace MoreNote.SecurityProvider.Core
{
	public interface ISignatureService
	{
		public Task<String> rawSignature(String data);


		public Task<bool> rawVerify(String data, String sign, String cer, bool usbKey, String pubKeyModulusInHex, String pubKeyExpInHex);


        public bool GMT0009_VerifySign(byte[] m, byte[] signData, byte[] pubkey, byte[] userId);

    }
}