using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Fido2NetLib;
using Fido2NetLib.Objects;

using Newtonsoft.Json.Converters;

namespace MoreNote.Models.Model.FIDO2
{

    /// <summary>
    /// 注册凭证参数
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public class MakeCredentialParams
    {

        /// <summary>
        /// 用户名称，这个是数据库中的完整名称
        /// </summary>
        public string Username { get; set; }

        public long? UserId { get;set;}
        /// <summary>
        /// attestation: String：表明依赖方是否需要证明。可选三个值：
        /// none：（默认）不需要证明。如上文所述，依赖方不关心证明，因此认证器不会签名。对于 iOS/iPad OS 13，必须设置为此值，否则验证将失败
        /// indirect：依赖方需要证明，但证明方式可由认证器选择。在支持匿名证明的认证器上，认证器会通过匿名证明的方式签名挑战，并向依赖方提供签名方式等信息
        /// direct：依赖方要求直接证明。此时认证器会使用烧录在认证器中的公钥进行签名，同时向依赖方提供签名方式等信息以供依赖方验证认证器是否可信。
        /// 如果你没有高安全需求（如银行交易等），请不要向认证器索取证明，即将 attestation 设置为 "none"。对于普通身份认证来说，要求证明不必要的，且会有浏览器提示打扰到用户。
        /// </summary>
        
        public AttestationConveyancePreference Attestation { get; set; }=AttestationConveyancePreference.None;

        /// <summary>
        ///  在 authenticatorSelection 中，我们还可以设置两个可选属性
        ///  authenticatorSelection.requireResidentKey: Boolean：是否要求将私钥钥永久存储于认证器中。默认值为 false。
        ///  对于 iOS/iPad OS 13，必须设置为 false，否则验证将失败
        ///  WebAuthn 扩展，可以提供规范之外的配置和响应,实际情况中很少会使用这一特性
        ///  将 requireResidentKey 设置为 true 可以实现无用户名的登录，即认证器同时替代了用户名和密码。
        ///  需要注意的是，尽管大部分认证器可以实现无限对公私钥，但能永久存储的私钥数量是有限的（对于 Yubikey，这通常是 25）
        ///  
        /// authenticatorSelection.userVerification  可选）指定认证器是否需要验证“用户为本人 (User Verified, UV)”，否则只须“用户在场 (User Present, UP)”。
        /// 具体验证过程取决于认证器（不同认证器的认证方法不同，也有认证器不支持用户验证），而对验证结果的处理情况则取决于依赖方。该参数可以为以下三个值之一：
        /// 
        /// required：依赖方要求用户验证
        /// preferred：（默认）依赖方希望有用户验证，但也接受用户在场的结果
        /// discouraged：依赖方不关心用户验证。对于 iOS/iPad OS 13，必须设置为此值，否则验证将失败
        /// </summary>
        public AuthenticatorSelection AuthenticatorSelection { get; set; }=new AuthenticatorSelection()
        {
            RequireResidentKey = false,
            UserVerification = UserVerificationRequirement.Preferred,
            //可选）指定要求的认证器类型。如果没有满足要求的认证器，认证可能会失败。该参数可以为 null（表示接受所有类型的认证器：
            AuthenticatorAttachment = null
        };


        public MakeCredentialParams(string UserName,long? UserId)
        {
            this.Username = UserName;
            this.UserId = UserId;
        }   

        public Fido2User GetFido2UserByUser()
        {
            var user = new Fido2User()
            {
                DisplayName = this.Username,
                Name = this.Username,
                Id=Encoding.UTF8.GetBytes(this.Username)
            };
            return user;
        }

    }
}