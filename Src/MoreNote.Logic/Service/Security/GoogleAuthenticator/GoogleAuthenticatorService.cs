using Google.Authenticator;
using MoreNote.Common.Utils;
using MoreNote.Logic.Database;
using System.Linq;

namespace MoreNote.Logic.Service
{
    /// <summary>
    /// 谷歌两步认证服务
    /// </summary>
    public class GoogleAuthenticatorService
    {


        private DataContext dataContext;
        public GoogleAuthenticatorService(DataContext dataContext)
        {
            this.dataContext=dataContext;
         
        }

        public void SetSecretKey(long? userId, string secretKey)
        {
           
                var user = dataContext.User.Where(b => b.UserId == userId).FirstOrDefault();
                user.GoogleAuthenticatorSecretKey = secretKey;
                dataContext.SaveChanges();
            
        }

        public string GetSecretKey(long? userId)
        {
           
                var user = dataContext.User.Where(b => b.UserId == userId).FirstOrDefault();
                return user.GoogleAuthenticatorSecretKey;
            
        }

        public string GenerateSecretKey()
        {
            string secretKey = RandomTool.CreatSafeSaltBase64();
            return secretKey;
        }

        public SetupCode GenerateSetup(string issuer, string accountTitleNoSpaces, string accountSecretKey, bool secretIsBase32 = false, int QRPixelsPerModule = 3)
        {
            TwoFactorAuthenticator tfA = new TwoFactorAuthenticator();
            var setupCode = tfA.GenerateSetupCode(issuer, accountTitleNoSpaces, accountSecretKey, secretIsBase32, QRPixelsPerModule, false);
            return setupCode;
        }

        public byte[] GetQRCode(string accountSecretKey, bool secretIsBase32 = false, int QRPixelsPerModule = 3)
        {
            var issuer = "www.morenote.top";
            var accountTitleNoSpaces = "www.morenote.top";
            //todo:自定义issuer和accountTitleNoSpaces
            TwoFactorAuthenticator tfA = new TwoFactorAuthenticator();
            var image = tfA.GenerateSetupCodeImage(issuer, accountTitleNoSpaces, accountSecretKey, secretIsBase32, QRPixelsPerModule);
            return image;
        }

        public byte[] GetQRCode(string issuer, string accountTitleNoSpaces, string accountSecretKey, bool secretIsBase32 = false, int QRPixelsPerModule = 3)
        {
            TwoFactorAuthenticator tfA = new TwoFactorAuthenticator();
            var image = tfA.GenerateSetupCodeImage(issuer, accountTitleNoSpaces, accountSecretKey, secretIsBase32, QRPixelsPerModule);
            return image;
        }

        public bool TestTwoFactorCode(string secretKey, string code)
        {
            TwoFactorAuthenticator tfA = new TwoFactorAuthenticator();
            var result = tfA.ValidateTwoFactorPIN(secretKey, code);
            return result;
        }

        public string[] GetCurrent(string secretKey)
        {
            TwoFactorAuthenticator tfA = new TwoFactorAuthenticator();
            string[] pins = tfA.GetCurrentPINs(secretKey);
            return pins;
        }
    }
}