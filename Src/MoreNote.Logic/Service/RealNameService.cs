using MoreNote.Logic.Database;
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

        public RealNameService(DataContext dataContext)
        {
            this.dataContext=dataContext;
        }

        public void SetRealName(long? userId,string realName)
        {


        }

        public void GetRealName(long? userId)
        {

        }
        public void verifyHmac(RealNameInformation realName )
        {


        } 
        private  RealNameInformation DoHamc(RealNameInformation realNameInformation)
        {



            return realNameInformation;

        }
    }
}
