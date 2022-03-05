using Microsoft.AspNetCore.Mvc;

namespace MoreNote.Controllers.API
{
    public class NotesRepository : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
