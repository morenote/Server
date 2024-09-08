using Microsoft.AspNetCore.Http;

using MoreNote.Framework.Controllers;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Notes;

namespace MoreNote.Controllers.Joplin
{
    /// <summary>
    /// 基础类
    /// </summary>
    public class JoplinBaseController : BaseController
	{
		public JoplinBaseController(AttachService attachService
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
