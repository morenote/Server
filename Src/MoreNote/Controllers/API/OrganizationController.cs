using Microsoft.AspNetCore.Mvc;

namespace MoreNote.Controllers.API
{
    /// <summary>
    /// 组织
    /// </summary>
    public class OrganizationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
