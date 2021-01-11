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
       private DependencyInjectionService dependencyInjectionService;

       public AccessService(DependencyInjectionService dependencyInjectionService)
       {
           this.dependencyInjectionService = dependencyInjectionService;
       }

       public  async  Task<bool> InsertAccessAsync(AccessRecords ar)
        {
            using(DataContext dataContext = dependencyInjectionService.GetDataContext())
            {
                   var result = dataContext.AccessRecords.Add(ar);
                return await dataContext.SaveChangesAsync() > 0;
            }
        }
    }
}
