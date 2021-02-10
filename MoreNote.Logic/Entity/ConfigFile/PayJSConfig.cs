using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Entity.ConfigFile
{

    public class PayJSConfig
    {
        /// <summary>
        /// 支付渠道代号
        /// </summary>
        public  int PayCode{get; }=1;
        //商户ID
        public String PayJS_MCHID { get; set; }
        //密钥
        public String PayJS_Key { get; set; }
        public static PayJSConfig GenerateTemplate()
        {
            PayJSConfig payJSConfig=new PayJSConfig()
            {
                PayJS_MCHID= "商户ID",
                PayJS_Key= "密钥"
            };
            return payJSConfig;
        }
    }
}
