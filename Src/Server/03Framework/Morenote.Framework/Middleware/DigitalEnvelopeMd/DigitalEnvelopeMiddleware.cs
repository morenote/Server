using github.hyfree.GM;

using Lucene.Net.Support.IO;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Service;
using MoreNote.Models.DTO.Leanote;
using MoreNote.Models.DTO.Leanote.ApiRequest;
using MoreNote.SecurityProvider.Core;

using SharpCompress;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
        ICryptographyProvider cryptography;

        public DigitalEnvelopeMiddleware(RequestDelegate next, GMService gMService, ConfigFileService configFileService, ICryptographyProvider cryptographyProvider)
        {
            _next = next;
            this.gMService = gMService;
            this.PrivateKey = configFileService.ReadConfig().SecurityConfig.PrivateKey;
            this.cryptography=cryptographyProvider;
           

        }
       
        public async Task InvokeAsync(HttpContext context)
        {
            DigitalEnvelope digitalEnvelope = null;
            var enc_field = context.Request.Headers["enc_field"];
            Stream originalBody=null;
            MemoryStream ms=null;
         

            if (!string.IsNullOrEmpty(enc_field) && context.Request.Method.Equals("POST"))
            {
                if (this.PrivateKey.Length > 64)
                {
                    var PrivateKeyBUff = await cryptography.SM4Decrypt(HexUtil.HexToByteArray(this.PrivateKey));
                    this.PrivateKey = HexUtil.ByteArrayToHex(PrivateKeyBUff);
                }

                var digitalEnvelopeJson = context.Request.Form["digitalEnvelopeJson"];

                
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


                // 保持原来的流
                 originalBody = context.Response.Body;

                // 用ms替换当前的流
                ms = new MemoryStream();
                context.Response.Body = ms;
            }



           


            await _next(context);
            if (!string.IsNullOrEmpty(enc_field) && ms!=null && originalBody!=null)
            {
                ms.Seek(0,SeekOrigin.Begin);
                var reader = new StreamReader(ms);
                var json=await reader.ReadToEndAsync();

                if (string.IsNullOrEmpty(json))
                {
                    return;

                }

                var apiRe=ApiResponseDTO.FormJson(json);


                var key = digitalEnvelope.getSM4Key(this.gMService, this.PrivateKey);


                var payLoad = new PayLoadDTO();
                payLoad.SetData(apiRe.Data.ToString());


                var payLoadJson = payLoad.ToJson();

                var jsonHex = HexUtil.ByteArrayToHex(Encoding.UTF8.GetBytes(payLoadJson));

                var encBuffer = gMService.SM4_Encrypt_CBC(Encoding.UTF8.GetBytes(payLoadJson), key.HexToByteArray(), digitalEnvelope.IV.HexToByteArray());
                var enc = HexUtil.ByteArrayToHex(encBuffer);
                apiRe.Data = enc;
                apiRe.Encryption = true;

                var apiReJson=apiRe.ToJson();
                using (StreamWriter streamWriter=new StreamWriter(originalBody))
                {
                  await  streamWriter.WriteAsync(apiReJson);
                }
            }
              
        }
    }
}
