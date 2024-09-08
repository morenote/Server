using Autofac;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Config.ConfigFile;
using MoreNote.CryptographyProvider;
using MoreNote.Language;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Models.DTO.Vditor.Upload;
using MoreNote.AutoFac.Property;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.DistributedIDGenerator;
using MoreNote.Logic.Service.Logging;
using MoreNote.Models.Entity.Leanote;
using MoreNote.Models.Entity.Leanote.Notes;
using MoreNote.Models.Entity.Leanote.User;
using MoreNote.Value;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using MoreNote.Logic.Service.Notes;
using MoreNote.Models.Entiys.Leanote.Notes;

namespace MoreNote.Framework.Controllers
{
    /**
     * 源代码基本是从GO代码直接复制过来的
     *
     * 只是简单的实现了API的功能
     *
     * 2020年01月27日
     * */

    public abstract class BaseController : Controller
	{
		public AttachService attachService;

		/// <summary>
		/// 网站配置
		/// </summary>
		public WebSiteConfig config;

		public string defaultSortField = "UpdatedTime";
		public string leanoteUserId = "admin";
		public NoteFileService noteFileService;

		/// <summary>
		/// 默认1000
		/// </summary>
		public int pageSize = 1000;

		public TokenSerivce tokenSerivce;

		public UserService userService;

		// 不能更改
		protected IHttpContextAccessor _accessor;

		protected ConfigFileService configFileService;



		[Autowired]
		protected IDistributedIdGenerator idGenerator { get; set; }

		[Autowired]
		protected IDistributedCache distributedCache { get; set; }//分布式缓存

		[Autowired]
		protected ILoggingService log { get; set; }

		[Autowired]
		protected ICryptographyProvider cryptographyProvider { get; set; }
		[Autowired]
		protected IComponentContext componentContext { get; set; }
		[Autowired]
		protected SubmitTreeService SubmitTreeService { get; set; }

		
		public BaseController(AttachService attachService
			, TokenSerivce tokenSerivce
			, NoteFileService noteFileService
			, UserService userService
			, ConfigFileService configFileService
			, IHttpContextAccessor accessor

			)
		{

			this.attachService = attachService;
			this.tokenSerivce = tokenSerivce;
			this.noteFileService = noteFileService;
			this.configFileService = configFileService;
			this.userService = userService;
			this._accessor = accessor;

			config = configFileService.ReadConfig();

		}
		[ApiExplorerSettings(IgnoreApi = true)]
		protected string GetAntiCSRFToken()
		{
			var token = HttpContext.Session.GetString("AntiCSRFToken");
			if (string.IsNullOrEmpty(token))
			{
				token = RandomTool.CreatSafeRandomHex(16);
				HttpContext.Session.SetString("AntiCSRFToken", token);
			}
			return token;
		}
		/// <summary>
		/// 检查验证码
		/// </summary>
		/// <returns></returns>
		[ApiExplorerSettings(IgnoreApi = true)]
		protected bool CheckVerifyCode(string captcha, out string message)
		{
			string verifyCode = HttpContext.Session.GetString("VerifyCode");
			int time = HttpContext.Session.GetInt32("VerifyCodeTime").GetValueOrDefault(0);
			int valid = HttpContext.Session.GetInt32("VerifyCodeValid").GetValueOrDefault(0);
			if (valid != 1 || !UnixTimeUtil.IsValid(time, 60))//验证码的保质期是60秒
			{
				message = "验证码过期或失效";
				return false;
			}
			//销毁验证码的标志
			HttpContext.Session.SetInt32("VerifyCodeValid", 0);
			if (string.IsNullOrEmpty(verifyCode) || string.IsNullOrEmpty(captcha))
			{
				message = "错误参数";
				return false;
			}
			else
			{
				if (captcha.Equals("0") || !captcha.ToLower().Equals(verifyCode))
				{
					message = "验证码错误";
					return false;
				}
			}
			message = "";
			return true;
		}

