using WebApiClient;
using WebApiClient.Attributes;

namespace MoreNote.SignatureService.NetSign
{

	public interface INetSignApi : IHttpApi
	{


		[HttpPost("/rawSignature")]
		public Task<String> rawSignature(String data);

		[HttpPost("/rawVerify")]
		public Task<bool> rawVerify(String data, String sign, String cer, bool usbKey, String pubKeyModulusInHex, String pubKeyExpInHex);


	}
}
