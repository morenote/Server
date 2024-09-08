using Microsoft.AspNetCore.Http;

using MoreNote.Framework.Controllers;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Notes;
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