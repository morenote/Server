using MoreNote.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace PAYJS_CSharp_SDK.Model
{
    /// <summary>
    ///  Native 扫码支付（主扫） API返回数据
    /// </summary>
    public class NativeResponseMessage
    {
        /// <summary>
        /// 1:请求成功，0:请求失败
        /// </summary>
        public int return_code { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        public string return_msg { get; set; }
        /// <summary>
        /// PAYJS 平台订单号
        /// </summary>
        public string payjs_order_id { get; set; }
        /// <summary>
        /// 用户生成的订单号原样返回
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 金额。单位：分
        /// </summary>
        public string total_fee { get; set; }
        /// <summary>
        /// 二维码图片地址
        /// </summary>
        public string qrcode { get; set; }
        /// <summary>
        /// 可将该参数生成二维码展示出来进行扫码支付(有效期2小时)
        /// </summary>
        public string code_url { get; set; }
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
