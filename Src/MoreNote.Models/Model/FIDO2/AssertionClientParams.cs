using Fido2NetLib;
using Fido2NetLib.Objects;

namespace MoreNote.Models.Model.FIDO2
{
	public class AssertionClientParams
	{
		//是否需要用户确认
		public UserVerificationRequirement? UserVerification { get; set; } = UserVerificationRequirement.Discouraged;

		public AuthenticatorSelection authenticatorSelection { get; set; } = new AuthenticatorSelection()
		{
			RequireResidentKey = false,
			UserVerification = UserVerificationRequirement.Preferred,
			//可选）指定要求的认证器类型。如果没有满足要求的认证器，认证可能会失败。该参数可以为 null（表示接受所有类型的认证器：
			AuthenticatorAttachment = null
		};

		public AuthenticationExtensionsClientInputs Extensions { get; set; } = new AuthenticationExtensionsClientInputs()
		{

			UserVerificationMethod = true
		};
	}
}