using Microsoft.Extensions.DependencyInjection;
using MoreNote.Logic.DB;
using System;

namespace MoreNote.Logic.Service
{
    /// <summary>
    /// 依赖注入服务
    /// </summary>
    public  class DependencyInjectionService
    {
        public IServiceProvider ServiceProvider { get; set; }

        public DependencyInjectionService(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

    
        public IServiceScope GetServiceScope()
        {
            var scope = ServiceProvider.CreateScope();
            return scope;
        }

        public UserService GetUserService()
        {
            return ServiceProvider.GetRequiredService<UserService>();
        }

        public NoteImageService GetNoteImageService()
        {
            return ServiceProvider.GetRequiredService<NoteImageService>();
        }

        public NoteContentService GetNoteContentService()
        {
            return ServiceProvider.GetRequiredService<NoteContentService>();
        }

        public AttachService GetAttachService()
        {
            return ServiceProvider.GetRequiredService<AttachService>();
        }

        public CommonService GetCommonService()
        {
            return ServiceProvider.GetRequiredService<CommonService>();
        }

        public InitServices GetInitServices()
        {
            return ServiceProvider.GetRequiredService<InitServices>();
        }

        public NotebookService GetNotebookService()
        {
            return ServiceProvider.GetRequiredService<NotebookService>();
        }

        public TagService GetTagService()
        {
            return ServiceProvider.GetRequiredService<TagService>();
        }

        public TokenSerivce GetTokenSerivce()
        {
            return ServiceProvider.GetRequiredService<TokenSerivce>();
        }

        public NoteService GetNoteService()
        {
            return ServiceProvider.GetRequiredService<NoteService>();
        }

        public BlogService GetBlogService()
        {
            return ServiceProvider.GetRequiredService<BlogService>();
        }

        public ConfigService GetConfigService()
        {
            return ServiceProvider.GetRequiredService<ConfigService>();
        }

        public ConfigFileService GetConfigFileService()
        {
            return ServiceProvider.GetRequiredService<ConfigFileService>();
        }

        public EmailService GetEmailService()
        {
            return ServiceProvider.GetRequiredService<EmailService>();
        }
        public AccessService GetAccessService()
        {
            return ServiceProvider.GetRequiredService<AccessService>();;
        }
    }
}