		protected enum FileTyte
		{
			/**
             * 文件分类
             * 视频 音频 图片 二进制 纯文本
             * */
			Video, Audio, Image, Binary, PlainText
		}
		[ApiExplorerSettings(IgnoreApi = true)]
		protected IActionResult Action()
		{
			return Content("error");
		}
		[ApiExplorerSettings(IgnoreApi = true)]
		protected long? ConvertUserIdToLong()
		{
			string hex = _accessor.HttpContext.Request.Form["userId"];
			if (string.IsNullOrEmpty(hex))
			{
				hex = _accessor.HttpContext.Request.Query["userId"];
			}
			if (string.IsNullOrEmpty(hex))
			{
				return 0;
			}
			return hex.ToLongByHex();
		}

		/// <summary>
		///  得到第几页
		/// </summary>
		/// <returns></returns>
		[ApiExplorerSettings(IgnoreApi = true)]
		protected int GetPage()
		{
			var pageValue = Request.Query["page"];
			if (StringValues.IsNullOrEmpty(pageValue))
			{
				return 1;
			}
			int value = 0;
			Int32.TryParse(pageValue, out value);
			return value;
		}

		/// <summary>
		/// 通过HttpContext获得token
		/// todo:得到token, 这个token是在AuthInterceptor设到Session中的
		/// </summary>
		/// <returns></returns>
		[ApiExplorerSettings(IgnoreApi = true)]
		protected string GetTokenByHttpContext()
		{
			/**
             *  软件从不假设某个IP或者使用者借助cookie获得永久的使用权
             *  任何访问，必须显式的提供token证明
             *
             *  API服务不接受cookie中的信息，token总是显式提交的
             *
             **/
			string token = null;
			if (_accessor.HttpContext.Request.Form != null)
			{
				token = _accessor.HttpContext.Request.Form["token"];
			}

			if (string.IsNullOrEmpty(token))
			{
				token = _accessor.HttpContext.Request.Query["token"];
			}
			if (string.IsNullOrEmpty(token))
			{
				return "";
			}
			else
			{
				return token;
			}
		}
		[ApiExplorerSettings(IgnoreApi = true)]
		protected UserInfo SetUserInfo()
		{
			var userInfo = this.GetUserBySession();
			ViewBag.userInfo = userInfo;
			//todo:关于配置逻辑
			if (userInfo != null)
			{
				ViewBag.isAdmin = userInfo.IsAdmin();
				ViewBag.IsSuperAdmin = userInfo.IsSuperAdmin();

			}
			else
			{
				ViewBag.isAdmin = false;
				ViewBag.IsSuperAdmin = false;
			}


			return userInfo;
		}
		[ApiExplorerSettings(IgnoreApi = true)]
		protected UserInfo GetUserBySession()
		{
			string userid_hex = _accessor.HttpContext.Session.GetString("UserId");
			long? userid_number = userid_hex.ToLongByHex();

			UserInfo user = userService.GetUserByUserId(userid_number);
			return user;
		}
		[ApiExplorerSettings(IgnoreApi = true)]
		protected UserInfo GetUserByToken(string token)
		{
			if (string.IsNullOrEmpty(token))
			{
				return null;
			}
			else
			{
				UserInfo user = tokenSerivce.GetUserByToken(token);
				return user;
			}
		}
		[ApiExplorerSettings(IgnoreApi = true)]
		public UserInfo GetUserByToken()
		{
			string token = GetTokenByHttpContext();
			if (string.IsNullOrEmpty(token))
			{
				return null;
			}
			else
			{
				UserInfo user = tokenSerivce.GetUserByToken(token);
				return user;
			}
		}
		[ApiExplorerSettings(IgnoreApi = true)]
		protected long? GetUserIdBySession()
		{
			string userid_hex = _accessor.HttpContext.Session.GetString("UserId");
			long? userid_number = userid_hex.ToLongByHex();
			return userid_number;
		}
		[ApiExplorerSettings(IgnoreApi = true)]
		protected void UpdateSession(string key, string value)
		{
			_accessor.HttpContext.Session.SetString(key, value);
		}
		[ApiExplorerSettings(IgnoreApi = true)]
		// todo:得到用户信息
		protected long? GetUserIdByToken(string token)
		{
			if (string.IsNullOrEmpty(token))
			{
				return 0;
			}
			else
			{
				UserInfo user = tokenSerivce.GetUserByToken(token);
				long? userid = (user == null ? 0 : user.Id);
				return userid;
			}
		}
		[ApiExplorerSettings(IgnoreApi = true)]
		protected long? GetUserIdByToken()
		{
			string token = GetTokenByHttpContext();
			if (string.IsNullOrEmpty(token))
			{
				string userid_hex = _accessor.HttpContext.Session.GetString("UserId");
				long? userid_number = userid_hex.ToLongByHex();
				return userid_number;
			}
			else
			{
				UserInfo user = tokenSerivce.GetUserByToken(token);
				long? userid = (user == null ? 0 : user.Id);
				return userid;
			}
		}
		[ApiExplorerSettings(IgnoreApi = true)]
		protected UserInfo GetUserAndBlogUrl(Notebook repository)
		{
			var userid = GetUserIdBySession();
			if (userid == null)
			{
				return new UserInfo();
			}
			else
			{
				return userService.GetUserAndBlogUrl(userid, repository);
			}
		}
		[ApiExplorerSettings(IgnoreApi = true)]
		protected bool HasLogined()
		{
			string userHex = HttpContext.Session.GetString("UserId");
			if (string.IsNullOrEmpty(userHex))
			{
				//没登陆
				return false;
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// 获得国际化语言资源文件
		/// </summary>
		/// <returns></returns>
		[ApiExplorerSettings(IgnoreApi = true)]
		protected LanguageResource GetLanguageResource()
		{
			var lnag = "zh-cn";

			var locale = Request.Cookies["LEANOTE_LANG"];

			if (string.IsNullOrEmpty(locale))
			{
				locale = lnag;
			}
			var languageResource = LanguageFactory.GetLanguageResource(locale);
			return languageResource;
		}

		/// <summary>
		/// 设置区域性信息
		/// </summary>
		/// <returns></returns>
		[ApiExplorerSettings(IgnoreApi = true)]
		protected string SetLocale()
		{
			//todo:SetLocale未完成

			var lnag = "zh-cn";

			var locale = Request.Cookies["LEANOTE_LANG"];

			if (string.IsNullOrEmpty(locale))
			{
				locale = lnag;
			}

			var languageResource = LanguageFactory.GetLanguageResource(locale);

			ViewBag.msg = languageResource.GetMsg();

			ViewBag.member = languageResource.GetMember();
			ViewBag.markdown = languageResource.GetMarkdown();
			ViewBag.blog = languageResource.GetBlog();
			ViewBag.noteconf = languageResource.GetNote();
			ViewBag.tinymce_editor = languageResource.GetTinymce_editor();
			ViewBag.demonstrationOnly = configFileService.ReadConfig().GlobalConfig.DemonstrationOnly;

			ViewBag.locale = locale;
			ViewBag.siteUrl = config.APPConfig.SiteUrl;
			ViewBag.blogUrl = config.APPConfig.BlogUrl;
			ViewBag.leaUrl = config.APPConfig.LeaUrl;
			ViewBag.noteUrl = config.APPConfig.NoteUrl;

			return null;
		}





		[ApiExplorerSettings(IgnoreApi = true)]
		protected void SetUserIdToSession(long? userId)
		{
			_accessor.HttpContext.Session.SetString("UserId", userId.ToHex());
		}

		// todo :上传附件
		[ApiExplorerSettings(IgnoreApi = true)]
		protected bool UploadAttach(string name, long? userId, long? noteId, out string msg, out long? serverFileId)
		{
			msg = "";
			serverFileId = 0;
			FileStoreConfig config = configFileService.ReadConfig().FileStoreConfig;

			var diskFileId = idGenerator.NextId();
			serverFileId = diskFileId;
			var httpFiles = _accessor.HttpContext.Request.Form.Files;
			//检查是否登录
			if (userId == 0)
			{
				userId = GetUserIdBySession();
				if (userId == 0)
				{
					msg = "NoLogin";
					return false;
				}
			}

			if (httpFiles == null || httpFiles.Count < 1)
			{
				return false;
			}
			var httpFile = httpFiles[name];
			var fileEXT = Path.GetExtension(httpFile.FileName).Replace(".", "");
			if (!IsAllowAttachExt(fileEXT))
			{
				msg = $"The_Attach_extension_{fileEXT}_is_blocked";
				return false;
			}
			var fileName = diskFileId.ToHex() + "." + fileEXT;
			//判断合法性
			if (httpFiles == null || httpFile.Length < 0)
			{
				return false;
			}
			//将文件保存在磁盘
			// Task<bool> task = noteFileService.SaveUploadFileOnUPYunAsync(upyun, httpFile, uploadDirPath, fileName);
			//Task<bool> task = noteFileService.SaveUploadFileOnDiskAsync(httpFile, uploadDirPath, fileName);

			var ext = Path.GetExtension(fileName);
			var provider = new FileExtensionContentTypeProvider();
			var memi = provider.Mappings[ext];
			var nowTime = DateTime.Now;
			var objectName = $"{userId.ToHex()}/attachments/{nowTime.ToString("yyyy")}/{nowTime.ToString("MM")}/{diskFileId.ToHex()}{ext}";
			bool result = noteFileService.SaveFile(objectName, httpFile, memi).Result;

			if (result)
			{
				//将结果保存在数据库
				AttachInfo attachInfo = new AttachInfo()
				{
					Id = diskFileId,
					UserId = userId,
					NoteId = noteId,
					UploadUserId = userId,
					Name = fileName,
					Title = httpFile.FileName,
					Size = httpFile.Length,
					Path = fileName,
					Type = fileEXT.ToLower(),
					CreatedTime = DateTime.Now
					//todo: 增加特性=图片管理
				};
				var AddResult = attachService.AddAttach(attachInfo, true, out string AttachMsg);
				if (!AddResult)
				{
					msg = "添加数据库失败";
				}
				return AddResult;
			}
			else
			{
				msg = "磁盘保存失败";
				return false;
			}
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		protected void UploadAudio()
		{
		}

		//上传图片 png jpg
		[ApiExplorerSettings(IgnoreApi = true)]
		protected bool UploadImage()
		{
			return false;
		}

		/// <summary>
		/// 获取文件MEMI
		/// </summary>
		/// <param name="ext"></param>
		/// <returns></returns>
		[ApiExplorerSettings(IgnoreApi = true)]
		protected string GetMemi(string ext)
		{
			try
			{
				var provider = new FileExtensionContentTypeProvider();
				if (provider.Mappings.ContainsKey(ext))
				{
					var memi = provider.Mappings[ext];
					return memi;
				}
				else
				{
					return "application/octet-stream";
				}
			}
			catch (Exception)
			{
				return "application/octet-stream";
				throw;
			}
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		protected bool UploadImages(string name, long? userId, long? noteId, bool isAttach, out long? serverFileId, out string msg)
		{
			if (isAttach)
			{
				return UploadAttach(name, userId, noteId, out msg, out serverFileId);
			}
			msg = "";
			serverFileId = 0;
			FileStoreConfig config = configFileService.ReadConfig().FileStoreConfig;
			string uploadDirPath = null;

			var diskFileId = idGenerator.NextId();
			serverFileId = diskFileId;
			var httpFiles = _accessor.HttpContext.Request.Form.Files;
			//检查是否登录
			if (userId == 0)
			{
				userId = GetUserIdBySession();
				if (userId == 0)
				{
					msg = "NoLogin";
					return false;
				}
			}

			if (httpFiles == null || httpFiles.Count < 1)
			{
				return false;
			}
			var httpFile = httpFiles[name];
			var fileEXT = Path.GetExtension(httpFile.FileName).Replace(".", "");
			var ext = Path.GetExtension(httpFile.FileName);
			if (!IsAllowImageExt(fileEXT))
			{
				msg = $"The_image_extension_{fileEXT}_is_blocked";
				return false;
			}
			var fileName = diskFileId.ToHex() + "." + fileEXT;
			//判断合法性
			if (httpFiles == null || httpFile.Length < 0)
			{
				return false;
			}
			//将文件保存在磁盘
			//Task<bool> task = noteFileService.SaveUploadFileOnDiskAsync(httpFile, uploadDirPath, fileName);

			var provider = new FileExtensionContentTypeProvider();
			var memi = provider.Mappings[ext];
			var nowTime = DateTime.Now;
			var objectName = $"{userId.ToHex()}/images/{nowTime.ToString("yyyy")}/{nowTime.ToString("MM")}/{diskFileId.ToHex()}{ext}";
			bool result = noteFileService.SaveFile(objectName, httpFile, memi).Result;

			if (result)
			{
				//将结果保存在数据库
				NoteFile noteFile = new NoteFile()
				{
					Id = diskFileId,
					UserId = userId,
					AlbumId = 1,
					Name = fileName,
					Title = fileName,
					Path = uploadDirPath + fileName,
					Size = httpFile.Length,
					CreatedTime = nowTime
					//todo: 增加特性=图片管理
				};
				var AddResult = noteFileService.AddImage(noteFile, 0, userId, true);
				if (!AddResult)
				{
					msg = "添加数据库失败";
				}
				return AddResult;
			}
			else
			{
				msg = "磁盘保存失败";
				return false;
			}
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		protected UploadData UploadImagesOrAttach(long? userId, out string msg)
		{
			//检查哈登录
			msg = string.Empty;
			if (userId == null)
			{
				msg = "Need to log in";
				return null;
			}

			FileStoreConfig config = configFileService.ReadConfig().FileStoreConfig;
			string uploadDirPath = null;

			var diskFileId = idGenerator.NextId();

			var httpFiles = _accessor.HttpContext.Request.Form.Files;
			if (httpFiles == null || httpFiles.Count < 1)
			{
				msg = "Invalid upload";
				return null;
			}
			var uploadData = new UploadData();
			foreach (var httpFile in httpFiles)
			{
				string uploadType = "images";//images  attachments
				var fileEXT = Path.GetExtension(httpFile.FileName).Replace(".", "");
				var ext = Path.GetExtension(httpFile.FileName);
				if (IsAllowImageExt(fileEXT))
				{
					//msg = $"The_image_extension_{fileEXT}_is_blocked";
					//return null;
					uploadType = "images";
				}
				else if (IsAllowAttachExt(fileEXT))
				{
					uploadType = "attachments";
				}
				else
				{
					uploadData.errFiles.Add(httpFile.FileName);
					continue;
				}
				var fileName = diskFileId.ToHex() + "." + fileEXT;

				//将文件保存在磁盘
				//Task<bool> task = noteFileService.SaveUploadFileOnDiskAsync(httpFile, uploadDirPath, fileName);
				try
				{
					var provider = new FileExtensionContentTypeProvider();
					var memi = provider.Mappings[ext];
					var nowTime = DateTime.Now;
					var objectName = $"{userId.ToHex()}/{uploadType}/{nowTime.ToString("yyyy")}/{nowTime.ToString("MM")}/{diskFileId.ToHex()}{ext}";
					bool result = noteFileService.SaveFile(objectName, httpFile, memi).Result;

					if (result)
					{
						//将结果保存在数据库
						NoteFile noteFile = new NoteFile()
						{
							Id = diskFileId,
							UserId = userId,
							AlbumId = 1,
							Name = fileName,
							Title = fileName,
							Path = uploadDirPath + fileName,
							Size = httpFile.Length,
							CreatedTime = nowTime
							//todo: 增加特性=图片管理
						};
						var AddResult = noteFileService.AddImage(noteFile, 0, userId, true);
						if (!AddResult)
						{
							msg = "添加数据库失败";
							uploadData.errFiles.Add(httpFile.FileName);
							continue;
						}
						else
						{
							uploadData.succMap.Add(httpFile.FileName, "/api/File/Images/" + diskFileId.ToHex());
						}
					}
					else
					{
						msg = "磁盘保存失败";
						uploadData.errFiles.Add(httpFile.FileName);
						continue;
					}
				}
				catch (Exception ex)
				{
					uploadData.errFiles.Add(httpFile.FileName);
					continue;
				}
			}
			msg = "success";
			return uploadData;
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		protected string UploadImagesOrAttach(ref FileModel fileModel, out string msg, long? userId)
		{
			if (fileModel == null)
			{
				msg = "fileModel=null";
				return null;
			}
			//检查哈登录
			msg = string.Empty;


			if (userId == null)
			{
				msg = "Need to log in";
				return null;
			}

			if (string.IsNullOrEmpty(fileModel.fileName) || fileModel.fileName.IndexOf(".") == -1)
			{
				fileModel.fileName = "unknow.jpg";
			}
			if (fileModel.data == null || fileModel.data.Length == 0)
			{
				return null;

			}
			WebSiteConfig webSiteConfig = configFileService.ReadConfig();
			FileStoreConfig fileStoreConfig = configFileService.ReadConfig().FileStoreConfig;
			string uploadDirPath = null;

			var diskFileId = idGenerator.NextId();

			string uploadType = "images";//images  attachments
			string datafileName = fileModel.fileName;
			if (datafileName.IndexOf("&") > 1)
			{
				datafileName = datafileName.Substring(0, datafileName.IndexOf("&"));
			}

			var fileEXT = Path.GetExtension(datafileName).Replace(".", "");

			var ext = Path.GetExtension(datafileName);
			if (IsAllowImageExt(fileEXT))
			{
				//msg = $"The_image_extension_{fileEXT}_is_blocked";
				//return null;
				uploadType = "images";
			}
			else if (IsAllowAttachExt(fileEXT))
			{
				uploadType = "attachments";
			}
			else
			{
				uploadType = "images";
				fileEXT = "png";
				ext = ".png";

			}
			var fileName = diskFileId.ToHex() + "." + fileEXT;
			string resultURL = string.Empty;//最终返回URL
											//将文件保存在磁盘
											//Task<bool> task = noteFileService.SaveUploadFileOnDiskAsync(httpFile, uploadDirPath, fileName);
			try
			{
				var provider = new FileExtensionContentTypeProvider();
				var memi = provider.Mappings[ext];
				var nowTime = DateTime.Now;
				var objectName = $"{userId.ToHex()}/{uploadType}/{nowTime.ToString("yyyy")}/{nowTime.ToString("MM")}/{diskFileId.ToHex()}{ext}";
				bool result = noteFileService.SaveFile(objectName, fileModel.data, memi).Result;

				if (result)
				{
					//将结果保存在数据库
					NoteFile noteFile = new NoteFile()
					{
						Id = diskFileId,
						UserId = userId,
						AlbumId = 1,
						Name = fileName,
						Title = fileName,
						Path = uploadDirPath + fileName,
						Size = fileModel.data.Length,
						CreatedTime = nowTime
						//todo: 增加特性=图片管理
					};
					var AddResult = noteFileService.AddImage(noteFile, 0, userId, true);
					if (!AddResult)
					{
						msg = "添加数据库失败";
						return null;
					}
					else
					{
						resultURL = webSiteConfig.APPConfig.SiteUrl + "/api/File/Images/" + diskFileId.ToHex();
					}
				}
				else
				{
					msg = "磁盘保存失败";

					return null;
				}
			}
			catch (Exception ex)
			{
				msg = ex.Message;
				return null;
			}
			msg = "success";
			return resultURL;
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		protected async Task<FileModel> DownLoadFile(string url)
		{
			try
			{

				//建立请求
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				//添加Referer信息
				request.Headers.Add(HttpRequestHeader.Referer, url);
				//伪装成谷歌浏览器
				request.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36");
				//request.Headers.Add(HttpRequestHeader.UserAgent, "Power By www.morenote.top");

				if (request.CookieContainer == null)
				{
					request.CookieContainer = new CookieContainer();
				}

				//发送请求获取Http响应
				HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false);
				//HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync().ConfigureAwait(false));

				var originalString = response.ResponseUri.OriginalString;
				Console.WriteLine(originalString);
				//获取响应流
				Stream receiveStream = response.GetResponseStream();
				//获取响应流的长度
				int length = (int)response.ContentLength;
				//读取到内存
				MemoryStream stmMemory = new MemoryStream();
				byte[] buffer1 = new byte[length];
				int i;
				//将字节逐个放入到Byte 中
				while ((i = await receiveStream.ReadAsync(buffer1, 0, buffer1.Length).ConfigureAwait(false)) > 0)
				{
					stmMemory.Write(buffer1, 0, i);
				}
				//写入磁盘
				string name = System.IO.Path.GetFileName(originalString);
				byte[] imageBytes = stmMemory.ToArray();

				stmMemory.Close();
				receiveStream.Close();
				response.Close();
				var fileModel = new FileModel
				{
					fileName = name,
					data = imageBytes
				};

				return fileModel;
			}
			catch (Exception ex)
			{

				return null;
			}
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		protected bool IsAllowAttachExt(string ext)
		{
			//上传文件扩展名 白名单  后期会集中到一个类里面专门处理上传文件的问题
			HashSet<string> exts = new HashSet<string>() { "bmp","jpg","png","tif","gif","pcx","tga","exif","fpx","svg","psd","cdr","pcd","dxf","ufo","eps","ai","raw","WMF","webp",
				"zip","7z","rar"
				,"mp4","mp3",
				"doc","docx","ppt","pptx","xls","xlsx"};
			if (exts.Contains(ext.ToLower()))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//检查上传图片后缀名
		[ApiExplorerSettings(IgnoreApi = true)]
		protected bool IsAllowImageExt(string ext)
		{
			HashSet<string> exts = new HashSet<string>() { "bmp", "jpg", "jpeg", "png", "tif", "gif", "pcx", "tga", "exif", "fpx", "svg", "psd", "cdr", "pcd", "dxf", "ufo", "eps", "ai", "raw", "WMF", "webp" };
			if (exts.Contains(ext.ToLower()))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 返回结果json
		/// </summary>
		/// <param name="data">响应数据</param>
		/// <param name="success">响应状态</param>
		/// <param name="message">响应消息</param>
		/// <param name="isLogin">登录状态</param>
		/// <returns></returns>
		[ApiExplorerSettings(IgnoreApi = true)]
		protected ActionResult ResultData(object data, bool success = true, string message = "", bool isLogin = true)
		{
			return Ok(new
			{
				IsLogin = isLogin,
				Success = success,
				Message = message,
				Data = data
			});
		}
		[ApiExplorerSettings(IgnoreApi = true)]
		protected IActionResult SimpleJson(object? data)
		{
			return Json(data, MyJsonConvert.GetSimpleOptions());
		}
		[ApiExplorerSettings(IgnoreApi = true)]
		protected IActionResult LeanoteJson(object? data)
		{
			return Json(data, MyJsonConvert.GetLeanoteOptions());
		}
	}
}