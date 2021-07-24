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
        /// 通过文件上传对象
        /// </summary>
        /// <param name="bucketName">桶名称</param>
        /// <param name="objectName">对象名称</param>
        /// <param name="fileName">本地文件完整路径名称</param>
        /// <param name="contentType">HTTP content-type</param>
        /// <param name="metaData">其他元数据</param>
        /// <returns></returns>
        public Task PutObjectAsync(string bucketName, string objectName, string fileName, string contentType = "application/octet-stream", Dictionary<string, string> metaData = null);
        /// <summary>
        /// 通过流上传对象
        /// </summary>
        /// <param name="bucketName">桶名称</param>
        /// <param name="objectName">对象名称</param>
        /// <param name="data">数据流</param>
        /// <param name="size">数据大小</param>
        /// <param name="contentType">HTTP content-type</param>
        /// <param name="metaData">其他元数据</param>
        /// <returns></returns>
        public abstract Task PutObjectAsync(string bucketName, string objectName, Stream data, long size, string contentType = "application/octet-stream", Dictionary<string, string> metaData = null);

        /// <summary>
        /// 生成一个给HTTP GET请求用的presigned URL
        /// </summary>
        /// <param name="bucketName">桶名称</param>
        /// <param name="objectName">对象名称</param>
        /// <param name="reqParams"></param>
        /// <returns></returns>
        public abstract Task<string> PresignedGetObjectAsync(string bucketName, string objectName, Dictionary<string, string> reqParams = null);

        public abstract Task GetObjectAsync(string bucketName, string objectName, Action<Stream> callback);
        public abstract Task<Stream> GetObjectAsync(string bucketName, string objectName);

        public abstract Task<byte[]> GetObjecByteArraytAsync(string bucketName, string objectName);
        public abstract  Task RemoveObjectAsync(string bucketName, string objectName);
    }
}
