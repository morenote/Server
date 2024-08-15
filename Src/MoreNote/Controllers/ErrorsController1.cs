using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Framework.Controllers;
using MoreNote.Logic.Service;

namespace MoreNote.Controllers
{
	public class ErrorsController1 : BaseController
	{
		public ErrorsController1(AttachService attachService
			, TokenSerivce tokenSerivce
			, NoteFileService noteFileService
			, UserService userService
			, ConfigFileService configFileService
			, IHttpContextAccessor accessor
			 ) :
			base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
		{


		}
		[HttpGet]
		public IActionResult Unauthorized1()
		{
			return Unauthorized();

		}

	}
}
