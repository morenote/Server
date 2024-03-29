﻿using MoreNote.Config.ConfigFile;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.FileService.IMPL
{
	/// <summary>
	/// 又拍云文件存储服务
	/// </summary>
	public class UpyunFileStoreService : IFileStorageService
	{
		public UpyunFileStoreService(WebSiteConfig siteConfig)
		{

		}

		public Task<byte[]> GetObjecByteArraytAsync(string bucketName, string objectName)
		{
			throw new NotImplementedException();
		}

		public Task GetObjectAsync(string bucketName, string objectName, Action<Stream> callback)
		{
			throw new NotImplementedException();
		}

		public Task<Stream> GetObjectAsync(string bucketName, string objectName)
		{
			throw new NotImplementedException();
		}

		public Task GetObjectAsync(string bucketName, string objectName, string fileName)
		{
			throw new NotImplementedException();
		}

		public Task<string> PresignedGetObjectAsync(string bucketName, string objectName, Dictionary<string, string> reqParams = null)
		{
			throw new NotImplementedException();
		}

		public Task PutObjectAsync(string bucketName, Stream data, string objectName, string fileName, string contentType, Dictionary<string, string> metaData)
		{
			throw new NotImplementedException();
		}

		public Task PutObjectAsync(string bucketName, string objectName, string fileName, string contentType = "application/octet-stream", Dictionary<string, string> metaData = null)
		{
			throw new NotImplementedException();
		}

		public Task PutObjectAsync(string bucketName, string objectName, Stream data, long size, string contentType = "application/octet-stream", Dictionary<string, string> metaData = null)
		{
			throw new NotImplementedException();
		}

		public Task RemoveObjectAsync(string bucketName, string objectName)
		{
			throw new NotImplementedException();
		}
	}
}
