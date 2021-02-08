using Microsoft.Extensions.DependencyInjection;
using MoreNote.Logic.DB;
using System;

namespace MoreNote.Logic.Service
{
    /// <summary>
    /// 依赖注入服务
    /// </summary>
     class DependencyInjectionService
    {
        private IServiceProvider ServiceProvider { get; set; }

        private DependencyInjectionService(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        private DataContext GetDataContext()
        {
                var scope = ServiceProvider.CreateScope();
                var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                return dataContext;
        }
        private IServiceScope GetServiceScope()
        {
            var scope = ServiceProvider.CreateScope();
            return scope;
        }

        private UserService GetUserService()
        {
            return ServiceProvider.GetRequiredService<UserService>();
        }

        private NoteImageService GetNoteImageService()
        {
            return ServiceProvider.GetRequiredService<NoteImageService>();
        }

        private NoteContentService GetNoteContentService()
        {
            return ServiceProvider.GetRequiredService<NoteContentService>();
        }

        private AttachService GetAttachService()
        {
            return ServiceProvider.GetRequiredService<AttachService>();
        }

        private CommonService GetCommonService()
        {
            return ServiceProvider.GetRequiredService<CommonService>();
        }

        private InitServices GetInitServices()
        {
            return ServiceProvider.GetRequiredService<InitServices>();
        }

        private NotebookService GetNotebookService()
        {
            return ServiceProvider.GetRequiredService<NotebookService>();
        }

        private TagService GetTagService()
        {
            return ServiceProvider.GetRequiredService<TagService>();
        }

        private TokenSerivce GetTokenSerivce()
        {
            return ServiceProvider.GetRequiredService<TokenSerivce>();
        }

        private NoteService GetNoteService()
        {
            return ServiceProvider.GetRequiredService<NoteService>();
        }

        private BlogService GetBlogService()
        {
            return ServiceProvider.GetRequiredService<BlogService>();
        }

        private ConfigService GetConfigService()
        {
            return ServiceProvider.GetRequiredService<ConfigService>();
        }

        private ConfigFileService GetConfigFileService()
        {
            return ServiceProvider.GetRequiredService<ConfigFileService>();
        }

        private EmailService GetEmailService()
        {
            return ServiceProvider.GetRequiredService<EmailService>();
        }
        private AccessService GetAccessService()
        {
            return ServiceProvider.GetRequiredService<AccessService>();;
        }
    }
}