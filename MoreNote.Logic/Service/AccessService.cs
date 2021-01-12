using Microsoft.Extensions.DependencyInjection;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using MoreNote.Logic.ExtensionMethods.DI;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service
{
    public class AccessService
    {
        private DependencyInjectionService dependencyInjectionService;

        public AccessService(DependencyInjectionService dependencyInjectionService)
        {
            this.dependencyInjectionService = dependencyInjectionService;
        }

        public async Task<bool> InsertAccessAsync(AccessRecords ar)
        {
            using (IServiceScope scope = dependencyInjectionService.GetServiceScope())
            using (DataContext dataContext = scope.GetDataContext())
            {
                var result = dataContext.AccessRecords.Add(ar);
                return await dataContext.SaveChangesAsync() > 0;
            }
        }
    }
}