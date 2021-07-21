using Microsoft.AspNetCore.Http;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.Service.FileStoreService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UpYunLibrary;

namespace MoreNote.Logic.Service
{
    /// <summary>
    /// 又拍云对象储存文件服务
    /// 是对于FileServier的实现
    /// </summary>
    public class NoteFileService
    {
        private const string DEFAULT_ALBUM_ID = "52d3e8ac99c37b7f0d000001";

        private DataContext dataContext;
        ConfigFileService ConfigFileService;
        FileStoreConfig storeConfig;
        WebSiteConfig webSiteConfig;
        public NoteFileService(DataContext dataContext, ConfigFileService ConfigFileService)
        {
            this.dataContext = dataContext;
            this.ConfigFileService=ConfigFileService;
            this.storeConfig= ConfigFileService.WebConfig.FileStoreConfig;
            this.webSiteConfig= ConfigFileService.WebConfig;
        }

        public async Task<bool> SaveFile(string objectName, IFormFile formFile,string contentType)
        {
            try
            {
                var fileStore = FileStoreServiceFactory.Instance(webSiteConfig);
                using (Stream stream = formFile.OpenReadStream())
                {
                   await fileStore.PutObjectAsync(webSiteConfig.MinIOConfig.NoteFileBucketName, objectName, stream,stream.Length, contentType);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
               
            }
           
        }

        public async Task<bool> SaveUploadFileOnUPYunAsync(UpYun upyun, IFormFile formFile, string uploadDirPath, string fileName)
        {
            if (formFile.Length > 0)
            {
                // string fileExt = Path.GetExtension(formFile.FileName); //文件扩展名，不含“.”
                // long? fileSize = formFile.Length; //获得文件大小，以字节为单位

                if (!Directory.Exists(uploadDirPath))
                {
                    Directory.CreateDirectory(uploadDirPath);
                }
                var filePath = uploadDirPath + fileName;
                MemoryStream stmMemory = new MemoryStream();
                await formFile.CopyToAsync(stmMemory).ConfigureAwait(false);
                byte[] imageBytes = stmMemory.ToArray();
                return upyun.writeFile($"{uploadDirPath}{fileName}", imageBytes, true);
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SaveUploadFileOnDiskAsync(IFormFile formFile, string saveUploadDirPath, string fileName)
        {

           

            if (formFile.Length > 0)
            {
                // string fileExt = Path.GetExtension(formFile.FileName); //文件扩展名，不含“.”
                // long? fileSize = formFile.Length; //获得文件大小，以字节为单位

                if (!Directory.Exists(saveUploadDirPath))
                {
                    Directory.CreateDirectory(saveUploadDirPath);
                }
                var filePath = saveUploadDirPath + fileName;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream).ConfigureAwait(false);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        // add Image

        public bool AddImage(NoteFile image, long? albumId, long? userId, bool needCheck)
        {
            image.CreatedTime = DateTime.Now;
            if (albumId != 0)
            {
                image.AlbumId = albumId;
            }
            else
            {
                image.AlbumId = 1;
                image.IsDefaultAlbum = true;
            }
            image.UserId = userId;
            return AddFile(image);
            throw new Exception();
        }

        private bool AddFile(NoteFile noteFile)
        {
            dataContext.NoteFile.Add(noteFile);
            return dataContext.SaveChanges() > 0;
        }

        // list images
        // if albumId == "" get default album images
        public Page ListImagesWithPage(long? userId, long? albunId, string key, int pageNumber, int pageSize)
        {
            throw new Exception();
        }

        // get all images names
        // for upgrade
        public Dictionary<string, bool> GetAllImageNamesMap(long? userId)
        {
            throw new Exception();
        }

        // delete image
        public bool DeleteImage(long? userId, long? fileId)
        {
            throw new Exception();
        }

        // update image title
        public bool UpdateImage(long? userId, long? fileId, string title)
        {
            throw new Exception();
        }

        public string GetFileBase64(string userid, string fileId)
        {
            throw new Exception();
        }

        // 得到图片base64, 图片要在之前添加data:image/png;base64,
        public string GetImageBase64(long? userId, long? fileId)
        {
            throw
                 new Exception();
        }

        // 获取文件路径
        // 要判断是否具有权限
        // userId是否具有fileId的访问权限
        public string GetFile(long? userId, string fileId)
        {
            throw new Exception();
        }

        public NoteFile GetFile(long? fileId)
        {
            
                var result = dataContext.NoteFile.Where(b => b.FileId == fileId);
                var file = (result == null ? null : result.FirstOrDefault());
                return file;
            
        }

        // 复制共享的笔记时, 复制其中的图片到我本地
        // 复制图片
        public bool CopyImage(long? userId, long? fileId, long? toUserId)
        {
            throw new Exception();
        }

        // 是否是我的文件
        public bool IsMyFile(long? userId, long? fileId)
        {
            throw new Exception();
        }
    }
}