using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreNote.Logic.Service;
using MoreNote.Framework.Controllers;

namespace MoreNote.Controllers
{
    public class IOController : BaseController
    {
        public IOController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
           

        }

        //Post表单上传
        public async Task<IActionResult> FileFormSave(List<IFormFile> files)

        {

            // var files = Request.Form.Files;

            long? size = files.Sum(f => f.Length);

            //string webRootPath = _hostingEnvironment.WebRootPath;

            // string contentRootPath = _hostingEnvironment.ContentRootPath;

            foreach (var formFile in files)

            {

                if (formFile.Length > 0)

                {
                    string fileExt = Path.GetExtension(formFile.FileName); //文件扩展名，不含“.”

                    long? fileSize = formFile.Length; //获得文件大小，以字节为单位

                    string newFileName = System.Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名

                    if (!Directory.Exists("upload"))
                    {
                        Directory.CreateDirectory("upload");
                    }
                    var filePath = "upload/" + newFileName;

                    using (var stream = new FileStream(filePath, FileMode.Create))

                    {
                        await formFile.CopyToAsync(stream);

                    }

                }

            }



            return Ok(new { count = files.Count, size });

        }

        //AJAX上传
        public async Task<IActionResult> FileAJAXSave()
        {
            var date = Request;
            var files = Request.Form.Files;
            long? size = files.Sum(f => f.Length);
            //string webRootPath = _hostingEnvironment.WebRootPath;
            //string contentRootPath = _hostingEnvironment.ContentRootPath;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    string fileExt = Path.GetExtension(formFile.FileName); //文件扩展名，不含“.”
                    long? fileSize = formFile.Length; //获得文件大小，以字节为单位
                    string newFileName = System.Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名
                    var filePath = "upload/" + newFileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {

                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return Ok(new { count = files.Count, size });
        }


        //下载
        public IActionResult DownLoad(string fileFullName)
        {
            var addrUrl = fileFullName;
            var stream = System.IO.File.OpenRead(addrUrl);
            string fileExt = Path.GetExtension(fileFullName);
            //获取文件的ContentType
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[fileExt];
            return File(stream, memi, Path.GetFileName(addrUrl));
        }
       // [HttpPost]
        private IActionResult UploadFiles(List<IFormFile> files)
        {
            long? size = 0;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var file in files)
            {
                //var fileName = file.FileName;
                var fileName = ContentDispositionHeaderValue
                    .Parse(file.ContentDisposition)
                    .Name;
                //.Trim('"');
                var fileDir = Path.Combine("_hostingEnv.WebRootPath", "UploadFiles");
                if (!Directory.Exists(fileDir))
                {
                    Directory.CreateDirectory(fileDir);
                }
                string filePath = fileDir + $@"\{fileName}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                stringBuilder.AppendLine($"文件\"{fileName}\" /{size}字节上传成功 <br/>");
            }
            stringBuilder.AppendLine($"共{files.Count}个文件 /{size}字节上传成功! <br/>");
            ViewBag.Message = stringBuilder.ToString();
            return View();
        }
        /// <summary>
        /// AJAX请求上传，通过Request.Form.Files获取上传文件信息
        /// </summary>
        /// <returns></returns>
       // [HttpPost]
        private JsonResult AjaxUploadFiles()
        {
            long? size = 0;
            var files = Request.Form.Files;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var file in files)
            {
                //var fileName = file.FileName;
                var fileName = ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .Name
                                .Trim();
                var fileDir = Path.Combine("", "UploadFiles");
                if (!Directory.Exists(fileDir))
                {
                    Directory.CreateDirectory(fileDir);
                }
                string filePath = fileDir + $@"\{fileName}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                stringBuilder.AppendLine($"文件\"{fileName}\" /{size}字节上传成功 <br/>");
            }
            stringBuilder.AppendLine($"共{files.Count}个文件 /{size}字节上传成功! <br/>");
            ViewBag.Message = stringBuilder.ToString();
            return Json(new
            {
                msg = stringBuilder.ToString()
            });
        }


        public async Task<IActionResult> EditorImageUploadService(List<IFormFile> files)
        {
            return null;
           /**
                string newAvatar = string.Empty;
                long? size = files.Sum(f => f.Length);
                var uploadResult = new EditorImageUploadResult();
                foreach (var formFile in files)

                {

                    if (formFile.Length > 0)

                    {


                        string fileExt = Path.GetExtension(formFile.FileName); //文件扩展名，不含“.”
                                                                               //  string fileName = System.Guid.NewGuid().ToString("N");

                        long? fileSize = formFile.Length; //获得文件大小，以字节为单位

                        //string newFileName = System.Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名
                        //string newFileName = "123" + "." + fileExt; //随机生成新的文件名
                        //string newFileName = fileName+fileExt; //随机生成新的文件名

                        if (!Directory.Exists("wwwroot/editImages"))
                        {
                            Directory.CreateDirectory("wwwroot/editImages");
                        }

                        string sha1FileName = string.Empty;
                        byte[] hashBytes = HashData(formFile.OpenReadStream(), "SHA1");
                        string SHA1 = ByteArrayToHexString(hashBytes);
                        sha1FileName = SHA1 + fileExt;
                        if (System.IO.File.Exists("wwwroot/editImages/" + sha1FileName))
                        {

                            //Do nothing 

                        }
                        else
                        {
                            using (var stream = new FileStream("wwwroot/editImages/" + sha1FileName, FileMode.GenerateImage))
                            {

                                await formFile.CopyToAsync(stream);
                                // System.IO.FileStream fs = new System.IO.FileStream ( fileName , System.IO.FileMode.Open , System.IO.FileAccess.Read );

                            }

                        }

                        uploadResult.data.Add($"/editImages/{sha1FileName}");

                    }


                }

                uploadResult.errno = 0;

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(uploadResult);
                return Content(json);
                // return RedirectToAction("User");
                // return Ok(new { count = files.Count, size });

          



    **/

        }
        /// <summary>
        /// 计算哈希值
        /// </summary>
        /// <param name="stream">要计算哈希值的 Stream</param>
        /// <param name="algName">算法:SHA1,MD5</param>
        /// <returns>哈希值字节数组</returns>
        private byte[] HashData(System.IO.Stream stream, string algName)
        {
            System.Security.Cryptography.HashAlgorithm algorithm;
            if (algName == null)
            {
                throw new ArgumentNullException("algName 不能为 null");
            }
            if (string.Compare(algName, "SHA1", true) == 0)
            {
                algorithm = System.Security.Cryptography.SHA1.Create();
            }
            else
            {
                if (string.Compare(algName, "MD5", true) != 0)
                {
                    throw new Exception("algName 只能使用 SHA1 或 MD5");
                }
                algorithm = System.Security.Cryptography.MD5.Create();
            }
            return algorithm.ComputeHash(stream);
        }
        /// <summary>
        /// 字节数组转换为16进制表示的字符串
        /// </summary>
        private string ByteArrayToHexString(byte[] buf)
        {
            return BitConverter.ToString(buf).Replace("-", "");
        }
    }
}