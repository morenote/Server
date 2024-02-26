namespace MoreNote.Models.Enums
{
	/// <summary>
	/// 登录安全策略级别
	/// </summary>
	public enum LoginSecurityPolicyLevel
	{
		//U2F=人脸验证、谷歌动态令牌、安全问题
		//密码设备=FIDO2、安全令牌

		unlimited = 0,//无限制，可以使用单一因子登录
		loose = 1,//宽松，最近登录并且信任的设备使用单一因子(口令或密码设备)登录;首次登录设备时，使用双因素。
		strict = 2,//严格，无论任何环境必须使用口令+U2F、口令+FIDO2、口令+智能密码钥匙三种方式的任意一种方式
		compliant = 3//合规，必须使用口令+GM智能密码钥匙的组合，提供最高级别的安全保护
	}
}
