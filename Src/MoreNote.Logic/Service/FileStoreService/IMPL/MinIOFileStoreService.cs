using Minio;
using MoreNote.Logic.Entity.ConfigFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.FileService.IMPL
{
    /// <summary>
    /// MINIO存储服务
    /// </summary>
    public class MinIOFileStoreService : IFileStorageService
    {
        private MinioClient minioClient;
        private MinIOConfig minIOConfig;

        private int presignedGetObjectAsyncExpiresInt;

        public MinIOFileStoreService(WebSiteConfig siteConfig)
        {
            this.minIOConfig = siteConfig.MinIOConfig;
            this.presignedGetObjectAsyncExpiresInt = siteConfig.FileStoreConfig.BrowserDownloadExpiresInt;
            minioClient = new MinioClient(minIOConfig.Endpoint, minIOConfig.MINIO_ACCESS_KEY, minIOConfig.MINIO_SECRET_KEY);
            if (minIOConfig.WithSSL)
            {
                minioClient.WithSSL();
            }
        }

        public async Task PutObjectAsync(string objectName, Stream data, long size, string contentType = "application/octet-stream", Dictionary<string, string> metaData = null)
        {
            await minioClient.PutObjectAsync(minIOConfig.BucketName, objectName, data, size, contentType, metaData);
        }

        public async Task PutObjectAsync(string objectName, string fileName, string contentType = "application/octet-stream", Dictionary<string, string> metaData = null)
        {
            await minioClient.PutObjectAsync(minIOConfig.BucketName, objectName, fileName, contentType, metaData);
        }

        public async Task<string> PresignedGetObjectAsync(string objectName, Dictionary<string, string> reqParams = null)
        {
            String url = await minioClient.PresignedPutObjectAsync(minIOConfig.BucketName, objectName, presignedGetObjectAsyncExpiresInt);
            return url;
        }

        public async Task GetObjectAsync(string objectName, Action<Stream> callback)
        {
            // Get input stream to have content of 'my-objectname' from 'my-bucketname'
            await minioClient.GetObjectAsync(minIOConfig.BucketName, objectName, callback);
        }

        public async Task<Stream> GetObjectAsync(string objectName)
        {
            // Get input stream to have content of 'my-objectname' from 'my-bucketname'
            Stream stream = null;
            await minioClient.GetObjectAsync(minIOConfig.BucketName, objectName, (call) => { stream = call; });
            while (stream == null)
            {
                Thread.Yield();
            }
            return stream;
        }

        public async Task<byte[]> GetObjecByteArraytAsync(string objectName)
        {
            // Get input stream to have content of 'my-objectname' from 'my-bucketname'
            byte[] data = null;
            Semaphore sem = new Semaphore(1, 1);
            sem.WaitOne(5000);
            await minioClient.GetObjectAsync(minIOConfig.BucketName,

                objectName,  (callStream) =>
                {
                    MemoryStream stmMemory = new MemoryStream();
                      callStream.CopyTo(stmMemory);
                    data= stmMemory.GetBuffer();
                    sem.Release();
                });
            sem.WaitOne(5000);
           
            return data;
        }
    }
}