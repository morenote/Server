using Microsoft.VisualStudio.TestTools.UnitTesting;

using MoreNote.Common.Utils;

namespace MoreNote.SignatureService.RSASign.Tests
{
	[TestClass()]
	public class RSASignServiceTests
	{
		[TestMethod()]
		public void rawVerifyTest()
		{
			RSASignService rsa = new RSASignService();
			var data = "SWQ9MTY0MjMwODE5MTg5MzE5NjgwMFVzZXJJZD0xNjQyMTQwNjA1MTc1OTU5NTUyVGFnPUxvZ2luQ2hhbGxlbmdlUmVxdWVzdE51bWJlcj1lZTk4Mzk5NzhiMGYyYWJiZjZkODQ4NDg2MzUxOGYxZjkwMWM1YjNlZWFiNzkxODQzNGFmMWFlYmExZWI2NDE0UmFuZG9tPTlXdXp4T2FkbGpTeXBSYVdnNjVWOGpCQXNQOCtQYnlGZ2pFTTVWMnNwN289VWlueFRpbWU9MTY4MDM5MTc2MA==";
			var sign = "Clv9kwIbesMoRPzjsKmmJt4cvNbUHg0UBWhXZ1PZVJRmukNX22TfpHZ7K5p5p6fd0rZO3uUL876XnpaMUfxHlIkUsCB4mbVVwSE22ocxy4yPx5fnC6w8yzG2hHk9+NVyVIa9yuFChp1OIRYM8L3Jf4ohQd/zcXwrHHb6oyLvlfQ=";
			var cer = "";
			var usbkey = true;
			var pubkey = "sUDSKDKBCBUha/2APte/esDE4VmGVI1OUUk0FfbrO4xjl7/oE1KFnrff+JmE4XuSZGsCR2l1nV6fPOAhvahlUvlbnuldQKaBfF3sXjH9xDWinittlv3wf5jTIcJf2DhYz3IBI6MCNZxOJ6tOt/ej+TsqB6RXQwNBfAPa4bXFO7U=";
			var pubKeyExpInHex = "010001";

			var signBuffer = Convert.FromBase64String(sign);
			var dataBuff = Convert.FromBase64String(data);
			var modulus = Convert.FromBase64String(pubkey);
			var exponent = HexUtil.HexToByteArray(pubKeyExpInHex);
			RSAHelper rsaHelper = new RSAHelper();
			bool result = rsaHelper.Verify(signBuffer, dataBuff, modulus, exponent);
			Console.WriteLine(result.ToString());
		}
	}
}