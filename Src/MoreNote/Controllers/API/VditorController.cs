using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Controllers.API.APIV1;
using MoreNote.Logic.Models.DTO.Vditor.Upload;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoreNote.Controllers.API
{
   // [Route("api/vditor/[action]")]
    public class VditorController : APIBaseController
    {
        private AuthService authService;
        private UserService userService;
        private TokenSerivce tokenSerivce;
        

        public VditorController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor, AuthService authService
            ) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.authService = authService;
            this.userService = userService;
            this.tokenSerivce = tokenSerivce;
        }
        [Route("api/vditor/upload/{token}")]
        public IActionResult upload(string token)
        {

            string msg = null;
            var data = UploadImagesOrAttach(out msg);
            UploadFileResponse uploadFileResponse = new UploadFileResponse()
            {
                data = data
            };
            return Json(uploadFileResponse, MyJsonConvert.GetSimpleOptions());
        }

        [Route("api/vditor/fetch/{token}")]
        public async Task<IActionResult> fetch([FromBody] FetchFileRequest fetchFileRequest,string token)
        {
            var user=tokenSerivce.GetUserByToken(token);
            if (user==null)
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Content("401");
            }


            string msg = string.Empty;

            //请求
            //var fetchFileRequest = JsonSerializer.Deserialize<FetchFileRequest>(json);
            //判断下载路径

            if (fetchFileRequest.url.StartsWith("/api"))
            {
                fetchFileRequest.url=config.APPConfig.SiteUrl+fetchFileRequest.url;
            }

            //下载文件 
            var fileModel = await DownLoadFile(fetchFileRequest.url);
           
          
            //保存到本地
            var resultURL = UploadImagesOrAttach(ref fileModel, out msg, user.UserId);

            if (string.IsNullOrEmpty(resultURL))
            {
                resultURL=fetchFileRequest.url;

            }
            //返回的消息体
            var fetchResponse = new FetchFileResponse()
            {
                msg=msg,
                data = new FetchData()
                {
                    originalURL = fetchFileRequest.url,
                    url = resultURL
                }
            };
            return Json(fetchResponse, MyJsonConvert.GetSimpleOptions());
        }
    }
}