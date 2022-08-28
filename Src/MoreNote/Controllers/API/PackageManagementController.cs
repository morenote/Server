using Microsoft.AspNetCore.Mvc;

namespace MoreNote.Controllers.API
{
    /// <summary>
    /// 环境依赖包管理
    /// </summary>
    public class PackageManagementController : Controller
    {
        public IActionResult Index()
        {
          return Content("");
        }
    }
}
