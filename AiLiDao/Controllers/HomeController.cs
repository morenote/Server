using System.Diagnostics;

using AiLiDao.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AiLiDao.Controllers
{
    [Route("/{action=Anime}")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
       
        public IActionResult Music()
        {
            return View();
        }
        public IActionResult SingleMusicIndexc()
        {
            return View();
        }  public IActionResult SongList()
        {
            return View();
        }

        public IActionResult Anime()
        {
            return View();
        }
        public IActionResult AnimeIndex()
        {
            return View();
        }   
        
        public IActionResult Comic()
        {
            return View();
        }
        public IActionResult AcgVideo()
        {
            return View();
        }  
        public IActionResult PixivPicture()
        {
            return View();
        }
        public IActionResult CosPlay()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult VIP()
        {
            return View();
        }
        public IActionResult BBC()
        {
            return View();
        }

        public IActionResult PlayerV1()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
