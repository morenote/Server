using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoreNote.API
{
    [Route("api/[controller]")]
    //[ApiController]
    public class FileController : ApiBaseController
    {
        public FileController(IHttpContextAccessor accessor) : base(accessor)
        {
        }

        //todo: 输出image
        public IActionResult GetImage()
        {
            return null;
        }
        //todo:下载附件
        public IActionResult GetAttach()
        {
            return null;
        }
        //todo:下载所有附件
        public IActionResult GetAllAttachs()
        {
            return null;
        }
      
    }
}