using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;

namespace MoreNote.Logic.Service
{
   public  class AccessService
    {
        public static async  Task<bool> InsertAccessAsync(AccessRecords ar)
        {
          
            using (var db = DataContext.getDataContext())
            {
                var result = db.AccessRecords.Add(ar);
                return await db.SaveChangesAsync() > 0;
            }
        }
    }
}
