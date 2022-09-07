using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Controllers.API.APIV1;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.MyOrganization;
using MoreNote.Logic.Service.MyRepository;
using MoreNote.Logic.Service.Security.USBKey.CSP;
using MoreNote.Models.DTO.Leanote.USBKey;
using MoreNote.Models.Entity.Leanote;
using MoreNote.Models.Enum;

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoreNote.Controllers.API
{
    /// <summary>
    /// 笔记仓库
    /// </summary>

    [Route("api/NotesRepository/[action]")]
    public class NotesRepositoryController : APIBaseController
    {
        private NotebookService notebookService;
        private NoteRepositoryService noteRepositoryService;
        private OrganizationService organizationService;
        private EPassService ePassService;
        private DataSignService dataSignService;

        public NotesRepositoryController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor,
            NotebookService notebookService,
            NoteRepositoryService noteRepositoryService,
             OrganizationService organizationService,
               EPassService ePassService,
               DataSignService dataSignService
           ) :
         base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.notebookService = notebookService;
            this.noteRepositoryService = noteRepositoryService;
            this.ePassService = ePassService;
            this.dataSignService = dataSignService;
        }

        [HttpGet]
        public IActionResult GetMyNoteRepository(string userId, string token)
        {
            var apiRe = new ApiRe()
            {
                Ok = false,
                Data = null
            };
            var user = tokenSerivce.GetUserByToken(token);
            if (user != null)
            {
                var rep = noteRepositoryService.GetNoteRepositoryList(user.Id);
                apiRe = new ApiRe()
                {
                    Ok = true,
                    Data = rep
                };
            }
            apiRe.Msg = "";
            return LeanoteJson(apiRe);
        }


        [HttpPost]
        public async Task<IActionResult> CreateNoteRepository(string token, string data, string dataSignJson)
        {
            var re = new ApiRe()
            {
                Ok = false,
                Data = null
            };
            var verify = false;
            if (this.config.SecurityConfig.ForceDigitalSignature)
            {
                //验证签名
                var dataSign = DataSignDTO.FromJSON(dataSignJson);
                verify = await this.ePassService.VerifyDataSign(dataSign);
                if (!verify)
                {
                    return LeanoteJson(re);
                }
                verify = dataSign.SignData.Operate.Equals("/api/NotesRepository/CreateNoteRepository");
                if (!verify)
                {
                    re.Msg = "Operate is not Equals ";
                    return LeanoteJson(re);
                }
                //签名存证
                this.dataSignService.AddDataSign(dataSign, "CreateNoteRepository");
            }

            var user = tokenSerivce.GetUserByToken(token);
            var notesRepository = JsonSerializer.Deserialize<NotesRepository>(data, MyJsonConvert.GetLeanoteOptions());
            if (notesRepository.RepositoryOwnerType == RepositoryOwnerType.Organization)
            {
                var orgId = notesRepository.OwnerId;
                verify = organizationService.Verify(orgId, user.Id, OrganizationAuthorityEnum.AddRepository);
                if (verify == false)
                {
                    re.Msg = "您没有权限创建这个仓库";

                    return LeanoteJson(re);
                }
            }
            if (notesRepository.RepositoryOwnerType == RepositoryOwnerType.Personal)
            {
                if (notesRepository.OwnerId != user.Id)
                {
                    re.Msg = "您没有权限创建这个仓库";
                    return LeanoteJson(re);
                }
            }
            //if (!MyStringUtil.IsNumAndEnCh(notesRepository.Name))
            //{
            //    apiRe.Msg = "仓库路径仅允许使用英文大小写、数字，不允许特殊符号";
            //    return LeanoteJson(apiRe);
            //}
            if (noteRepositoryService.ExistNoteRepositoryByName(notesRepository.OwnerId, notesRepository.Name))
            {
                re.Msg = "仓库名称冲突";
                return LeanoteJson(re);
            }

            var result = noteRepositoryService.CreateNoteRepository(notesRepository);

            var list = new List<string>(4) { "life", "study", "work", "tutorial" };
            foreach (var item in list)
            {
                // 添加笔记本, 生活, 学习, 工作
                var userId = user.Id;
                var notebook = new Notebook()
                {
                    Id = idGenerator.NextId(),
                    NotesRepositoryId = result.Id,
                    Seq = 0,
                    UserId = userId,
                    CreatedTime = DateTime.Now,
                    Title = item,
                    ParentNotebookId = null,
                };
                notebookService.AddNotebook(notebook);
            }

            if (result == null)
            {
                re.Msg = "数据库创建仓库失败";
                return LeanoteJson(re);
            }
            re.Ok = true;
            re.Data = result;
            return LeanoteJson(re);
        }

        /// <summary>
        /// 删除仓库
        /// </summary>
        /// <param name="token"></param>
        /// <param name="noteRepositoryId"></param>
        /// <returns></returns>
        [HttpPost,HttpDelete]
        public async Task<IActionResult> DeleteNoteRepository(string token, string noteRepositoryId, string dataSignJson)
        {
            var verify = false;
            var user = tokenSerivce.GetUserByToken(token);
            var re = new ApiRe()
            {
                Ok = false,
                Data = null
            };
            if (user == null)
            {
                return LeanoteJson(re);
            }

            if (this.config.SecurityConfig.ForceDigitalSignature)
            {
                //验证签名
                var dataSign = DataSignDTO.FromJSON(dataSignJson);
                verify = await this.ePassService.VerifyDataSign(dataSign);
                if (!verify)
                {
                    return LeanoteJson(re);
                }
                verify = dataSign.SignData.Operate.Equals("/api/NotesRepository/DeleteNoteRepository");
                if (!verify)
                {
                    re.Msg = "Operate is not Equals ";
                    return LeanoteJson(re);
                }
                //签名存证
                this.dataSignService.AddDataSign(dataSign, "DeleteNoteRepository");
            }

            verify = noteRepositoryService.Verify(noteRepositoryId.ToLongByHex(), user.Id, RepositoryAuthorityEnum.DeleteRepository);
            if (!verify)
            {
                return LeanoteJson(re);
            }

            this.noteRepositoryService.DeleteNoteRepository(noteRepositoryId.ToLongByHex());
            re.Ok = true;
            return LeanoteJson(re);
        }
    }
}