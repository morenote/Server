﻿using Minio;
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
   

        private int presignedGetObjectAsyncExpiresInt;

        public MinIOFileStoreService(WebSiteConfig siteConfig)
        {
         
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
        public MinIOFileStoreService( string Endpoint, string MINIO_ACCESS_KEY,string MINIO_SECRET_KEY, bool WithSSL,int presignedGetObjectAsyncExpiresInt)
        {
            
            this.presignedGetObjectAsyncExpiresInt = presignedGetObjectAsyncExpiresInt;
            minioClient = new MinioClient(Endpoint, MINIO_ACCESS_KEY, MINIO_SECRET_KEY);
            if (WithSSL)
            {
                minioClient.WithSSL();
            }
        }


        public async Task PutObjectAsync(string bucketName, string objectName, Stream data, long size, string contentType, Dictionary<string, string> metaData = null)
        {
            await minioClient.PutObjectAsync(bucketName, objectName, data, size, contentType, metaData);
        }

        public async Task PutObjectAsync(string bucketName, string objectName, string fileName, string contentType = "application/octet-stream", Dictionary<string, string> metaData = null)
        {
            await minioClient.PutObjectAsync(bucketName, objectName, fileName, contentType, metaData);
        }

        public async Task<string> PresignedGetObjectAsync(string bucketName, string objectName, Dictionary<string, string> reqParams = null)
        {
            String url = await minioClient.PresignedGetObjectAsync(bucketName, objectName, presignedGetObjectAsyncExpiresInt);
            return url;
        }

        public async Task GetObjectAsync(string bucketName, string objectName, Action<Stream> callback)
        {
            // Get input stream to have content of 'my-objectname' from 'my-bucketname'
            await minioClient.GetObjectAsync(bucketName, objectName, callback);
        }

        public async Task<Stream> GetObjectAsync(string bucketName, string objectName)
        {
            // Get input stream to have content of 'my-objectname' from 'my-bucketname'
           MemoryStream memory = new MemoryStream();
            Semaphore sem = new Semaphore(0, 1);
           
            await minioClient.GetObjectAsync(bucketName,objectName,  (callStream) =>
                {
                    

                      callStream.CopyTo(memory);
                    sem.Release();
                });
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
            await minioClient.GetObjectAsync(bucketName,objectName,fileName);

        }
        public async Task<byte[]> GetObjecByteArraytAsync(string bucketName, string objectName)
        {
            // Get input stream to have content of 'my-objectname' from 'my-bucketname'
            byte[] data = null;
            Semaphore sem = new Semaphore(1, 1);
            sem.WaitOne(5000);
            await minioClient.GetObjectAsync(bucketName,

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
        public async Task RemoveObjectAsync(string bucketName,string objectName)
        {
           await minioClient.RemoveObjectAsync(bucketName,objectName);
        }
    }
}