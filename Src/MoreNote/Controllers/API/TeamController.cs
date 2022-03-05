using Microsoft.AspNetCore.Mvc;

namespace MoreNote.Controllers.API
{
    public class TeamController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
