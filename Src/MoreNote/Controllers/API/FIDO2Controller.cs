using System.Collections.Generic;

using Fido2NetLib;
using Fido2NetLib.Objects;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoreNote.Controllers.API
{
    public class FIDO2Controller : Controller
    {
        public IActionResult CreateAttestationOptions()
        {
            
            return View();
        }

        public IActionResult RegisterCredentials()
        {
            return View();
        }

        public IActionResult CreateAssertionOptions()
        {
            return View();
        }

        public IActionResult VerifyTheAssertionResponse()
        {
            return View();
        }
    }
}