
using System;
using System.Globalization;
using System.Text.Json;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.ModelBinder;
using MoreNote.Common.Utils;
using MoreNote.Controllers.API.APIV1;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;
using MoreNote.Models.DTO.Leanote;
using MoreNote.Models.DTO.Leanote.Config;

namespace MoreNote.Controllers.API
{
    [Route("api/Config/[action]")]
    public class ConfigController : APIBaseController
    {
        private AuthService authService;
        private UserService userService;
        private TokenSerivce tokenSerivce;
        public ConfigController(AttachService attachService
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
        /// <summary>
        /// 获得服务器的口令保护方案的密码算法
        /// <para>当服务器启用sjj1962或其他非对称加密算法时，客户端必须强制实现非对称加密算法的数字信封，</para>
        /// <para>用于保护用户传输信息的机密性</para>
        /// </summary>
        /// <param name="token"></param>
        /// <returns>SecurityConfigDTO</returns>
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult GetSecurityConfig()
        {
            var securityConfig = config.SecurityConfig;
            var dto = SecurityConfigDTO.Instance(securityConfig);
            var re = new ApiReDTO();
            re.Ok = true;
            re.Data = dto;
            return LeanoteJson(re);
        }
     
    }
}