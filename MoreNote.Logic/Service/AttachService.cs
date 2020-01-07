using System;
using System.Collections.Generic;
using System.Text;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;

namespace MoreNote.Logic.Service
{
    public class AttachService
    {
        public static bool AddAttach(AttachInfo attachInfo,bool fromApi)
        {
            attachInfo.CreatedTime = DateTime.Now;
            int a = 0;
            using (var db = new DataContext())
            {
                var result = db.AttachInfo.Add(attachInfo);
                a = db.SaveChanges();
                
            }
            if (a < 1) return false;

            return false;



        }

    }
}
