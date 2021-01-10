using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Logic.Service;

namespace MoreNote.Controllers
{
    public class ErrorsController1 : BaseController
    {
        public ErrorsController1(DependencyInjectionService dependencyInjectionService) : base(dependencyInjectionService)
        {
           

        }
        public IActionResult Unauthorized1()
        {
            return Unauthorized();
            
        }

    }
}
