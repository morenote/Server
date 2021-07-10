using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.FileService
{
    /// <summary>
    /// 文件储存抽象接口
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        /// 通过文件上传对象。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="metaData"></param>
        /// <returns></returns>
        public  Task PutObjectAsync(string objectName, string fileName, string contentType = "application/octet-stream", Dictionary<string, string> metaData = null);
        /// <summary>
        /// 通过流上传对象
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <param name="contentType"></param>
        /// <param name="metaData"></param>
        /// <returns></returns>
        public abstract Task PutObjectAsync(string objectName, Stream data, long size, string contentType = "application/octet-stream", Dictionary<string, string> metaData = null);

        /// <summary>
        /// 生成一个给HTTP GET请求用的presigned URL
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="reqParams"></param>
        /// <returns></returns>
        public abstract Task<string> PresignedGetObjectAsync(string objectName, Dictionary<string, string> reqParams = null);

        public abstract Task GetObjectAsync(string objectName, Action<Stream> callback);
        public abstract Task<Stream> GetObjectAsync(string objectName);

        public abstract Task<byte[]> GetObjecByteArraytAsync(string objectName);
    }
}
