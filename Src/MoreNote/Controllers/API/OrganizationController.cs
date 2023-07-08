using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Controllers.API.APIV1;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;
using MoreNote.Logic.Service.MyOrganization;
using MoreNote.Logic.Service.MyRepository;
using MoreNote.Models.DTO.Leanote;
using MoreNote.Models.Enum;

namespace MoreNote.Controllers.API
{
    /// <summary>
    /// 组织
    /// </summary>
    [Route("api/Organization/[action]")]
    public class OrganizationController : APIBaseController
    {
        OrganizationService organizationService;
        public OrganizationController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor,
            NotebookService notebookService,
            OrganizationService organizationService,
            RepositoryService noteRepositoryService
           ) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.organizationService = organizationService;
        }
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult GetOrganizationListByAuthorityEnum(string token, OrganizationAuthorityEnum AuthorityEnum)
        {
            
            var apiRe = new ApiResponseDTO()
            {
                Ok = false,
                Data = null
            };
            var user = tokenSerivce.GetUserByToken(token);
            if (user != null)
            {
                var list = organizationService.GetOrganizationList(user.Id, AuthorityEnum);
              
                apiRe = new ApiResponseDTO()
                {
                    Ok = true,
                    Data = list
                };
            }
            apiRe.Msg = "";
            return SimpleJson(apiRe);
        }

    }
}
