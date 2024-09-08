using Microsoft.AspNetCore.Http;

using MoreNote.Framework.Controllers;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Notes;

namespace MoreNote.Controllers.API.APIV1
{
    /**
     * 源代码基本是从GO代码直接复制过来的
     *
     * 只是简单的实现了API的功能
     *
     * 2020年01月27日
     * */

    public class APIBaseController : BaseController
	{

		
		public APIBaseController(AttachService attachService
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