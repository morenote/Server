using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Models.DTO.Joplin;
using MoreNote.Logic.Service;
using MoreNote.Models.Entity.Leanote.User;

using System.Net;
using System.Threading.Tasks;

namespace MoreNote.Controllers.Joplin
{
	/// <summary>
	/// Sesssion API
	/// </summary>
	public class JoplinSessionsController : JoplinBaseController
	{
		private AuthService AuthService { get; set; }

		public JoplinSessionsController(AttachService attachService
		  , TokenSerivce tokenSerivce
		  , NoteFileService noteFileService
		  , UserService userService
		  , ConfigFileService configFileService
		  , IHttpContextAccessor accessor
		  , AuthService authService
		  ) :
			base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
		{
			this.AuthService = authService;
		}

		[Route("api/sessions")]
		[HttpGet]
		public async Task<IActionResult> sessions([FromBody] SessionRequestDto sessionRequest)
		{
			string token = string.Empty;
			UserInfo user = null;
			var tokenStr = await AuthService.LoginByPWD(sessionRequest.email, sessionRequest.password);

			if (!string.IsNullOrEmpty(tokenStr))
			{
				var response = new SessionResponseDto
				{
					id = token,
					user_id = user.Id.ToHex()
				};
				return Json(response, MyJsonConvert.GetLeanoteOptions());
			}
			else
			{
				var response = new SessionResponseDto
				{
					error = "Invalid username or password"
				};
				//（禁止） 服务器拒绝请求。
				Response.StatusCode = (int)HttpStatusCode.Forbidden;
				return Json(response, MyJsonConvert.GetLeanoteOptions());
			}
		}
	}
}