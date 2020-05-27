using MoreNote.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace PAYJS_CSharp_SDK.Model
{
    public class CheckRequestMessage
    {
        /// <summary>
        /// PAYJS 平台订单号
        /// </summary>
        public String payjs_order_id { get; set; }
        public string ToJsonString()
        {
            String json = JsonSerializer.Serialize(this, MyJsonConvert.GetOptions());
            return json;
        }
    }
}
