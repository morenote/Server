using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Google.Authenticator;

using MoreNote.Common.Util;
using MoreNote.Logic.DB;

namespace MoreNote.Logic.Service
{
    public class GoogleAuthenticatorService
    {
        public static void SetSecretKey(long userId,string secretKey)
        {
            using (var db= DataContext.getDataContext())
            {
                var user = db.User.Where(b => b.UserId == userId).FirstOrDefault();
                user.GoogleAuthenticatorSecretKey = secretKey;
                db.SaveChanges();
            }
        }
        public static string GetSecretKey(long userId)
        {
            using (var db = DataContext.getDataContext())
            {
                var user = db.User.Where(b => b.UserId == userId).FirstOrDefault();
                return user.GoogleAuthenticatorSecretKey;
            }
        }
        public static string GenerateSecretKey()
        {
           string secretKey=RandomTool.CreatSafeSalt();
           return secretKey;
        }
        public static SetupCode GenerateSetup(string issuer, string accountTitleNoSpaces, string accountSecretKey,bool secretIsBase32=false,int QRPixelsPerModule=3)
        {
            TwoFactorAuthenticator tfA = new TwoFactorAuthenticator();
            var setupCode = tfA.GenerateSetupCode(issuer, accountTitleNoSpaces, accountSecretKey, secretIsBase32, QRPixelsPerModule,false);
            return setupCode;
        }
        public static byte[] GetQRCode( string accountSecretKey, bool secretIsBase32 = false, int QRPixelsPerModule = 3)
        {
            var issuer = "www.morenote.top";
            var accountTitleNoSpaces = "www.morenote.top";
            //todo:自定义issuer和accountTitleNoSpaces
            TwoFactorAuthenticator tfA = new TwoFactorAuthenticator();
            var image = tfA.GenerateSetupCodeImage(issuer, accountTitleNoSpaces, accountSecretKey, secretIsBase32, QRPixelsPerModule);
            return image;
        }
        public static byte[] GetQRCode(string issuer, string accountTitleNoSpaces, string accountSecretKey, bool secretIsBase32 = false, int QRPixelsPerModule = 3)
        {
            TwoFactorAuthenticator tfA = new TwoFactorAuthenticator();
            var image = tfA.GenerateSetupCodeImage(issuer, accountTitleNoSpaces, accountSecretKey, secretIsBase32, QRPixelsPerModule);
            return image;
        }
        public static bool TestTwoFactorCode(string secretKey,string code)
        {
            TwoFactorAuthenticator tfA = new TwoFactorAuthenticator();
            var result = tfA.ValidateTwoFactorPIN(secretKey, code);
            return result;

        } 
        public static string[] GetCurrent(string secretKey)
        {
            TwoFactorAuthenticator tfA = new TwoFactorAuthenticator();
            string[] pins = tfA.GetCurrentPINs(secretKey);
            return pins;
        }
    }
}
