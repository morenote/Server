using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Model
{
    public class UserLoginSecurityStrategy
    {
        
        public long? UserId { get; set; }//用户id
        
        public string UserName { get; set; }//用户名
        public bool EnhancedValidationRequired { get; set; } = false;//是否需要增强认证
        //=========================密码登录策略===============================
        public bool AllowPassWordLogin { get; set; } = true; //允许使用密码登录登录
        //=========================安全设备登录认证策略==============
        public bool AllowFIDO2Login { get; set; } = false;//允许使用FIDO2协议登录
        public bool AllowUSBKeyLogin { get; set; } = false;//允许使用UsbKey协议登录
        public bool AllowQRCodeLogin { get; set; } = false;//允许使用APP扫描登录登录
        //===============================增强认证策略（双因素）===================
        public bool EVGoogle2StepAuthentication { get; set; } = false;//谷歌两步认证增强认证
        public bool EVFace { get; set;  } = false;//人脸识别增强
        public bool EVEmailCode { get; set; } = false;//邮箱验证码
        public bool EVSMCode { get; set; } = false;//短信验证码

    }
}
