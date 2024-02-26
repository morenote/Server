using Microsoft.AspNetCore.Http;

using MoreNote.Framework.Controllers;
using MoreNote.Logic.Service;
// ReSharper disable All
namespace MoreNote.Controllers.Member
{
	public class MemberBaseController : BaseController
	{
		public MemberBaseController(AttachService attachService
		  , TokenSerivce tokenSerivce
		  , NoteFileService noteFileService
		  , UserService userService
		  , ConfigFileService configFileService
		  , IHttpContextAccessor accessor

		   ) :
			base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
		{

		}
	}
}