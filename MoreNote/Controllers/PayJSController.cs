﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.Config;
using MoreNote.Common.Config.Model;
using MoreNote.Common.Utils;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using PAYJS_CSharp_SDK;
using PAYJS_CSharp_SDK.Model;

namespace MoreNote.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class PayJSController : Controller
    {
        static WebSiteConfig webSiteConfig = ConfigManager.GetWebConfig();
        static Payjs pay = new Payjs(webSiteConfig.PayJS_MCHID, webSiteConfig.PayJS_Key);
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult NativePay()
        {

            long id = SnowFlake_Net.GenerateSnowFlakeID();
            var nativeRequestMessage = new NativeRequestMessage()
            {
                total_fee = 1,
                out_trade_no = id.ToString(),
                body = "test",
                attch = "userid",
                notify_url = "http://frp.morenote.top:7001/PayJS/AsyncNotification"
            };
         
           
            var responseMessage = pay.native(nativeRequestMessage);
            ViewBag.msg = responseMessage;
            GoodOrder goodOrder = new GoodOrder()
            {
                GoodOrderId = id,
                mchid = webSiteConfig.PayJS_MCHID,
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
            using (var db = new DataContext())
            {
                var orderObj = db.GoodOrder.Add(goodOrder);     
                db.SaveChanges();
            }
            
            return View();
        }
        /// <summary>
        /// 支付成功异步通知接口
        /// </summary>
        /// <param name="notifyResponseMessage"></param>
        /// <returns></returns>
        public IActionResult AsyncNotification(NotifyResponseMessage notifyResponseMessage)
        {
            if (notifyResponseMessage==null)
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
                    using (var db = new DataContext())
                    {
                        var orderObj = db.GoodOrder.Where(b => b.payjs_order_id.Equals(notifyResponseMessage.payjs_order_id)).FirstOrDefault() ;
                        if (orderObj.total_fee!=notifyResponseMessage.total_fee)
                        {
                            throw new Exception("金额不正确");
                        }
                        orderObj.PayStatus = true;
                        orderObj.transaction_id = notifyResponseMessage.transaction_id;
                        orderObj.openid = notifyResponseMessage.openid;
                        orderObj.Notify = true;
                        orderObj.NotifyResponseMessage = notifyResponseMessage.ToJsonString();
                        db.SaveChanges();
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Content("OK");

                    }
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
