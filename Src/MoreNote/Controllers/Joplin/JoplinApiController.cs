using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.Utils;
using MoreNote.Logic.Models.DTO.Joplin;
using MoreNote.Logic.Service;

namespace MoreNote.Controllers.Joplin
{
	public class JoplinApiController : JoplinBaseController
	{
		private AuthService AuthService { get; set; }

		public JoplinApiController(AttachService attachService
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

		[Route("/api/items/root:/info.json:/content")]
		[HttpGet]
		public IActionResult GetInfoJson()
		{
			JoplinServerInfo joplinServerInfo = new JoplinServerInfo();
			return Json(joplinServerInfo, MyJsonConvert.GetSimpleOptions());
		}

		[Route("/api/items/root:/{**what}")]
		[HttpGet]
		public IActionResult GetItems(string what)
		{
			return Content(what);
		}

		[Route("/api/items/root:/{**what}")]
		[HttpDelete]
		public IActionResult DeleteItems(string what)
		{
			return Content(what);
		}

		[Route("/api/items/root:/{**what}")]
		[HttpPost]
		public IActionResult PostItems(string what)
		{
			return Content(what);
		}

	}
}