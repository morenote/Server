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
        private string BucketName;

        private int presignedGetObjectAsyncExpiresInt;

        public MinIOFileStoreService(WebSiteConfig siteConfig)
        {
            this.BucketName = siteConfig.MinIOConfig.BucketName;
            this.presignedGetObjectAsyncExpiresInt = siteConfig.FileStoreConfig.BrowserDownloadExpiresInt;
            minioClient = new MinioClient(siteConfig.MinIOConfig.Endpoint, siteConfig.MinIOConfig.MINIO_ACCESS_KEY, siteConfig.MinIOConfig.MINIO_SECRET_KEY);
            if (siteConfig.MinIOConfig.WithSSL)
            {
                minioClient.WithSSL();
            }
        }
        public MinIOFileStoreService(MinIOConfig minIOConfig)
        {
            
            this.presignedGetObjectAsyncExpiresInt = minIOConfig.BrowserDownloadExpiresInt;
            minioClient = new MinioClient(minIOConfig.Endpoint, minIOConfig.MINIO_ACCESS_KEY, minIOConfig.MINIO_SECRET_KEY);
            if (minIOConfig.WithSSL)
            {
                minioClient.WithSSL();
            }
        }
        public MinIOFileStoreService(string BucketName, string Endpoint, string MINIO_ACCESS_KEY,string MINIO_SECRET_KEY, bool WithSSL,int presignedGetObjectAsyncExpiresInt)
        {
            this.BucketName=BucketName;
            this.presignedGetObjectAsyncExpiresInt = presignedGetObjectAsyncExpiresInt;
            minioClient = new MinioClient(Endpoint, MINIO_ACCESS_KEY, MINIO_SECRET_KEY);
            if (WithSSL)
            {
                minioClient.WithSSL();
            }
        }


        public async Task PutObjectAsync(string objectName, Stream data, long size, string contentType = "application/octet-stream", Dictionary<string, string> metaData = null)
        {
            await minioClient.PutObjectAsync(BucketName, objectName, data, size, contentType, metaData);
        }

        public async Task PutObjectAsync(string objectName, string fileName, string contentType = "application/octet-stream", Dictionary<string, string> metaData = null)
        {
            await minioClient.PutObjectAsync(BucketName, objectName, fileName, contentType, metaData);
        }

        public async Task<string> PresignedGetObjectAsync(string objectName, Dictionary<string, string> reqParams = null)
        {
            String url = await minioClient.PresignedPutObjectAsync(BucketName, objectName, presignedGetObjectAsyncExpiresInt);
            return url;
        }

        public async Task GetObjectAsync(string objectName, Action<Stream> callback)
        {
            // Get input stream to have content of 'my-objectname' from 'my-bucketname'
            await minioClient.GetObjectAsync(BucketName, objectName, callback);
        }

        public async Task<Stream> GetObjectAsync(string objectName)
        {
            // Get input stream to have content of 'my-objectname' from 'my-bucketname'
            Stream stream = null;
            await minioClient.GetObjectAsync(BucketName, objectName, (call) => { stream = call; });
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
            await minioClient.GetObjectAsync(BucketName,

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