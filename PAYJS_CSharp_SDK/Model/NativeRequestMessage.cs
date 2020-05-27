using MoreNote.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace PAYJS_CSharp_SDK.Model
{
    /// <summary>
    /// Native 扫码支付（主扫） API请求数据
    /// </summary>
    public class NativeRequestMessage
    {
        /// <summary>
        /// mchid
        /// </summary>
        public String mchid { get; set; }
        /// <summary>
        /// total_fee
        /// </summary>
        public int total_fee { get; set; }
        /// <summary>
        /// 用户端自主生成的订单号
        /// </summary>
        public String out_trade_no { get; set; }
        /// <summary>
        /// 订单标题
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 用户自定义数据，在notify的时候会原样返回
        /// </summary>
        public string attch { get; set; }
        /// <summary>
        /// 接收微信支付异步通知的回调地址。必须为可直接访问的URL，不能带参数、session验证、csrf验证。留空则不通知
        /// </summary>
        public string notify_url { get; set; }
        /// <summary>
        /// 支付宝交易传值：alipay ，微信支付无需此字段
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 数据签名
        /// </summary>
        public string sign { get; set; }
        public string ToJsonString()
        {
            String json  = JsonSerializer.Serialize(this, MyJsonConvert.GetOptions());
            return json;
        }
    }
}
