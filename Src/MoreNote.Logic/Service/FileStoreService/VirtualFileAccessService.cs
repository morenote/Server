using MoreNote.Logic.Database;
using MoreNote.Models.Entity.Leanote;

using System.Collections.Generic;
using System.Linq;

namespace MoreNote.Logic.Service.FileStoreService
{
	public class VirtualFileAccessService
	{
		private DataContext dataContext;

		public VirtualFileAccessService(DataContext dataContext)
		{
			this.dataContext = dataContext;
		}

		public List<VirtualFileInfo> GetRootVFiles(Repository repository)
		{
			var list = this.dataContext.VirtualFileInfo.Where(b => b.Id == repository.Id && b.ParentId == null && b.IsDelete == false).ToList();
			return list;
		}

		public List<VirtualFileInfo> GetChildVFiles(long? id)
		{
			var list = this.dataContext.VirtualFileInfo.Where(b => b.ParentId == id && b.ParentId == null && b.IsDelete == false).ToList();
			return list;
		}

		public void AddVFile(VirtualFileInfo virtualFileInfo)
		{
			this.dataContext.VirtualFileInfo.Add(virtualFileInfo);
		}

		public void DeleteVFile(long? id)
		{
			var node = this.dataContext.VirtualFileInfo.Where(b => b.Id == id).First();
			node.IsDelete = true;
			this.dataContext.SaveChanges();
		}
		public VirtualFileInfo GetVFile(long? id)
		{
			var node = this.dataContext.VirtualFileInfo.Where(b => b.Id == id).First();
			return node;
		}

	}
}