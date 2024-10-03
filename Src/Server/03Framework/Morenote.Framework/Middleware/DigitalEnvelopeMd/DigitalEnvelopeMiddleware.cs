using github.hyfree.GM;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using MoreNote.Logic.Service;
using MoreNote.Models.DTO.Leanote.ApiRequest;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morenote.Framework.Middleware.DigitalEnvelopeMd
{
    public class DigitalEnvelopeMiddleware
    {
        private readonly RequestDelegate _next;
        GMService gMService;
        string PrivateKey;

        public DigitalEnvelopeMiddleware(RequestDelegate next, GMService gMService, ConfigFileService configFileService)
        {
            _next = next;
            this.gMService = gMService;
            this.PrivateKey = configFileService.ReadConfig().SecurityConfig.PrivateKey;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var enc_field = context.Request.Headers["enc_field"];
            if (!string.IsNullOrWhiteSpace(enc_field) && context.Request.Method.Equals("POST"))
            {

                var digitalEnvelopeJson = context.Request.Form[enc_field];

                DigitalEnvelope digitalEnvelope = null;
                var verify = false;
                //数字信封
                digitalEnvelope = DigitalEnvelope.FromJSON(digitalEnvelopeJson);
                var data = digitalEnvelope.GetPayLoadValue(this.gMService, this.PrivateKey);
                if (data == null)
                {
                    throw new Exception("数字信封解密失败");
                }
                //赋予解密的数字信封
                var form = await context.Request.ReadFormAsync();

                // 创建一个新的 Dictionary 来保存修改后的 Form 数据
                var newForm = form.ToDictionary(x => x.Key, x => x.Value);
                // 在这里修改你需要修改的字段
                if (newForm.ContainsKey(enc_field))
                {
                    newForm[enc_field] = new StringValues(data);
                }
                // 构建新的 Form 集合
                var newFormCollection = new FormCollection(newForm);

                // 替换原始的 Form 数据
                context.Request.Form = newFormCollection;

            }

            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }
}
