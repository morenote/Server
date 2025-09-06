using github.hyfree.GM;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using MoreNote.Common.Utils;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Security.USBKey.CSP;
using MoreNote.Models.DTO.Leanote;
using MoreNote.Models.DTO.Leanote.USBKey;

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morenote.Framework.Filter.Global
{
	/// <summary>
	/// 检查报文是否被签名
	/// </summary>
	public class MessageSignFilter : Attribute, IAsyncAuthorizationFilter
    {
		public TokenSerivce tokenSerivce;
		private EPassService ePassService;
        private DataSignService dataSignService;

        private GMService gm;
        public string Operate { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
		private ISession _session => _httpContextAccessor.HttpContext.Session;

		public MessageSignFilter(TokenSerivce tokenSerivce,
			EPassService ePassService,
            GMService gMService,
            DataSignService dataSignService)
		{
			this.tokenSerivce = tokenSerivce;
			this.ePassService = ePassService;
			this.dataSignService = dataSignService;
            this.gm = gMService;
		}

		public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
		{
            //读取header中的签名字段
			var sign_field = context.HttpContext.Request.Headers["sign_field"];
			var sign = context.HttpContext.Request.Headers["sign"];
            if (string.IsNullOrEmpty(sign_field))
            {
                SignError(context, "sign_field IsNullOrEmpty ,id=1");
                return;
            }
            var data = context.HttpContext.Request.Form[sign_field];
           

            var url=context.HttpContext.Request.GetDisplayUrl();
            var index=url.IndexOf("/api");
            url=url.Substring(index);

			if (string.IsNullOrEmpty(sign_field) || string.IsNullOrEmpty(data))
			{
				 SignError(context, "sign_field IsNullOrEmpty ");
				return;
            }
            try
            {
                //验证签名
                var dataSign = DataSignDTO.FromJSON(sign);
                var verify = await this.ePassService.VerifyDataSign(dataSign);
                if (!verify)
                {
                    SignError(context, "VerifyDataSign is Error ");
                    return;
                }
                verify = dataSign.SignData.Operate.Equals(url);
                if (!verify)
                {
                    SignError(context, "Operate Is Error ");
                    return;
                }
                //验证格式是否正确
                var hash = gm.SM3String(data).ToUpper();
                if (!dataSign.SignData.Hash.Equals(hash))
                {
                    SignError(context, "hash Is Error ");
                    return;
                }

                //签名存证
                this.dataSignService.AddDataSign(dataSign, this.Operate);

            }
            catch (Exception ex)
            {

                SignError(context, ex.Message);
                return;
            }
           
		}
		public void SignError(AuthorizationFilterContext context,string errMessage)
		{
            ApiResponseDTO apiRe = new ApiResponseDTO()
            {
                Ok = false,
                Msg = errMessage,
            };
            //return Json(apiRe, MyJsonConvert.GetOptions());
            context.Result = new JsonResult(apiRe, MyJsonConvert.GetSimpleOptions());
            
        }

       
    }
}