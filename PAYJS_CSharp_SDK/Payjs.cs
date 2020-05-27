using MoreNote.Common.Utils;

using PAYJS_CSharp_SDK.Model;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PAYJS_CSharp_SDK
{
   public  class Payjs
    {
        private string[] apiList = new string[] { "native", "cashier", "jsapi", "micropay", "facepay", "refund", "close", "user", "info" };

        private string mchid;

        private string key;

        private Dictionary<string, string> apiUrl;

        public Payjs(string mchid, string key, string server = "https://payjs.cn/api/")
        {
            this.mchid = mchid;
            this.key = key;

            apiUrl = new Dictionary<string, string>();
            foreach (string apiName in apiList)
            {
                apiUrl.Add(apiName, server + apiName);
            }
        }

        /// <summary>
        /// 收款码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string native(Dictionary<string, string> param)
        {
            string url = this.apiUrl[MethodBase.GetCurrentMethod().Name];
            return this.post(url, param);
        }
        public NativeResponseMessage native(NativeRequestMessage nativeRequestMessage)
        {

            Dictionary<string, string> param = new Dictionary<string, string>();
            param["total_fee"] = nativeRequestMessage.total_fee.ToString();
            param["out_trade_no"] = nativeRequestMessage.out_trade_no.ToString(); ;
            param["body"] = nativeRequestMessage.body;
            param["attch"] = nativeRequestMessage.attch;
            //可选项目
            if (!String.IsNullOrEmpty(nativeRequestMessage.notify_url))
            {
                param["notify_url"] = nativeRequestMessage.notify_url;
            }
            if (!String.IsNullOrEmpty(nativeRequestMessage.type))
            {
                param["type"] = nativeRequestMessage.notify_url;
            }
            string url = this.apiUrl[MethodBase.GetCurrentMethod().Name];
            String json= this.post(url, param);
            NativeResponseMessage message= JsonSerializer.Deserialize<NativeResponseMessage>(json, MyJsonConvert.GetOptions());
            return message;
        }

        /// <summary>
        /// 收银台
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string cashier(Dictionary<string, string> param)
        {
            string url = this.apiUrl[MethodBase.GetCurrentMethod().Name];
            return url + "?" + buildParam(param);
        }

        /// <summary>
        /// JSAPI
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string jsapi(Dictionary<string, string> param)
        {
            string url = this.apiUrl[MethodBase.GetCurrentMethod().Name];
            return this.post(url, param);
        }

        /// <summary>
        /// 刷卡支付
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string micropay(Dictionary<string, string> param)
        {
            string url = this.apiUrl[MethodBase.GetCurrentMethod().Name];
            return this.post(url, param);
        }

        /// <summary>
        /// 人脸支付
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string facepay(Dictionary<string, string> param)
        {
            string url = this.apiUrl[MethodBase.GetCurrentMethod().Name];
            return this.post(url, param);
        }

        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string check(Dictionary<string, string> param)
        {
            string url = this.apiUrl[MethodBase.GetCurrentMethod().Name];
            return this.post(url, param);
        }
        public CheckResponseMessage Check(CheckRequestMessage  requestMessage)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param["payjs_order_id"] = requestMessage.payjs_order_id;
            string url = this.apiUrl[MethodBase.GetCurrentMethod().Name];
            string json= this.post(url, param);
            CheckResponseMessage checkRequestMessage= JsonSerializer.Deserialize<CheckResponseMessage>(json, MyJsonConvert.GetOptions());
            return checkRequestMessage;
        }

        /// <summary>
        /// 关闭订单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string close(Dictionary<string, string> param)
        {
            string url = this.apiUrl[MethodBase.GetCurrentMethod().Name];
            return this.post(url, param);
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string refund(Dictionary<string, string> param)
        {
            string url = this.apiUrl[MethodBase.GetCurrentMethod().Name];
            return this.post(url, param);
        }

        /// <summary>
        /// 异步通知的数据校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool notifyCheck(Dictionary<string, string> param)
        {
            string originSign = param["sign"];
            param.Remove("sign");
            return sign(param)["sign"] == originSign;
        }
        public bool notifyCheck(NotifyResponseMessage notifyResponseMessage)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param["return_code"] = notifyResponseMessage.return_code.ToString();
            param["total_fee"] = notifyResponseMessage.total_fee.ToString();
            param["out_trade_no"] = notifyResponseMessage.out_trade_no.ToString();
            param["payjs_order_id"] = notifyResponseMessage.payjs_order_id;
            param["transaction_id"] = notifyResponseMessage.transaction_id;
            param["time_end"] = notifyResponseMessage.time_end;
            param["openid"] = notifyResponseMessage.openid;
            if (!string.IsNullOrEmpty(notifyResponseMessage.attach))
            {
                param["attach"] = notifyResponseMessage.attach;
            }
            if (!string.IsNullOrEmpty(notifyResponseMessage.type))
            {
                param["type"] = notifyResponseMessage.type;
            }
            param["sign"] = notifyResponseMessage.sign;
         
            string originSign = param["sign"];
            param.Remove("sign");
            return sign(param)["sign"] == originSign;
        }

        /// <summary>
        /// 用户详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string user(Dictionary<string, string> param)
        {
            string url = this.apiUrl[MethodBase.GetCurrentMethod().Name];
            return this.post(url, param);
        }

        /// <summary>
        /// 商户资料
        /// </summary>
        /// <returns></returns>
        public string info()
        {
            string url = this.apiUrl[MethodBase.GetCurrentMethod().Name];
            return this.post(url, new Dictionary<string, string>());
        }


        private Dictionary<string, string> sign(Dictionary<string, string> param)
        {
            param.Add("mchid", this.mchid);
            //去掉空的，排序
            Dictionary<string, string> newParam = param.Where(w => w.Value.Trim() != "").
                OrderBy(o => o.Key).ToDictionary(d => d.Key, d => d.Value);
            string paramStr = "";
            //拼接
            foreach (KeyValuePair<string, string> keyPair in newParam)
            {
                paramStr += (keyPair.Key + "=" + keyPair.Value + "&");
            }
            //加上key
            paramStr += "key=" + this.key;
            //md5后大写
            string sign = (md5(paramStr)).ToUpper();

            newParam.Add("sign", sign);
            return newParam;
        }

        private string md5(string str)
        {
            //from https://www.cnblogs.com/zq20/p/6268243.html
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        private string buildParam(Dictionary<string, string> param)
        {
            param = sign(param);
            if (!(param == null || param.Count == 0))
            {
                StringBuilder sb = new StringBuilder();
                foreach (string key in param.Keys)
                {
                    sb.AppendFormat("{0}={1}&", key, param[key]);
                }
                return sb.ToString().Substring(0, sb.Length - 1);
            }
            return "";
        }

        private string post(string url, Dictionary<string, string> parameters)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            request.Method = "POST";
            request.UserAgent = "PAYJS C# SDK BY LIBINBIN";
            request.ContentType = "application/x-www-form-urlencoded";
            string str = buildParam(parameters);
            if (str != "")
            {
                byte[] data = Encoding.UTF8.GetBytes(str);
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            using (Stream s = request.GetResponse().GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }

    }
}
