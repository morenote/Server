
using MoreNote.Config.ConfigFile;
using MoreNote.Models.Enums;

using System.Text.Json.Serialization;

namespace MoreNote.Models.DTO.Leanote.Config
{
	public class SecurityConfigDTO
	{
		/// <summary>
		/// 服务器公钥（Hex格式）
		/// </summary>
		public string PublicKey { get; set; }
		/// <summary>
		/// 是否传输报文加密
		/// </summary>
		public string TransEncryptedPublicKey { get; set; }

		/// <summary>
		/// 是否允许第三方注册
		/// 邀请注册 不受限制
		/// </summary>
		public bool OpenRegister { get; set; } = false;
		/// <summary>
		/// 是否允许开启demo体验账号
		/// demo账号允许匿名未知来宾使用你的服务 
		/// </summary>
		public bool OpenDemo { get; set; } = false;
		/// <summary>
		/// 允许发送实时数据和诊断数据到我们的服务器
		/// 以便我们进行故障遥测和分析
		/// 默认=false
		/// </summary>
		public bool ShareYourData { get; set; } = false;

		/// <summary>
		/// 密码加密算法
		/// </summary>
		public string PasswordHashAlgorithm { get; set; }
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
		public DigitalEnvelopeProtocol ForceDigitalEnvelopeProtocol { get; set; }


		public bool ForceDigitalSignature { get; set; }
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public DigitalSignatureProtocol ForceDigitalSignatureProtocol { get; set; } = DigitalSignatureProtocol.SM2;

		/// <summary>
		/// 是否启用人机校验 验证码
		/// </summary>
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public NeedVerificationCode NeedVerificationCode { get; set; }
		/// <summary>
		/// 是否启用浏览器水印，默认值false
		/// </summary>
		public bool OpenWatermark { get; set; }

		public static SecurityConfigDTO Instance(SecurityConfig securityConfig)
		{
			SecurityConfigDTO dto = new SecurityConfigDTO()
			{
				PublicKey = securityConfig.PublicKey,
				TransEncryptedPublicKey = securityConfig.TransEncryptedPublicKey,
				OpenRegister = securityConfig.OpenRegister,
				OpenDemo = securityConfig.OpenDemo,
				ShareYourData = securityConfig.ShareYourData,
				PasswordHashAlgorithm = securityConfig.PasswordHashAlgorithm,
				ForceDigitalEnvelope = securityConfig.ForceDigitalEnvelope,
				ForceDigitalEnvelopeProtocol = securityConfig.ForceDigitalEnvelopeProtocol,
				ForceDigitalSignature = securityConfig.ForceDigitalSignature,
				ForceDigitalSignatureProtocol = securityConfig.ForceDigitalSignatureProtocol,
				NeedVerificationCode = securityConfig.NeedVerificationCode,
				OpenWatermark = securityConfig.OpenWatermark
			};
			return dto;

		}

	}
}
