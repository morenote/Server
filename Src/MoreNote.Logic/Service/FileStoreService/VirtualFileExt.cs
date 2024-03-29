﻿using MoreNote.Models.Entity.Leanote;

using System.IO;

namespace MoreNote.Logic.Service.FileStoreService
{
	public static class VirtualFileExt
	{
		public static VirtualFileInfo ToVirtualFile(this FileInfo fileInfo)
		{
			var vf = new VirtualFileInfo()
			{
				Name = fileInfo.Name,
				ModifyDate = fileInfo.LastWriteTime,
				Size = fileInfo.Length
			};
			return vf;
		}
		public static VirtualFolderInfo ToVirtualFolder(this DirectoryInfo directoryInfo)
		{
			var vf = new VirtualFolderInfo()
			{

				Name = directoryInfo.Name,
				ModifyDate = directoryInfo.LastWriteTime,
			};
			return vf;
		}
	}
}
