using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.DrawingCore.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MoreNote.Logic.Model
{
    public class JavaScriptResult : IActionResult
    {
        public string JsScript { get; set; }
        public JavaScriptResult()
        {

        }
        public JavaScriptResult(string script)
        {
            this.JsScript = script;
        }
        public Task ExecuteResultAsync(ActionContext context)
        {
            var response= context.HttpContext.Response;
            response.ContentType = "application/javascript; charset=utf-8";
           // return response.WriteAsync(HttpUtility.JavaScriptStringEncode(this.JsScript));
            return response.WriteAsync(this.JsScript);
        }
    }
}
