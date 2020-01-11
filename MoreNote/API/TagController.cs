using Microsoft.AspNetCore.Mvc;

namespace MoreNote.LeaNoteAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        //todo:获取同步的标签
        public IActionResult GetSyncTags()
        {
            return null;
        }
        //todo:添加Tag
        public IActionResult AddTag()
        {
            return null;
        }
        //todo:删除标签
        public IActionResult DeleteTag()
        {
            return null;
        }
   

    }
}