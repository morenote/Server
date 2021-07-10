using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.FileService.IMPL
{
    /// <summary>
    /// 本地文件存储服务
    /// </summary>
    public class DiskFileStoreService : IFileStorageService
    {
        public Task<byte[]> GetObjecByteArraytAsync(string objectName)
        {
            throw new NotImplementedException();
        }

        public Task GetObjectAsync(string objectName, Action<Stream> callback)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> GetObjectAsync(string objectName)
        {
            throw new NotImplementedException();
        }

        public Task<string> PresignedGetObjectAsync(string objectName, Dictionary<string, string> reqParams = null)
        {
            throw new NotImplementedException();
        }

        public Task PutObjectAsync(Stream data, string objectName, string fileName, string contentType, Dictionary<string, string> metaData)
        {
            throw new NotImplementedException();
        }

        public Task PutObjectAsync(string objectName, string fileName, string contentType = "application/octet-stream", Dictionary<string, string> metaData = null)
        {
            throw new NotImplementedException();
        }

        public Task PutObjectAsync(string objectName, Stream data, long size, string contentType = "application/octet-stream", Dictionary<string, string> metaData = null)
        {
            throw new NotImplementedException();
        }
    }
}
