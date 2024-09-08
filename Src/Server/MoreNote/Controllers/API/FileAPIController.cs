using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Config.ConfigFile;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.FileStoreService;
using MoreNote.Logic.Service.Notes;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace MoreNote.Controllers.API.APIV1
{
    //[ApiController]
    public class FileAPIController : APIBaseController
	{
		public NoteService noteService;
		public WebSiteConfig webSiteConfig;

		public FileAPIController(AttachService attachService
			 , TokenSerivce tokenSerivce
			 , NoteFileService noteFileService
			 , UserService userService
			 , ConfigFileService configFileService
			 , IHttpContextAccessor accessor,
			NoteService noteService
			) :
			base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
		{
			this.noteService = noteService;
			this.webSiteConfig = configFileService.ReadConfig();
		}


		/// <summary>
		/// 获取用户头像
		/// </summary>
		/// <param name="userIdHex"></param>
		/// <param name="filename"></param>
		/// <returns></returns>
		[Route("api/File/Avatars/{userIdHex}/{filename}")]
		[HttpGet]
		[ResponseCache(Duration = 600, VaryByQueryKeys = new[] { "userIdHex", "filename" })]
		public async Task<IActionResult> GetAvatar(string userIdHex, string filename)
		{
			var fileUrlPath = $"{userIdHex}/images/logo";
			var objectName = $"{fileUrlPath}/{filename}";
			var provider = new FileExtensionContentTypeProvider();

			string fileExt = Path.GetExtension(filename);
			var memi = provider.Mappings[fileExt];
			var fileService = FileStoreServiceFactory.Instance(webSiteConfig);
			try
			{
				var data = await fileService.GetObjecByteArraytAsync(webSiteConfig.MinIOConfig.NoteFileBucketName, objectName);
				return File(data, memi);
			}
			catch (Exception ex)
			{
				Response.StatusCode = (int)HttpStatusCode.NotFound;
				return Content("NotFound");
			}
		}


		//经过格式化的URL,有助于CDN或者反向代码服务器缓存图片
		//api/File/GetImageForWeb/xxxxx   xxxx=xxx.jpg
		[Route("CacheServer/File/Images/{fileId}")]
		[HttpGet]
		[ResponseCache(Duration = 600, VaryByQueryKeys = new[] { "fileId" })]
		public async Task<IActionResult> GetImageForWeb(string fileId)
		{

			if (string.IsNullOrEmpty(fileId))
			{
				return Content("error");
			}
			var myFileId = fileId.ToLongByHex();
			var noteFile = noteFileService.GetFile(myFileId);
			if (noteFile == null)
				//return Content("NoFoundImage");
				return NoFoundImage();
			//获取操作对象
			string fileExt = Path.GetExtension(noteFile.Name);
			var fileService = FileStoreServiceFactory.Instance(webSiteConfig);

			var objectName = $"{noteFile.UserId.ToHex()}/images/{noteFile.CreatedTime.ToString("yyyy")}/{noteFile.CreatedTime.ToString("MM")}/{noteFile.Id.ToHex()}{Path.GetExtension(noteFile.Name)}";
			var provider = new FileExtensionContentTypeProvider();
			var memi = provider.Mappings[fileExt];
			var data = await fileService.GetObjecByteArraytAsync(webSiteConfig.MinIOConfig.NoteFileBucketName, objectName);

			return File(data, memi);
		}
		/// <summary>
		/// 下载图片
		/// </summary>
		/// <param name="fileId">文件id</param>
		/// <param name="key">访问密钥</param>
		/// <returns></returns>
		//todo: 输出image 需要get参数
		//api/File/GetImage?fileId=xxxx
		[Route("api/File/GetImage")]
		[Route("api/File/Images/{fileId}/{key}")]
		[Route("api/File/Images/{fileId}")]
		[HttpGet]
		[ResponseCache(Duration = 600, VaryByQueryKeys = new[] { "fileId", "isRedirect", "key" })]
		public async Task<IActionResult> GetImage(string fileId, string key)
		{

			if (string.IsNullOrEmpty(fileId))
			{
				return Content("error");
			}
			var myFileId = fileId.ToLongByHex();
			var noteFile = noteFileService.GetFile(myFileId);
			if (noteFile == null)
				//return Content("NoFoundImage");
				return NoFoundImage();
			//获取操作对象
			string fileExt = Path.GetExtension(noteFile.Name);
			var fileService = FileStoreServiceFactory.Instance(webSiteConfig);

			var objectName = $"{noteFile.UserId.ToHex()}/images/{noteFile.CreatedTime.ToString("yyyy")}/{noteFile.CreatedTime.ToString("MM")}/{noteFile.Id.ToHex()}{Path.GetExtension(noteFile.Name)}";
			var provider = new FileExtensionContentTypeProvider();
			var memi = provider.Mappings[fileExt];
			var data = await fileService.GetObjecByteArraytAsync(webSiteConfig.MinIOConfig.NoteFileBucketName, objectName);

			return File(data, memi);
		}
		[ResponseCache(Duration = 600)]
		public IActionResult NoFoundImage()
		{
			return Redirect($"/404.jpg");
		}

		[Route("api/File/GetAttach")]
		[HttpGet]
		[ResponseCache(Duration = 600, VaryByQueryKeys = new[] { "fileId" })]
		public async Task<IActionResult> GetAttach(string fileId)
		{
			//todo:bug 要使用流式下载，减少下载时候的内存消耗
			var attach = await attachService.GetAttachAsync(fileId.ToLongByHex(), GetUserIdBySession());

			var fileService = FileStoreServiceFactory.Instance(config);
			var data = await fileService.GetObjecByteArraytAsync(config.MinIOConfig.NoteFileBucketName, attach.Path);
			var provider = new FileExtensionContentTypeProvider();
			string fileExt = Path.GetExtension(attach.Name);
			var memi = provider.Mappings[fileExt];
			return File(data, memi, attach.Title);
		}

		//todo:下载所有附件
		[HttpGet]
		[ResponseCache(Duration = 600, VaryByQueryKeys = new[] { "noteId", "token" })]
		public IActionResult GetAllAttachs(string noteId, string token)
		{
			return null;
		}
	}
}