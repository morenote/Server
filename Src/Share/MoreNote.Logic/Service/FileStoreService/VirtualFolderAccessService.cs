using MoreNote.Logic.Database;
using MoreNote.Logic.Service.DistributedIDGenerator;
using MoreNote.Models.Entity.Leanote;
using MoreNote.Models.Entiys.Leanote.Notes;

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MoreNote.Logic.Service.FileStoreService
{
	public class VirtualFolderAccessService
	{
		private DataContext dataContext;

		private IDistributedIdGenerator idGenerator;
		public VirtualFolderAccessService(DataContext dataContext, IDistributedIdGenerator idGenerator)
		{
			this.dataContext = dataContext;
			this.idGenerator = idGenerator;
		}

		public List<VirtualFolderInfo> GetRootVFolder(Notebook repository)
		{
			//先来个扫描程序 仅用于测试用途
			var fi = new DirectoryInfo(repository.VirtualFileBasePath);
			var fis = fi.GetDirectories();
			foreach (var item in fis)
			{
				if (!IsExist(null, item.Name))
				{
					var vf = item.ToVirtualFolder();
					vf.Id = idGenerator.NextId();
					vf.RepositoryId = repository.Id;
					vf.IsDelete = false;
					AddVFolder(vf);
				}
			}

			var list = this.dataContext.VirtualFolderInfo.Where(b => b.RepositoryId == repository.Id && b.ParentId == null && b.IsDelete == false).ToList();
			return list;
		}

		public List<VirtualFolderInfo> GetChildVFiles(long? id)
		{
			var list = this.dataContext.VirtualFolderInfo.Where(b => b.ParentId == id && b.ParentId == null && b.IsDelete == false).ToList();
			return list;
		}

		public void AddVFolder(VirtualFolderInfo vf)
		{
			this.dataContext.VirtualFolderInfo.Add(vf);
			this.dataContext.SaveChanges();
		}

		public void DeleteVirtualFolderInfo(long? id)
		{
			var node = this.dataContext.VirtualFolderInfo.Where(b => b.Id == id).First();
			node.IsDelete = true;
			this.dataContext.SaveChanges();
		}
		public VirtualFolderInfo GetVirtualFolderInfo(long? id)
		{
			var node = this.dataContext.VirtualFolderInfo.Where(b => b.Id == id).First();
			return node;
		}
		public bool IsExist(long? id)
		{
			return this.dataContext.VirtualFolderInfo.Any(b => b.Id == id);
		}
		public bool IsExist(long? parentIdI, string name)
		{
			return this.dataContext.VirtualFolderInfo.Any(b => b.ParentId == parentIdI && b.Name == name);
		}
	}
}