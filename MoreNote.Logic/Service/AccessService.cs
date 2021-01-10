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
       private DataContext dataContext;

       public AccessService(DependencyInjectionService dependencyInjectionService,DataContext dataContext)
       {
           this.dataContext = dataContext;
       }

       public  async  Task<bool> InsertAccessAsync(AccessRecords ar)
        {
          
            
                var result = dataContext.AccessRecords.Add(ar);
                return await dataContext.SaveChangesAsync() > 0;
            
        }
    }
}
