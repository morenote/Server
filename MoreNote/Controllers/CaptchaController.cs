using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using System.Threading.Tasks;
namespace MoreNote.Controllers
{
    public class CaptchaController : Controller
    {

        /// <summary>
        /// 获取图形验证码
        /// </summary>
        /// <returns></returns>
       // [HttpGet("VerifyCode")]
        public async Task Get()
        {
            Response.ContentType = "image/jpeg";


            using (var stream = CheckCode.Create(out var code))
            {
                var buffer = stream.ToArray();

                //存session

                HttpContext.Session.SetString("VerifyCode", code.ToLower());
                HttpContext.Session.SetInt32("VerifyCodeValid",1);
              

                //string sessionID = Request.Cookies["SessionID"];
                //RedisManager.SetString(sessionID, code);
               
               // Response.Cookies.Append("code",code);

                // 将验证码的token放入cookie
                // Response.Cookies.Append(VERFIY_CODE_TOKEN_COOKIE_NAME, await SecurityServices.GetVerifyCodeToken(code));

               await  Response.Body.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            }


        }
    }
}