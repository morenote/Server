namespace MoreNote.SignatureService.NetSign
{
	public class NetSignService : ISignatureService
	{

		INetSignApi _netSignApi;
		public NetSignService(INetSignApi netSignApi)
		{
			_netSignApi = netSignApi;
		}

        public bool GMT0009_VerifySign(byte[] m, byte[] signData, byte[] pubkey, byte[] userId)
        {
            throw new NotImplementedException();
        }

        public Task<string> rawSignature(string data)
		{
			return _netSignApi.rawSignature(data);
		}

		public Task<bool> rawVerify(String data, String sign, String cer, bool usbKey, String pubKeyModulusInHex, String pubKeyExpInHex)
		{
			return this._netSignApi.rawVerify(data, sign, cer, usbKey, pubKeyModulusInHex, pubKeyExpInHex);
		}
	}
}
