using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UpYunLibrary;

namespace MoreNote.Logic.Service
{
     class FileManageService
    {
        const string DEFAULT_ALBUM_ID = "52d3e8ac99c37b7f0d000001";
        public static async Task<bool> SaveUploadFileOnUPYunAsync(UpYun upyun, IFormFile formFile, string uploadDirPath, string fileName)
        {
            if (formFile.Length > 0)
            {
                // string fileExt = Path.GetExtension(formFile.FileName); //文件扩展名，不含“.”
                // long fileSize = formFile.Length; //获得文件大小，以字节为单位

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
        public static async Task<bool> SaveUploadFileOnDiskAsync(IFormFile formFile, string uploadDirPath, string fileName)
        {
            if (formFile.Length > 0)
            {
                // string fileExt = Path.GetExtension(formFile.FileName); //文件扩展名，不含“.”
                // long fileSize = formFile.Length; //获得文件大小，以字节为单位

                if (!Directory.Exists(uploadDirPath))
                {
                    Directory.CreateDirectory(uploadDirPath);
                }
                var filePath = uploadDirPath + fileName;
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

        public static bool AddImage(NoteFile image,long albumId,long userId,bool needCheck)
        {
            image.CreatedTime=DateTime.Now;
            if (albumId!=0)
            {
                image.AlbumId=albumId;
            }
            else
            {
                image.AlbumId=1;
                image.IsDefaultAlbum=true;
            }
            image.UserId=userId;
            return AddFile(image);
            throw new Exception();
        }
        private static bool AddFile(NoteFile noteFile)
        {
            using (var db = DataContext.getDataContext())
            {
                db.File.Add(noteFile);
                return db.SaveChanges()>0;

            }
        }
        // list images
        // if albumId == "" get default album images
        public static Page ListImagesWithPage(long userId,long albunId,string key,int pageNumber,int pageSize)
        {
            throw new Exception();
        }
        // get all images names
        // for upgrade
        public static Dictionary<string,bool> GetAllImageNamesMap(long userId)
        {
            throw new Exception();
        }
        // delete image
        public static bool DeleteImage(long userId,long fileId)
        {
            throw new Exception();
        }
        // update image title
        public static bool UpdateImage(long userId,long fileId,string title)
        {
            throw new Exception();
        }
        public static string GetFileBase64(string userid,string fileId)
        {
            throw  new Exception();
        }
        // 得到图片base64, 图片要在之前添加data:image/png;base64,
        public static string GetImageBase64(long userId,long fileId)
        {
            throw 
                 new Exception();
        }
        // 获取文件路径
        // 要判断是否具有权限
        // userId是否具有fileId的访问权限
        public static string GetFile(long userId,string fileId)
        {
            throw new Exception();
        }
        public static NoteFile GetFile(long fileId)
        {
            using (var db=new DataContext())
            {
               var result= db.File.Where(b=>b.FileId==fileId);
               var file=(result==null?null:result.FirstOrDefault());
               return file;

            }
        }
        // 复制共享的笔记时, 复制其中的图片到我本地
        // 复制图片
        public static bool CopyImage(long userId,long fileId,long toUserId)
        {
            throw new Exception();
        }
        // 是否是我的文件
        public static bool IsMyFile(long userId,long fileId)
        {
            throw  new Exception();
        }

    }
}
