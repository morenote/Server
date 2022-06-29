using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using MoreNote.Models.Entity.ConfigFile;
using MoreNote.Models.Entity.ConfigFile.Security;

namespace MoreNote.Logic.Entity.ConfigFile
{
    public class SecurityConfig
    {
        /// <summary>
        /// 敏感信息加密密钥和hmac完整性（bash64编码）
        /// 随机密钥 用于敏感操作和对用户的信息进行Hmac鉴权
        /// 请务必注意的是：
        ///  1、Secret重置后，服务器端保存的加密数据是无法恢复
        ///  2、当启用加密硬件时，hmac和加密密钥由加密硬件
        /// </summary>
        public string Secret{get;set;}
        /// <summary>
        /// 服务器公钥（Hex格式）
        /// </summary>
        public string PublicKey{get;set;}

        /// <summary>
        /// 服务器私钥（Hex格式）
        /// 注意：不要泄露服务器私钥给任何人
        /// </summary>
        public string PrivateKey{get;set;}


        public string TransEncryptedPublicKey { get; set; }
        public string TransEncryptedPrivateKey { get; set; }



        /// <summary>
        /// 是否允许第三方注册
        /// 邀请注册 不受限制
        /// </summary>
        public bool OpenRegister{get;set;}=false;
        /// <summary>
        /// 是否允许开启demo体验账号
        /// demo账号允许匿名未知来宾使用你的服务 
        /// </summary>
        public bool OpenDemo{get;set;}=false;
        /// <summary>
        /// 允许发送实时数据和诊断数据到我们的服务器
        /// 以便我们进行故障遥测和分析
        /// 默认=false
        /// </summary>
        public bool ShareYourData{get;set;}=false;
        /// <summary>
        /// 指定某个用户成为超级管理员
        /// 程序首次初始化时 admin是超级管理员
        /// </summary>
        public string AdminUsername{ get;set;}="admin";
        public string DemoUsername { get; set; } = "demo";
        /// <summary>
        /// 将log放置在哪里
        /// </summary>
        public string LogFolder{ get;set;}
        /// <summary>
        /// Session有效期
        /// 也就是保持登录的有效期
        /// </summary>
        public int SessionExpires { get;set;}
        /// <summary>
        /// 网站紧急维护模式
        /// 当需要重置管理员密码的时候，需要打开紧急维护模式
        /// </summary>
        public bool MaintenanceMode { get;set;}=false;
        /// <summary>
        /// 密码加密算法
        /// </summary>
        public string PasswordHashAlgorithm { get;set;}= "argon2";
        /// <summary>
        /// 强制通信数字信封加密
        /// <para>
        ///  当此配置激活时，客户端必须强制实现数字信封<code>DigitalEnvelopeProtocol</code>
        ///  否则通信过程将被服务器拒绝
        /// </para>
        /// </summary>
        public bool ForceDigitalEnvelope { get; set; } = false;
    
        /// <summary>
        /// 数字信封协议
        /// <para>
        ///  默认SM2
        /// </para>
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DigitalEnvelopeProtocol ForceDigitalEnvelopeProtocol { get; set; } = DigitalEnvelopeProtocol.SM2SM3SM4;

        /// <summary>
        /// 强制数字签名
        /// </summary>
        public bool ForceDigitalSignature { get; set; } = false;
        /// <summary>
        /// 使用的数字签名协议
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DigitalSignatureProtocol ForceDigitalSignatureProtocol { get; set; } = DigitalSignatureProtocol.SM2;
        /// <summary>
        /// 密码加密时的迭代次数
        /// 迭代次数越大，计算越困难
        /// </summary>
        public int PasswordHashIterations { get;set;}= 8;
        /// <summary>
        /// 密码加密时的cpu线程限制 仅适用于Argon2id
        /// cpu核心x2
        /// </summary>
        public int PasswordStoreDegreeOfParallelism=8;
        /// <summary>
        /// 密码加密时的内存限制 仅适用于Argon2id
        /// 内存越大，计算越困难
        /// </summary>
        public int PasswordStoreMemorySize{ get;set;} =1024*2;
        /// <summary>
        /// 是否启用人机校验 验证码
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NeedVerificationCode NeedVerificationCode{get;set;}=NeedVerificationCode.ON;



        /// <summary>
        /// FIDO2认证协议配置
        /// </summary>
        public FIDO2Config FIDO2Config{get;set;}=new FIDO2Config();

        /// <summary>
        /// 人脸验证配置
        /// </summary>
        public FaceConfig FaceConfig { get;set;}=new FaceConfig();
        /// <summary>
        /// 需要对日志计算Hmac，防止被篡改
        /// </summary>
        public bool LogNeedHmac { get;set;}=false;
        /// <summary>
        /// 是否启用数据库加密
        /// </summary>
       

        public bool DataBaseEncryption { get; set; } = false;

        public string? DataBaseEncrypthonKey { get; set; }

        public string? DataBaseEncrypthonIV { get; set; }

        public bool NeedEncryptionMachine { get; set; }=false;
        public string NetSignApi { get;set;}= "http://NetSign:8081/";
        public string HisuTSSC { get;set;}= "http://HisuTSSC:8080/";
        public string HisuCCBC { get;set;}= "http://HisuCCBC:8082/";
        



    }
}
