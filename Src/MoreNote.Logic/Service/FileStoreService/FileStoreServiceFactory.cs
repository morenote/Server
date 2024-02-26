using MoreNote.Config.ConfigFile;
using MoreNote.Logic.Service.FileService;
using MoreNote.Logic.Service.FileService.IMPL;

using System;

namespace MoreNote.Logic.Service.FileStoreService
{
	public class FileStoreServiceFactory
	{
		public static IFileStorageService Instance(WebSiteConfig webSiteConfig)
		{
			switch (webSiteConfig.FileStoreConfig.FileStorage)
			{
				case "minio":
					return new MinIOFileStoreService(webSiteConfig);

				case "upyun":
					return new UpyunFileStoreService(webSiteConfig);

				case "disk":
					return new DiskFileStoreService();

				default:

					throw new ArgumentException("FileStoreConfig.FileStorage is error", "FileStorage");
			}
		}
	}
}