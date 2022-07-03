using MoreNote.Common.ExtensionMethods;
using MoreNote.CryptographyProvider;
using MoreNote.Logic.Database;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.Service.DistributedIDGenerator;
using MoreNote.Logic.Service.Logging;
using MoreNote.Logic.Service.PasswordSecurity;
using MoreNote.Models.Entity.Leanote;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service
{
    public class RealNameService
    {
        private DataContext dataContext;

        public WebSiteConfig Config { get; set; }
        private IDistributedIdGenerator idGenerator;
        private PasswordStoreFactory passwordStoreFactory;

        private ILoggingService logging { get; set; }

        protected ICryptographyProvider cryptographyProvider { get; set; }

        public RealNameService(DataContext dataContext, IDistributedIdGenerator idGenerator,
            ConfigFileService configFileService,
            PasswordStoreFactory passwordStoreFactory,
            ILoggingService logging,
            ICryptographyProvider cryptographyProvider)
        {
            this.idGenerator = idGenerator;
            this.dataContext = dataContext;
            this.Config = configFileService.WebConfig;
            this.passwordStoreFactory = passwordStoreFactory;
            this.cryptographyProvider = cryptographyProvider;
            this.logging = logging;
        }

        public void SetRealName(long? userId, string cardNo)
        {
            if (dataContext.RealNameInformation.Where(b => b.UserId == userId).Any())
            {
                var realName = dataContext.RealNameInformation.Where(b => b.UserId == userId).FirstOrDefault();
                realName.IdCardNo = cardNo;
                if (this.Config.SecurityConfig.DataBaseEncryption)
                {
                    realName.IdCardNo = this.cryptographyProvider.SM4Encrypt(Encoding.UTF8.GetBytes(cardNo)).ByteArrayToBase64();
                    realName.IsEncryption = true;
                }

                realName.AddMac(this.cryptographyProvider);
                this.dataContext.SaveChanges();
            }
            else
            {
                var rni = new RealNameInformation()
                {
                    Id = this.idGenerator.NextId(),
                    UserId = userId,
                    IdCardNo = cardNo
                };
                if (this.Config.SecurityConfig.DataBaseEncryption)
                {
                    rni.IdCardNo = this.cryptographyProvider.SM4Encrypt(Encoding.UTF8.GetBytes(cardNo)).ByteArrayToBase64();
                    rni.IsEncryption = true;
                }
                rni.AddMac(this.cryptographyProvider);

                this.dataContext.RealNameInformation.Add(rni);
                this.dataContext.SaveChanges();
            }
        }

        public RealNameInformation GetRealNameInformationByUserId(long? userId)
        {
            var realName = this.dataContext.RealNameInformation.Where(b => b.UserId == userId).FirstOrDefault();
            if (realName == null)
            {
                return null;
            }
            if (this.Config.SecurityConfig.LogNeedHmac)
            {
                realName.VerifyHmac(this.cryptographyProvider);

            }
            if (realName.IsEncryption)
            {
                var dec = this.cryptographyProvider.SM4Decrypt(realName.IdCardNo.Base64ToByteArray());
                realName.IdCardNo = Encoding.UTF8.GetString(dec);
            }
           

            return realName;
        }
    }
}