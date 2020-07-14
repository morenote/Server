using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;

namespace MoreNote.Controllers.API
{
    [Route("api/Spam/[action]")]
    public class SpamController : Controller
    {
        public IActionResult Predict(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("error");
            }
            var modelOutput= SpamService.Predict(input);
            var spam = new SpamInfo()
            {
                SpamId = SnowFlakeNet.GenerateSnowFlakeID(),
                Input = input,
                Prediction = modelOutput.Prediction,
                Score = modelOutput.Score,
                ManualCheck = false,
                ManualResult = false
            };
            SpamService.AddSpamInfo(spam);
            return Json(modelOutput,MyJsonConvert.GetSimpleOptions());
        }
    }
}
