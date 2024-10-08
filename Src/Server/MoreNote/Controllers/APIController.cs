﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

using Morenote.Framework.Filter.Global;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Config.ConfigFile;
using MoreNote.Framework.Controllers;
using MoreNote.Logic.Database;
using MoreNote.Logic.Entity;
using MoreNote.AutoFac.Property;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.DistributedIDGenerator;
using MoreNote.Logic.Service.FileService.IMPL;
using MoreNote.Logic.Service.VerificationCode;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

using UpYunLibrary.ContentRecognition;
using MoreNote.Logic.Service.Notes;

namespace MoreNote.Controllers
{
    public class APIController : BaseController
	{
		private AccessService AccessService { get; set; }

		private DataContext dataContext;
		private ConfigFileService configFileService;

		/// <summary>
		/// 保险丝
		/// </summary>
		private readonly int _randomImageFuseSize;

		/// <summary>
		/// 保险丝计数器
		/// 当访问量查过保险丝的能力时，直接熔断接口。
		/// </summary>
		private static int _fuseCount = 0;

		private static readonly object _fuseObj = new object();



		//目录分隔符
		private static readonly char dsc = Path.DirectorySeparatorChar;

		//private static Dictionary<string, string> typeName = new Dictionary<string, string>();
		private static WebSiteConfig webcConfig;

		private static Random random = new Random();

		private AccessService accessService;

		private IDistributedIdGenerator idGenerator;

		[Autowired]
		private ICaptchaGenerator captchaGenerator { get; set; }

		public APIController(AttachService attachService
			, TokenSerivce tokenSerivce
			, NoteFileService noteFileService
			, UserService userService
			, ConfigFileService configFileService
			, IHttpContextAccessor accessor,
			AccessService accessService,
			DataContext dataContext


			) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
		{
			this.AccessService = accessService;
			this.dataContext = dataContext;

			this.configFileService = configFileService;
			webcConfig = configFileService.ReadConfig();

			_randomImageFuseSize = webcConfig.PublicAPI.RandomImageFuseSize;

		}

		private class RandomImageResult
		{
			public int Error { get; set; }
			public int Result { get; set; }
			public int Count { get; set; }
			public List<string> Images { get; set; }
		}

		[Route("CacheServer/RandomImages/{typeMd5}/{fileName}")]
		[HttpGet]
		public async Task<IActionResult> MinIOImagesAsync(string typeMd5, string fileName)
		{
			MinIOFileStoreService minio = new MinIOFileStoreService(webcConfig.MinIOConfig.Endpoint, webcConfig.MinIOConfig.MINIO_ACCESS_KEY, webcConfig.MinIOConfig.MINIO_SECRET_KEY, webcConfig.MinIOConfig.WithSSL, webcConfig.MinIOConfig.BrowserDownloadExpiresInt);
			string fileExt = Path.GetExtension(fileName);
			var objectName = $"{typeMd5}/{fileName}";
			var data = await minio.GetObjecByteArraytAsync(webcConfig.MinIOConfig.RandomImagesBucketName, objectName);
			var provider = new FileExtensionContentTypeProvider();
			var memi = provider.Mappings[fileExt];

			return File(data, memi);
		}







		

		//浏览器检测
		public IActionResult BrowserDetection()
		{
			return Redirect($"https://www.morenote.top/BrowserDetection.js");
		}

		/// <summary>
		/// 获取图形验证码
		/// </summary>
		/// <returns></returns>
		// [HttpGet("VerifyCode")]
		[SkipInspectionInstallationFilter]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult VerifyCode()
		{
			Response.ContentType = "image/jpeg";

			var buffer = captchaGenerator.GenerateImage(out var code);

			//存session
			HttpContext.Session.SetString("VerifyCode", code.ToLower());

			//使用标志，不允许重复使用一个验证码。
			//这个验证码被消费一次后，要置0。
			HttpContext.Session.SetInt32("VerifyCodeValid", 1);

			//验证码生成时间。
			HttpContext.Session.SetInt32("VerifyCodeTime", UnixTimeUtil.GetTimeStampInInt32());

			//string sessionID = Request.Cookies["SessionID"];
			//RedisManager.SetString(sessionID, code);

			// Response.Cookies.Append("code",code);

			// 将验证码的token放入cookie
			// Response.Cookies.Append(VERFIY_CODE_TOKEN_COOKIE_NAME, await SecurityServices.GetVerifyCodeToken(code));

			return File(buffer, "image/png");
		}
	}
}