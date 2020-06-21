using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;

namespace MoreNote.Logic.Service
{
    public class APPStoreInfoService
    {
        public static AppInfo[] GetAPPList()
        {

            using (var db = DataContext.getDataContext())
            {
                var result = db.AppInfo;
                return result.ToArray();
            }
        }
        public static bool AddAPP(AppInfo appInfo)
        {
            int a = 0;
            try
            {
                using (var db = DataContext.getDataContext())
                {
                    var result = db.AppInfo.Add(appInfo);
                    a = db.SaveChanges();
                    return a > 0;
                }
            }catch(Exception ex)
            {
                return false;
            }
        
        }
    }
}
