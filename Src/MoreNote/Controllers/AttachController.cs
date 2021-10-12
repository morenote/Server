using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Helper;
using MoreNote.Common.Utils;
using MoreNote.Framework.Controllers;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.FileStoreService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Controllers
{
    public class AttachController : BaseController
    {
        private ConfigFileService configFileService;
        private NoteService noteService;

        public AttachController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , NoteService noteService
            , IHttpContextAccessor accessor) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.configFileService = configFileService;
            this.noteService = noteService;
        }

        [Route("Attach/UploadAttach")]
        //上传附件
        public async Task<IActionResult> UploadAttach(string noteId)
        {
            // var xxx= _accessor.HttpContext.Request.Form["noteId"];
            var id = noteId.ToLongByHex();
            long? fileId = SnowFlakeNet.GenerateSnowFlakeID();
            var resultMsg = "error"; // 错误信息
            var userId = GetUserIdBySession();
            var Ok = false;

            var re = new ResponseMessage()
            {
                Id = fileId.ToHex24(),
                Msg = resultMsg,
                Ok = Ok,
            };
            if (id == null)
            {
                re.Msg = "noteId is null";
                return Json(re, MyJsonConvert.GetOptions());
            }
            //todo: 判断是否有权限为笔记添加附件
            var note = noteService.GetNoteById(id);
            if (note == null || note.UserId != userId)
            {
                return Json(re, MyJsonConvert.GetOptions());
            }
            var httpFiles = _accessor.HttpContext.Request.Form.Files;
            if (httpFiles == null || httpFiles.Count < 1)
            {
                return Json(re, MyJsonConvert.GetOptions());
            }
            var httpFile = httpFiles["file"];

            var uploadAttachMaxSizeByte = config.FileStoreConfig.UploadAttachMaxSizeMB * 1024 * 1024;

            if (httpFile.Length > uploadAttachMaxSizeByte)
            {
                resultMsg = $"The file's size is bigger than {config.FileStoreConfig.UploadAttachMaxSizeMB}M";

                re.Msg = resultMsg;
                return Json(re, MyJsonConvert.GetOptions());
            }
            //上传到对象储存
            var fileName = httpFile.FileName;

            var nowTime = DateTime.Now;
            var ext = Path.GetExtension(fileName);
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[ext];
            var filesize = httpFile.Length;
            var safeFileName = fileId.ToHex() + ext;

            var objectName = $"{userId.ToHex()}/attachments/{ nowTime.ToString("yyyy")}/{nowTime.ToString("MM")}/{safeFileName}";
            bool result = await noteFileService.SaveFile(objectName, httpFile, memi);
            if (!result)
            {
                return Json(re, MyJsonConvert.GetOptions());
            }
            var fileInfo = new AttachInfo()
            {
                AttachId = fileId,
                UserId = userId,

                Name = safeFileName,
                Title = fileName,
                NoteId = id,
                UploadUserId = userId,
                Path = objectName,
                Type = ext,
                Size = filesize
            };
            re.Item = fileInfo;
            var message = string.Empty;
            result = attachService.AddAttach(fileInfo, false, out message);
            if (!result)
            {
                re.Msg = message;
                re.Ok = false;
                return Json(re, MyJsonConvert.GetOptions());
            }
            re.Msg = "success";
            re.Ok = true;
            return Json(re, MyJsonConvert.GetOptions());
        }

        //删除附件
        public async Task<IActionResult> DeleteAttach(string attachId)
        {
            var attachIdLong = attachId.ToLongByHex();
            var result = await attachService.DeleteAttachAsync(attachIdLong, GetUserIdBySession());
            var response = new ResponseMessage()
            {
                Ok = result,
                Msg = string.Empty
            };
            return Json(response, MyJsonConvert.GetOptions());
        }

        //获取某个笔记的附件列表
        public async Task<IActionResult> GetAttachs(string noteId)
        {
            var response = new ResponseMessage()
            {
                Ok = true,
                List = await attachService.ListAttachsAsync(noteId.ToLongByHex(), GetUserIdBySession())
            };
            return Json(response, MyJsonConvert.GetOptions());
        }

        //下载附件
        public async Task<IActionResult> Download(string attachId)
        {
            //todo:bug 要使用流式下载，减少下载时候的内存消耗
            var attach = await attachService.GetAttachAsync(attachId.ToLongByHex(), GetUserIdBySession());
            var provider = new FileExtensionContentTypeProvider();
            string fileExt = Path.GetExtension(attach.Name);
            var memi = provider.Mappings[fileExt];
            var fileService = FileStoreServiceFactory.Instance(config);
            var data = await fileService.GetObjecByteArraytAsync(config.MinIOConfig.NoteFileBucketName, attach.Path);

            return File(data, memi, attach.Title);
        }

        //下载全部的附件get all attachs by noteId
        public async Task<IActionResult> DownloadAll(string noteId)
        {
            var note = noteService.GetNoteById(noteId.ToLongByHex());
            if (note == null)
            {
                return Content("No found note,Please check the  noteId");
            }

            if (GetUserIdBySession() != note.UserId)
            {
                return Content("No permission to access attachments");
            }
            // 得到文件列表
            var attachs = await attachService.ListAttachsAsync(noteId.ToLongByHex(), GetUserIdBySession());
            if (attachs.IsNullOrNothing())
            {
                return Content("");
            }
            var sb = new StringBuilder();
            var fileStore = FileStoreServiceFactory.Instance(config);
            //下载拼接
            foreach (var attach in attachs)
            {
                sb.Append(attach.AttachId);
            }
            //计算AttachId合并字符串的哈希
            string md5 = SHAEncryptHelper.MD5Encrypt(sb.ToString());
            var dir = config.FileStoreConfig.TempFolder + Path.DirectorySeparatorChar + note.NoteId.ToHex();
            var zipFileName = config.FileStoreConfig.TempFolder + Path.DirectorySeparatorChar + md5 + ".zip";
            if (!System.IO.File.Exists(zipFileName))
            {
                //清理文件夹
                if (Directory.Exists(dir))
                {
                    Directory.Delete(dir, true);
                }

                Directory.CreateDirectory(dir);

                //下载附件到本地
                foreach (var attach in attachs)
                {
                    string fileName = TextFilterUtil.DelUnSafeChar(attach.Title);
                    await fileStore.GetObjectAsync(config.MinIOConfig.NoteFileBucketName, attach.Path, dir + Path.DirectorySeparatorChar + fileName);
                }
                //执行压缩
                ZipHelper compressedFilesHelper = new ZipHelper();
                compressedFilesHelper.CreateZipFile(zipFileName, null, dir);
            }
            var memi = GetMemi(".zip");

            var stream = System.IO.File.Open(zipFileName, FileMode.Open, FileAccess.Read);

            {
                return File(stream, memi, Path.GetFileName(zipFileName));
            }
        }
    }
}