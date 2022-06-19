using MoreNote.CryptographyProvider;
using MoreNote.Logic.Database;
using MoreNote.Logic.Service.DistributedIDGenerator;
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
        DataContext dataContext;
        IDistributedIdGenerator distributedIdGenerator;
        ICryptographyProvider cryptographyProvider;
        public RealNameService(DataContext dataContext, 
            IDistributedIdGenerator distributedIdGenerator,
            ICryptographyProvider cryptographyProvider)
        {
            this.dataContext=dataContext;
            this.distributedIdGenerator=distributedIdGenerator;
            this.cryptographyProvider=cryptographyProvider;
        }

        public async void SetRealNameInformation(long? userId,string realName)
        {
            var rName=new RealNameInformation()
            {
                Id=this.distributedIdGenerator.NextId(),
                UserId=userId,
                RealName=realName
            };
            rName = await rName.AddMac(this.cryptographyProvider);

            dataContext.RealNameInformation.Add(rName);
            dataContext.SaveChanges();
        }

        public async Task<RealNameInformation> GetRealNameInformation(long? userId)
        {
            var realName=dataContext.RealNameInformation.Where(b=>b.UserId==userId).FirstOrDefault();
            if (realName==null)
            {
                return null;
            }
            await realName.VerifyHmac(this.cryptographyProvider);
            return realName;
        }
       
    }
}
