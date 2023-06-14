using CommunityToolkit.HighPerformance;

using Minio;
using Minio.DataModel;

using MoreNote.Logic.Entity.ConfigFile;
using SharpCompress.Common;

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
   

        private int presignedGetObjectAsyncExpiresInt;

        public MinIOFileStoreService(WebSiteConfig siteConfig)
        {
                var minIOConfig=siteConfig.MinIOConfig;
            Init(minIOConfig.Endpoint, minIOConfig.MINIO_ACCESS_KEY, minIOConfig.MINIO_SECRET_KEY, minIOConfig.WithSSL, minIOConfig.BrowserDownloadExpiresInt);

        }
        public MinIOFileStoreService(MinIOConfig minIOConfig)
        {

            Init(minIOConfig.Endpoint,minIOConfig.MINIO_ACCESS_KEY,minIOConfig.MINIO_SECRET_KEY,minIOConfig.WithSSL,minIOConfig.BrowserDownloadExpiresInt);

        }
        public MinIOFileStoreService( string Endpoint, string MINIO_ACCESS_KEY,string MINIO_SECRET_KEY, bool WithSSL,int presignedGetObjectAsyncExpiresInt)
        {
            Init( Endpoint,  MINIO_ACCESS_KEY,  MINIO_SECRET_KEY,  WithSSL,  presignedGetObjectAsyncExpiresInt);

        }
      

        public void Init(string Endpoint, string MINIO_ACCESS_KEY, string MINIO_SECRET_KEY, bool WithSSL, int presignedGetObjectAsyncExpiresInt)
        {

            this.presignedGetObjectAsyncExpiresInt = presignedGetObjectAsyncExpiresInt;
            minioClient = new MinioClient()
                .WithEndpoint(Endpoint)
                .WithCredentials(MINIO_ACCESS_KEY, MINIO_SECRET_KEY);
            if (WithSSL)
            {
                minioClient.WithSSL();
            }
            minioClient.Build();
        }


        public async Task PutObjectAsync(string bucketName, string objectName, Stream data, long size, string contentType, Dictionary<string, string> metaData = null)
        {
            PutObjectArgs putObjectArgs = new PutObjectArgs()
                                     .WithBucket(bucketName)
                                     .WithObject(objectName)
                                     .WithStreamData(data)
                                        .WithContentType(contentType)
                                        .WithObjectSize(size);
                                    

            await minioClient.PutObjectAsync(putObjectArgs);
        }

        public async Task PutObjectAsync(string bucketName, string objectName, string fileName, string contentType = "application/octet-stream", Dictionary<string, string> metaData = null)
        {
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
            .WithObject(objectName)
                    .WithFileName(fileName)
                    .WithContentType(contentType);;
            await minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false); ;
        }

        public async Task<string> PresignedGetObjectAsync(string bucketName, string objectName, Dictionary<string, string> reqParams = null)
        {
            var args = new PresignedGetObjectArgs()
                .WithBucket(bucketName)
            .WithObject(objectName)
                   .WithExpiry(presignedGetObjectAsyncExpiresInt);
                   
            String url = await minioClient.PresignedGetObjectAsync(args);
            return url;
        }

        public async Task GetObjectAsync(string bucketName, string objectName, Action<Stream> callback)
        {
            GetObjectArgs getObjectArgs = new GetObjectArgs()
                                      .WithBucket(bucketName)
                                      .WithObject(objectName)
                                      .WithCallbackStream((stream) =>
                                      {
                                          callback(stream);
                                      });
            // Get input stream to have content of 'my-objectname' from 'my-bucketname'
            await minioClient.GetObjectAsync(getObjectArgs);
        }

        public async Task<Stream> GetObjectAsync(string bucketName, string objectName)
        {
            // Get input stream to have content of 'my-objectname' from 'my-bucketname'
           MemoryStream memory = new MemoryStream();
            Semaphore sem = new Semaphore(0, 1);
            GetObjectArgs getObjectArgs = new GetObjectArgs()
                                      .WithBucket(bucketName)
                                      .WithObject(objectName)
                                      .WithCallbackStream((stream) =>
                                      {
                                          stream.CopyTo(memory);
                                          sem.Release();
                                      });
            await minioClient.GetObjectAsync(getObjectArgs);
            sem.WaitOne();
            return memory;
        }
        /// <summary>
        /// 下载文件到本地
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task GetObjectAsync(String bucketName, String objectName, String fileName)
        {
            GetObjectArgs getObjectArgs = new GetObjectArgs()
                                     .WithBucket(bucketName)
                                     .WithObject(objectName) ;
            await minioClient.GetObjectAsync(getObjectArgs);

        }
        public async Task<byte[]> GetObjecByteArraytAsync(string bucketName, string objectName)
        {
            // Get input stream to have content of 'my-objectname' from 'my-bucketname'
            byte[] data = null;
            Semaphore sem = new Semaphore(1, 1);
            sem.WaitOne(5000);

            GetObjectArgs getObjectArgs = new GetObjectArgs()
                                     .WithBucket(bucketName)
                                     .WithObject(objectName)
                                     .WithCallbackStream((stream) =>
                                     {
                                         MemoryStream stmMemory = new MemoryStream();
                                         stream.CopyTo(stmMemory);
                                         data = stmMemory.GetBuffer();
                                         sem.Release();
                                     });
            await minioClient.GetObjectAsync(getObjectArgs);
            sem.WaitOne(5000);
           
            return data;
        }
        public async Task RemoveObjectAsync(string bucketName,string objectName)
        {
            RemoveObjectArgs rmArgs = new RemoveObjectArgs()
                                 .WithBucket(bucketName)
                                 .WithObject(objectName);
            await minioClient.RemoveObjectAsync(rmArgs);
        }
    }
}