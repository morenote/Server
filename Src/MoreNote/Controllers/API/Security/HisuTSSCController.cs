using Microsoft.AspNetCore.Mvc;
using MoreNote.Logic.Property;
using MoreNote.Logic.Service.Logging;
using MoreNote.SignatureService;
using System.Threading.Tasks;

namespace MoreNote.Controllers.API.Security
{
    [Route("api/HisuTSSC/[action]")]
    public class HisuTSSCController : Controller
    {
        ISignatureService signatureService;

        [Autowired]
        ILoggingService loggingService { get; set; }
       public HisuTSSCController(ISignatureService signatureService)
        {
            this.signatureService = signatureService;

        }
        [HttpGet]
        public async Task<IActionResult> GenLog()
        {
            loggingService.Info("hello world");
            return Content("log生成测试");
        }
    }
}
