using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoreNote.Controllers
{
    public class ErrorsController1 : BaseController
    {
        public ErrorsController1(IHttpContextAccessor accessor) : base(accessor)
        {


        }
        public IActionResult Unauthorized1()
        {
            return Unauthorized();
            
        }

    }
}
