using MoreNote.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace PAYJS_CSharp_SDK.Model
{
    /// <summary>
    /// 订单查询接口返回数据
    /// </summary>
    public class CheckResponseMessage
    {
        /// <summary>
        /// 1:请求成功 0:请求失败
        /// </summary>
        public int return_code { get; set; }
        /// <summary>
        /// PAYJS 平台商户号
        /// </summary>
        public string mchid { get; set; }
        /// <summary>
        /// 用户端订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// PAYJS 订单号
        /// </summary>
        public string payjs_order_id { get; set; }
        /// <summary>
        /// 微信显示订单号
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>
        /// 0：未支付，1：支付成功
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 用户 OPENID
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        ///订单金额
        /// </summary>
        public int total_fee { get; set; }
        /// <summary>
        /// 订单支付时间
        /// </summary>
        public string paid_time { get; set; }
        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public string attach { get; set; }
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
