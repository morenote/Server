using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.Utils;
using MoreNote.Framework.Controllers;
using MoreNote.Logic.Database;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;

using PAYJS_CSharp_SDK;
using PAYJS_CSharp_SDK.Model;

using System;
using System.Linq;
using System.Net;

namespace MoreNote.Controllers
{
    
    public class PayJSController : BaseController
    {
        private DataContext dataContext;
        private ConfigFileService configFileService;
        public PayJSController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor
            , DataContext dataContext
            , ILoggingService loggingService) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor, loggingService)
        {
            this.dataContext = dataContext;
            this.configFileService= configFileService;
            webSiteConfig = configFileService.WebConfig;
            pay = new Payjs(webSiteConfig.Payjs.PayJS_MCHID, webSiteConfig.Payjs.PayJS_Key);

        }
        private  WebSiteConfig webSiteConfig;

        private  Payjs pay ;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NativePay()
        {
            long? id = SnowFlakeNet.GenerateSnowFlakeID();
            var nativeRequestMessage = new NativeRequestMessage()
            {
                total_fee = 1,
                out_trade_no = id.ToString(),
                body = "test",
                attch = "userid",
                notify_url = webSiteConfig.Payjs.Notify_Url
            };

            var responseMessage = pay.native(nativeRequestMessage);
            ViewBag.msg = responseMessage;
            CommodityOrder goodOrder = new CommodityOrder()
            {
                CommodityOrderId = id,
                mchid = webSiteConfig.Payjs.PayJS_MCHID,
                total_fee = nativeRequestMessage.total_fee,
                out_trade_no = id.ToString(),
                body = nativeRequestMessage.body,
                attch = nativeRequestMessage.attch,
                notify_url = nativeRequestMessage.notify_url,
                type = nativeRequestMessage.type,
                payjs_order_id = responseMessage.payjs_order_id,
                NativeRequestMessage = nativeRequestMessage.ToJsonString(),
                NativeResponseMessage = responseMessage.ToJsonString()
            };
          
                var orderObj = dataContext.GoodOrder.Add(goodOrder);
                dataContext.SaveChanges();
            

            return View();
        }

        /// <summary>
        /// 支付成功异步通知接口
        /// </summary>
        /// <param name="notifyResponseMessage"></param>
        /// <returns></returns>
        public IActionResult AsyncNotification(NotifyResponseMessage notifyResponseMessage)
        {
            if (notifyResponseMessage == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("BadRequest");
            }
            else
            {
                Console.WriteLine(notifyResponseMessage.openid);
                bool identify = pay.notifyCheck(notifyResponseMessage);
                if (!identify)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Content("BadRequest");
                }
                try
                {
                   
                        var orderObj = dataContext.GoodOrder.Where(b => b.payjs_order_id.Equals(notifyResponseMessage.payjs_order_id)).FirstOrDefault();
                        if (orderObj.total_fee != notifyResponseMessage.total_fee)
                        {
                            throw new Exception("金额不正确");
                        }
                        orderObj.PayStatus = true;
                        orderObj.transaction_id = notifyResponseMessage.transaction_id;
                        orderObj.openid = notifyResponseMessage.openid;
                        orderObj.Notify = true;
                        orderObj.NotifyResponseMessage = notifyResponseMessage.ToJsonString();
                        dataContext.SaveChanges();
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Content("OK");
                    
                }
                catch (Exception)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Content("BadRequest");
                }
            }
        }
    }
}