using Microsoft.Extensions.DependencyInjection;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using MoreNote.Logic.ExtensionMethods.DI;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service
{
    public class AccessService
    {
       private DataContext dataContext;

        public AccessService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<bool> InsertAccessAsync(AccessRecords ar)
        {
           
                var result = dataContext.AccessRecords.Add(ar);
                return await dataContext.SaveChangesAsync() > 0;
            
        }
    }
}