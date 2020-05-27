using MoreNote.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace PAYJS_CSharp_SDK.Model
{
    /// <summary>
    /// 支付成功异步通知接口返回信息
    /// </summary>
    public class NotifyResponseMessage
    {
        /// <summary>
        /// 1:请求成功，0:请求失败
        /// </summary>
        public int return_code { get; set; }
        /// <summary>
        /// 金额。单位：分
        /// </summary>
        public int total_fee { get; set; }
        /// <summary>
        /// 用户端自主生成的订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// PAYJS 订单号
        /// </summary>
        public string payjs_order_id { get; set; }
        /// <summary>
        /// 微信用户手机显示订单号
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>
        /// 支付成功时间
        /// </summary>
        public string time_end { get; set; }
        /// <summary>
        /// 用户OPENID标示，本参数没有实际意义，旨在方便用户端区分不同用户
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// PAYJS 商户号
        /// </summary>
        public string mchid { get; set; }
        /// <summary>
        /// 支付类型。微信订单不返回该字段；支付宝订单返回：alipay
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 数据签名
        /// </summary>
        public string sign { get; set; }
        public string ToJsonString()
        {
            String json = JsonSerializer.Serialize(this, MyJsonConvert.GetOptions());
            return json;
        }
    }
}
