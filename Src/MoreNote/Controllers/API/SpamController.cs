using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.Utils;
using MoreNote.Controllers.API.APIV1;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;

namespace MoreNote.Controllers.API
{
    [Route("api/Spam/[action]")]
    public class SpamController : APIBaseController
    {
        private SpamService spamService;
        public SpamController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor,
            SpamService spamService
           , ILoggingService loggingService) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor, loggingService)
        {
          this.spamService= spamService;
           

        }
        public IActionResult Predict(string input)
        {
         
            if (string.IsNullOrEmpty(input))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("error");
            }
            var modelOutput = spamService.Predict(input);
            var spam = new SpamInfo()
            {
                SpamId = SnowFlakeNet.GenerateSnowFlakeID(),
                Input = input,
                Prediction = modelOutput.Prediction,
                Score = modelOutput.Score,
                ManualCheck = false,
                ManualResult = false,
                CreatData = DateTime.Now
            };
            spamService.AddSpamInfo(spam);
            return Json(modelOutput, MyJsonConvert.GetSimpleOptions());
        }
    }
}
