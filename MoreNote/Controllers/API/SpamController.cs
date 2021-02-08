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

namespace MoreNote.Controllers.API
{
    [Route("api/Spam/[action]")]
    public class SpamController : BaseAPIController
    {
        private SpamService spamService;
        public SpamController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
           spamService=dependencyInjectionService.ServiceProvider.GetService(typeof(SpamService))as SpamService;
           

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
