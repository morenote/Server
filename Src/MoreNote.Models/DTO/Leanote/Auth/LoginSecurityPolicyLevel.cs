using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.DTO.Leanote.Auth
{
    /// <summary>
    /// 登录安全策略级别
    /// </summary>
    public enum LoginSecurityPolicyLevel
    {
        //U2F=人脸、谷歌动态令牌、安全问题 
        //密码设备=FIDO2、安全令牌

        unlimited=0,//无限制，可以使用单一因子登录
        loose =1,//宽松，在已经登录过并信任的设备上，可以使用单一因子登录
        strict =2,//严格，必须使用口令+U2F、FIDO2、智能密码钥匙三种方式的任意一种方式
        compliant= 3//合规，必须使用口令+智能密码钥匙的组合
    }
}
