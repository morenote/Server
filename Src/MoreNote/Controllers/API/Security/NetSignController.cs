using Microsoft.AspNetCore.Mvc;
using MoreNote.SignatureService;
using System.Threading.Tasks;

namespace MoreNote.Controllers.API.Security
{
    [Route("api/netsign/[action]")]
    public class NetSignController : Controller
    {
        ISignatureService signatureService;
       public NetSignController(ISignatureService signatureService)
        {
            this.signatureService = signatureService;

        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var plainBase64 = "aGVsbG93b3JsZA==";
            var signBase64 = "Qc8LvFJ35pv2ehTJ5b3Tf3znsKEyOGEZPETcx5VQUUunnSvZFwtwnk6ExGa2CPsM4DYDz3c2prw84BGc+sFsnMvSt6ctowvRl+uz/aO02OFTuqjH5ZiC3y+MO6WzZNEdgq5k45SSUkAcB6NN0mH6mBd475iCA5VvtZB7AsD1T1BTIlz/0Zp34BHORem5Dl5FRHHtfDlRAHaYr2/AJTcSwHKzQ2sp2X0iY+yxiS+2UWc0mWMjGKj6sQT8ocaqfRFH3/90lQvpB9KLEawBMSFNx2RsK6muqAfI9MX5cfXkzH7lHRDVNKOCOAQPvtNd17acKo6RgzJ9ng3kg7NfM7CF3g==";
            var cerBase64 = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAiTvUn5RUyxg6SgwLIKAEz/oRZ3dFwnCZJ1AygJfcV3VZD5k+1Oll1WS8lNyVO3mF0FmWnaEOkHHgd1FkYpQbUoSByBzxnD2KzRcVuLaD/U7PItibtlr4U5Wjz+1MEjMi//DbqdxBmZg4gF8sZu6rnfrqSIRadISWq2Yc7/KD2I1UGPP29XxKSzF7mXK0+CUeqfzsj4EzwbNi9yYkTwxKEFjPpxSYZJN4eFg1GBzqOqGQcd4LVPwTGseIqhFjjpFPE2N7kSecuAW6xB1Io71kAPLsWCDb6g3gxk2wQvlKkPhGfgt+nsfnbwBpu4GR4/0mFmB3cruqnaWD8en6A/0uswIDAQAB";
            var sign= await  signatureService.rawSignature(plainBase64);
            var verify = await signatureService.rawVerify(plainBase64, signBase64,cerBase64);
            return Content("签名结果="+sign+"\r\n验签结果="+verify);
        }
    }
}
