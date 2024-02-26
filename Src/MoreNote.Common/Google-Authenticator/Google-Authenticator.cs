using Google.Authenticator;

using System;


namespace MoreNote.Common.GoogleAuthenticator
{
	public class GoogleAuthenticatorHelper
	{
		public static String GetqrCodeImageUrl()
		{
			TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
			var setupInfo = tfa.GenerateSetupCode("Test Two Factor", "user@example.com", "key", false, 3);

			string qrCodeImageUrl = setupInfo.QrCodeSetupImageUrl;
			string manualEntrySetupCode = setupInfo.ManualEntryKey;
			Console.WriteLine(manualEntrySetupCode);
			return qrCodeImageUrl;
		}

	}
}
