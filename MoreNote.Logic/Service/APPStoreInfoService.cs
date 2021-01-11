using MoreNote.Logic.Entity;
using System;
using System.Linq;

namespace MoreNote.Logic.Service
{
    public class APPStoreInfoService
    {
        private DependencyInjectionService dependencyInjectionService;

        public APPStoreInfoService(DependencyInjectionService dependencyInjectionService)
        {
            this.dependencyInjectionService = dependencyInjectionService;
        }

        public AppInfo[] GetAPPList()
        {
            using (var dataContext = dependencyInjectionService.GetDataContext())
            {
                var result = dataContext.AppInfo;
                return result.ToArray();
            }
        }

        public bool AddAPP(AppInfo appInfo)
        {
            int a = 0;
            try
            {
                using (var dataContext = dependencyInjectionService.GetDataContext())
                {
                    var result = dataContext.AppInfo.Add(appInfo);
                    a = dataContext.SaveChanges();
                    return a > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}