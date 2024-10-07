using MoreNote.SecurityProvider.Core;

using WebApiClient;
using WebApiClient.Attributes;

namespace MoreNote.SignatureService.NetSign
{
    [HttpHost("http://localhost:8080/")]
    public interface ISJJApi : IHttpApi
	{


		[HttpPost("/hmac")]
		public Task<EncryptedResult> hmac(String data);

		[HttpPost("/verifyHmac")]
		public Task<EncryptedResult> verifyHmac(String data, String mac);
        [HttpPost("/encrypt")]
        public Task<EncryptedResult> encrypt(String data);
        [HttpPost("/decrypt")]
        public Task<EncryptedResult> decrypt(String data);

    }
}
