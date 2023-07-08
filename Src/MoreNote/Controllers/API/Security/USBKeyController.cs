using github.hyfree.GM.Common;

using log4net.Core;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Controllers.API.APIV1;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Security.USBKey;
using MoreNote.Logic.Service.Security.USBKey.CSP;
using MoreNote.Models.DTO.Leanote;
using MoreNote.Models.DTO.Leanote.Auth;
using MoreNote.Models.DTO.Leanote.USBKey;
using MoreNote.Models.Entity.Leanote.Management.Loggin;
using MoreNote.Models.Entity.Leanote.User;

using Org.BouncyCastle.Asn1.Cmp;

using System;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Controllers.API.Security
{
    [Route("api/USBKey/[action]")]
    public class USBKeyController : APIBaseController
    {
        private AuthService authService;
        private EPassService ePass;
        private UsbKeyManagerService usbKeyManager;

        public USBKeyController(AttachService attachService,
             TokenSerivce tokenSerivce,
            NoteFileService noteFileService,
             UserService userService,
             ConfigFileService configFileService,
             IHttpContextAccessor accessor,
            AuthService authService,
             EPassService ePassService,
             UsbKeyManagerService usbKeyManager

            ) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.authService = authService;
            this.ePass = ePassService;
            this.usbKeyManager = usbKeyManager;
        }

        /// <summary>
        /// 获得服务器挑战
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult LoginChallengeRequest(string email, string sessionCode)
        {
            var apiRe = new ApiResponseDTO();
            var user = userService.GetUserByEmail(email);
            if (user == null)
            {
                return LeanoteJson(apiRe);
            }
            var challenge = ePass.GenServerChallenge("LoginChallenge", sessionCode, user.Id);
            apiRe.Ok = true;
            apiRe.Data = challenge;
            return LeanoteJson(apiRe);
        }

        /// <summary>
        /// 对服务器的挑战给出响应
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> LoginChallengeResponse(string data)
        {
            var clinetResponse = ClientResponse.FromJSON(data);
            var challenge = ePass.GetServerChallenge(clinetResponse.Id);

            var result = await ePass.VerifyClientResponse(clinetResponse, false);

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in Request.Headers)
            {
                stringBuilder.Append(item.Key + "=" + item.Value.ToString() + "\r\n");
            }
            LoggingLogin logg = new LoggingLogin()
            {
                Id = this.idGenerator.NextId(),
                LoginDateTime = DateTime.Now,
                LoginMethod = "SmartToken",
                Ip = Request.Host.Host,
                BrowserRequestHeader = stringBuilder.ToString(),
            };

            ApiResponseDTO apiRe = new ApiResponseDTO();

            try
            {
                if (result)
                {
                    var userId = challenge.UserId;
                    var user = userService.GetUserByUserId(userId);
                    // var token = tokenSerivce.GenerateToken(user.UserId, user.Email);

                    //tokenSerivce.SaveToken(token);
                    //是否已经注册
                    var existCer= this.usbKeyManager.IsExist(user.Id,clinetResponse.Certificate);
                    if (!existCer)
                    {
                        apiRe.Ok=false;
                        apiRe.Msg= "USBKEY is not registered";
                        return LeanoteJson(apiRe);
                    }

                    //登录日志
                    logg.UserId = user.Id;
                    logg.IsLoginSuccess = true;
                    apiRe.Ok = true;
                    this.distributedCache.SetBool("USBKEY" + challenge.RequestNumber, true);

                    //apiRe.Data = userToken;
                    return LeanoteJson(apiRe);
                }
                else
                {
                    return LeanoteJson(apiRe);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                await logg.AddMac(this.cryptographyProvider);
                this.logging.Save(logg);
            }
        }
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> List(string userId)
        {
            ApiResponseDTO apiReDTO = new ApiResponseDTO();
            var list = this.usbKeyManager.ListAllUsbKey(userId.ToLongByHex());
            apiReDTO.Data = list;
            apiReDTO.Ok = true;
            return LeanoteJson(apiReDTO);
        }
        [HttpDelete]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Delete(string keyId, string token)
        {
            ApiResponseDTO apiReDTO = new ApiResponseDTO();
            var verify = this.tokenSerivce.VerifyToken(token);
            if (!verify)
            {
                apiReDTO.Ok = false;
                apiReDTO.Msg = "this.tokenSerivce.VerifyToken(token) == false";
                return LeanoteJson(apiReDTO);
            }
            var user = this.userService.GetUserByToken(token);
            this.usbKeyManager.DeleteUsbkey(keyId.ToLongByHex());
            if (!this.usbKeyManager.IsExist(user.Id))
            {
                //如果没有注册USBKEY，恢复默认的口令登录状态
                this.userService.SetUserLoginSecurityPolicyLevel(user.Id, LoginSecurityPolicyLevel.unlimited);
            }

           
            apiReDTO.Ok = true;
            return LeanoteJson(apiReDTO);
        }
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Find(string keyId)
        {
            ApiResponseDTO apiReDTO = new ApiResponseDTO();
           var key= this.usbKeyManager.Find(keyId.ToLongByHex());
            apiReDTO.Data= key;
            apiReDTO.Ok = true;
            return LeanoteJson(apiReDTO);
        }

        /// <summary>
        /// 注册挑战请求
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult RegistrationChallengeRequest(string email)
        {
            var apiRe = new ApiResponseDTO();
            var user = userService.GetUserByEmail(email);
            if (user == null)
            {
                return LeanoteJson(apiRe);
            }
            var sessionCode=RandomTool.CreatSafeRandomHex(32);//此处只是作为防重放
            var challenge = ePass.GenServerChallenge("RegistrationChallenge", sessionCode, user.Id);
            apiRe.Ok = true;
            apiRe.Data = challenge;
            return LeanoteJson(apiRe);
        }
        /// <summary>
        /// 注册挑战应答
        /// </summary>
        /// <param name="data"></param>
        /// <param name="token">用户token</param>
        /// <param name="sms">用户短信验证码或邮箱验证码</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> RegisterChallengeResponse(string data,string token,string  sms)
        {

            ApiResponseDTO apiRe = new ApiResponseDTO();
            var clinetResponse = ClientResponse.FromJSON(data);
            var challenge = ePass.GetServerChallenge(clinetResponse.Id);
            var verify = this.tokenSerivce.VerifyToken(token);
            if (string.IsNullOrEmpty(sms))
            {
                apiRe.Ok = false;
                apiRe.Msg = "sms == false";
                return LeanoteJson(apiRe);
            }
            if (!verify)
            {
                apiRe.Ok = false;
                apiRe.Msg = "this.tokenSerivce.VerifyToken(token) == false";
                return LeanoteJson(apiRe);
            }
            var user = this.userService.GetUserByToken(token);
            if (user == null)
            {
                apiRe.Ok = false;
                apiRe.Msg = "user==null";
                return LeanoteJson(apiRe);
            }
            if (!challenge.Tag.Equals("RegistrationChallenge"))
            {
                apiRe.Ok= false;
                apiRe.Msg= "RegistrationChallenge";
                return LeanoteJson(apiRe);
            }
            var result = await ePass.VerifyClientResponse(clinetResponse, false);

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in Request.Headers)
            {
                stringBuilder.Append(item.Key + "=" + item.Value.ToString() + "\r\n");
            }

            try
            {
                if (result)
                {
                    
                    var usbkey=new USBKeyBinding();
                    usbkey.Modulus= clinetResponse.Certificate;
                    //todo:这里应该动态设置，硬编码仅支持RSA1024
                    usbkey.Exponent= "010001";
                    usbkey.UserId=user.Id ;
                    if (this.usbKeyManager.IsExist(user.Id,clinetResponse.Certificate))
                    {
                        apiRe.Ok = false;
                        apiRe.Msg= "USBKey has been registered";
                        //apiRe.Data = userToken;
                        return LeanoteJson(apiRe);
                    }
                    this.usbKeyManager.Add(usbkey);
                    apiRe.Ok = true;
                    apiRe.Data=usbkey;
                    //apiRe.Data = userToken;
                    return LeanoteJson(apiRe);
                }
                else
                {
                    return LeanoteJson(apiRe);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }


    }
}