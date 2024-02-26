using MoreNote.Logic.Database;
using MoreNote.Models.Entity.Leanote.Management.Loggin;

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