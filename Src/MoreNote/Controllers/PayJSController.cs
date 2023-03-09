using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Framework.Controllers;
using MoreNote.Logic.Database;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.Service;
using MoreNote.Models.Entity.Leanote.Pay;
using PAYJS_CSharp_SDK;
using PAYJS_CSharp_SDK.Model;

using QRCoder;

using SixLabors.ImageSharp;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;

using Image = SixLabors.ImageSharp.Image;

namespace MoreNote.Controllers
{
    [Route("api/PayJS/[action]")]
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
            ) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.dataContext = dataContext;
            this.configFileService = configFileService;
            webSiteConfig = configFileService.WebConfig;
            pay = new Payjs(webSiteConfig.Payjs.PayJS_MCHID, webSiteConfig.Payjs.PayJS_Key);
        }

        private WebSiteConfig webSiteConfig;

        private Payjs pay;

        [HttpGet, HttpPost]
        public IActionResult Native()
        {
            long? id = idGenerator.NextId();
            var nativeRequestMessage = new NativeRequestMessage()
            {
                total_fee = 1,
                out_trade_no = id.ToString(),
                body = "test",
                attch = "userid",
                notify_url = webSiteConfig.Payjs.Notify_Url
            };

            var responseMessage = pay.Native(nativeRequestMessage);
            ViewBag.msg = responseMessage;
            CommodityOrder goodOrder = new CommodityOrder()
            {
                Id = id,
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

        [HttpGet, HttpPost]
        public IActionResult Cashier()
        {
            long? id = idGenerator.NextId();
            var cashierRequest = new CashierRequestMessage()
            {
                total_fee = 1,
                out_trade_no = id.ToString(),
                body = "test",
                attch = "userid",
                notify_url = webSiteConfig.Payjs.Notify_Url
            };
            var responseMessage = pay.Cashier(cashierRequest);
            return Content(responseMessage);
        }

        [HttpGet, HttpPost]
        public IActionResult CashierQR()
        {
            long? id = idGenerator.NextId();
            var cashierRequest = new CashierRequestMessage()
            {
                total_fee = 1,
                out_trade_no = id.ToString(),
                body = "test",
                attch = "userid",
                notify_url = webSiteConfig.Payjs.Notify_Url
            };
            var responseMessage = pay.Cashier(cashierRequest);
            var buffer = new byte[0];
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(responseMessage, QRCodeGenerator.ECCLevel.L);
                //QRCode qrCode = new QRCode(qrCodeData);
                //Bitmap qrCodeImage = qrCode.GetGraphic(20);
                var qrBmp = new BitmapByteQRCode(qrCodeData);
         
                Image image= Image.Load(qrBmp.GetGraphic(20));
                image.SaveAsJpeg(ms); 
                buffer = ms.ToArray();
            }
            return File(buffer, "image/jpeg");
        }

        /// <summary>
        /// 支付成功异步通知接口
        /// </summary>
        /// <param name="notifyResponseMessage"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
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