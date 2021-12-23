using MoreNote.Logic.Database;
using MoreNote.Logic.Entity;
using System;
using System.Linq;

namespace MoreNote.Logic.Service
{
    public class APPStoreInfoService
    {
        private DataContext dataContext;

        public APPStoreInfoService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public AppInfo[] GetAPPList()
        {
            var result = dataContext.AppInfo;
            return result.ToArray();
        }

        public bool AddAPP(AppInfo appInfo)
        {
            int a = 0;
            try
            {
                var result = dataContext.AppInfo.Add(appInfo);
                a = dataContext.SaveChanges();
                return a > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}