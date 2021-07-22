using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Framework.Controllers;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreNote.Common.Utils;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using Morenote.Framework.Filter.Global;

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
        public async Task<IActionResult> UploadAttach(string noteId)
        {
           // var xxx= _accessor.HttpContext.Request.Form["noteId"];
            var id=noteId.ToLongByHex();
            long? fileId=SnowFlakeNet.GenerateSnowFlakeID();
            var resultMsg = "error"; // 错误信息
            var userId=GetUserIdBySession();
            var Ok = false;
         

            var re=new ResponseMessage(){ 
                Id= fileId.ToHex24(),
                Msg=resultMsg,
                Ok=Ok,
               
            };
            if (id==null)
            {
                re.Msg= "noteId is null";
                return Json(re,MyJsonConvert.GetOptions());

            }
            //todo: 判断是否有权限为笔记添加附件
           var note=noteService.GetNoteById(id);
            if (note==null||note.UserId!= userId)
            {

                return Json(re,MyJsonConvert.GetOptions());
            }
            var httpFiles = _accessor.HttpContext.Request.Form.Files;
            if (httpFiles == null || httpFiles.Count < 1)
            {

                return Json(re, MyJsonConvert.GetOptions());
            }
            var httpFile = httpFiles["file"];

            var uploadAttachMaxSizeByte = config.FileStoreConfig.UploadAttachMaxSizeMB * 1024 * 1024;

            if (httpFile.Length> uploadAttachMaxSizeByte)
            {
                resultMsg = $"The file's size is bigger than {config.FileStoreConfig.UploadAttachMaxSizeMB}M";

                 re.Msg=resultMsg;
                return Json(re, MyJsonConvert.GetOptions());
            }
            //上传到对象储存
            var fileName=httpFile.FileName;
           
            var nowTime = DateTime.Now;
            var ext = Path.GetExtension(fileName);
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[ext];
            var filesize=httpFile.Length;
             var  safeFileName = fileId.ToHex()+ ext;

            var objectName = $"{userId.ToHex()}/attachments/{ nowTime.ToString("yyyy")}/{nowTime.ToString("MM")}/{safeFileName}";
            bool result =await noteFileService.SaveFile(objectName, httpFile, memi);
            if (!result)
            {
                return Json(re,MyJsonConvert.GetOptions());
            }
            var fileInfo = new AttachInfo()
            {
                AttachId=fileId,
                UserId=userId,
               
                Name = safeFileName,
                Title=fileName,
                NoteId=id,
                UploadUserId=userId,
                Path=objectName,
                Type=ext,
                Size=filesize
            };
            re.Item=fileInfo;
            var message=string.Empty;
             result= attachService.AddAttach(fileInfo,false,out message);
            if (!result)
            {
                re.Msg=message;
                re.Ok=false;
                return Json(re,MyJsonConvert.GetOptions());
            }
            re.Msg = "success";
            re.Ok=true;
            return Json(re,MyJsonConvert.GetOptions());

        }

    }
}
