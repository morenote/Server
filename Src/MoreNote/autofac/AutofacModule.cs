using Autofac;
using Autofac.Extras.NLog;

using github.hyfree.GM;

using Microsoft.AspNetCore.Mvc;

using Morenote.Framework.Filter.Global;

using MoreNote.AutoFac;
using MoreNote.Common.Utils;
using MoreNote.CryptographyProvider;
using MoreNote.CryptographyProvider.EncryptionMachine.StandardCyptographicDevice;
using MoreNote.Logic.Property;
using MoreNote.Logic.Security.FIDO2.Service;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.BlogBuilder;
using MoreNote.Logic.Service.BlogBuilder.VuePress;
using MoreNote.Logic.Service.Captcha.IMPL;
using MoreNote.Logic.Service.DistributedIDGenerator;
using MoreNote.Logic.Service.Logging;
using MoreNote.Logic.Service.Logging.IMPL;
using MoreNote.Logic.Service.MyOrganization;
using MoreNote.Logic.Service.MyRepository;
using MoreNote.Logic.Service.PasswordSecurity;
using MoreNote.Logic.Service.Security.USBKey.CSP;
using MoreNote.Logic.Service.Segmenter;
using MoreNote.Logic.Service.VerificationCode;
using MoreNote.SignatureService;
using MoreNote.SignatureService.NetSign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebApiClient.Extensions.Autofac;

namespace MoreNote.Common.autofac
{
    /// <summary>
    /// 容器注册类
    /// </summary>
    public class AutofacModule : Autofac.Module
    {
        public static ContainerBuilder builder { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            //依赖注入的对象
            builder.RegisterType<AccessService>();
            builder.RegisterType<AlbumService>();
            builder.RegisterType<APPStoreInfoService>();
            builder.RegisterType<AttachService>()
                .OnActivated(e => e.Instance.NoteService = e.Context.Resolve<NoteService>())
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<AuthService>().OnActivated(e =>
            {
                var authService = e.Instance;
                authService.UserService = e.Context.Resolve<UserService>();
                authService.TokenSerivce = e.Context.Resolve<TokenSerivce>();
            });
            builder.RegisterType<BlogService>()
                .OnActivated(e =>
                {
                    var blogService = e.Instance;
                    blogService.NoteService = e.Context.Resolve<NoteService>();
                    blogService.NoteContentService = e.Context.Resolve<NoteContentService>();
                    blogService.UserService = e.Context.Resolve<UserService>();
                    blogService.ConfigService = e.Context.Resolve<ConfigService>();
                    blogService.CommonService = e.Context.Resolve<CommonService>();
                })
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<CommonService>();
            builder.RegisterType<ConfigFileService>().SingleInstance();//单例模式 配置文件服务
            builder.RegisterType<ConfigService>();
            builder.RegisterType<EmailService>();
            builder.RegisterType<GoogleAuthenticatorService>();
            builder.RegisterType<GroupService>();
            builder.RegisterType<InitServices>();
            builder.RegisterType<NotebookService>().OnActivated(e =>
            {
                var instance = e.Instance;
                instance.UserService = e.Context.Resolve<UserService>();
                instance.NoteService = e.Context.Resolve<NoteService>();
            })
            .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<NoteContentHistoryService>();
            builder.RegisterType<NoteContentService>().OnActivated(e =>
            {
                var instance = e.Instance;
                instance.NoteImageService = e.Context.Resolve<NoteImageService>();
                instance.NoteService = e.Context.Resolve<NoteService>();
            });
            builder.RegisterType<NoteFileService>();
            builder.RegisterType<NoteImageService>();
            builder.RegisterType<NoteService>()
                .OnActivated(e =>
                {
                    var instance = e.Instance;
                    instance.ConfigService = e.Context.Resolve<ConfigService>();
                    instance.BlogService = e.Context.Resolve<BlogService>();
                    instance.NoteImageService = e.Context.Resolve<NoteImageService>();
                    instance.NoteImageService = e.Context.Resolve<NoteImageService>();
                    instance.AttachService = e.Context.Resolve<AttachService>();
                    instance.CommonService = e.Context.Resolve<CommonService>();
                    instance.UserService = e.Context.Resolve<UserService>();
                    instance.InitServices = e.Context.Resolve<InitServices>();
                    instance.NotebookService = e.Context.Resolve<NotebookService>();
                    instance.TagService = e.Context.Resolve<TagService>();
                    instance.NoteContentService = e.Context.Resolve<NoteContentService>();
                    instance.ShareService = e.Context.Resolve<ShareService>();
                })
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<PwdService>();

            builder.RegisterType<SessionService>();
            builder.RegisterType<ShareService>();
            builder.RegisterType<SpamService>().OnActivated(e =>
            {
                var instance = e.Instance;
                instance.ConfigFileService = e.Context.Resolve<ConfigFileService>();
            });
            builder.RegisterType<SuggestionService>();
            builder.RegisterType<TagService>().OnActivated(e =>
            {
                var instance = e.Instance;
                instance.NoteService = e.Context.Resolve<NoteService>();
                instance.UserService = e.Context.Resolve<UserService>();
            });
            builder.RegisterType<ThemeService>();
            builder.RegisterType<TokenSerivce>();
            builder.RegisterType<TrashService>().OnActivated(e =>
            {
                var instance = e.Instance;
                instance.noteService = e.Context.Resolve<NoteService>();
                instance.AttachService = e.Context.Resolve<AttachService>();
                instance.NoteContentService = e.Context.Resolve<NoteContentService>();
                instance.NotebookService = e.Context.Resolve<NotebookService>();
            });
            builder.RegisterType<UpgradeService>();

            builder.RegisterType<UserService>()
                   .OnActivated(e =>
                    {
                        var instance = e.Instance;
                        instance.BlogService = e.Context.Resolve<BlogService>();
                        instance.EmailService = e.Context.Resolve<EmailService>();
                    })
                    .InstancePerLifetimeScope()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<SHA256PasswordStore>()
                .As<IPasswordStore>();
            builder.RegisterType<JiebaSegmenterService>()
                .As<JiebaSegmenterService>();
            //==============================================
            builder.RegisterType<OrganizationMemberRoleService>();
            builder.RegisterType<OrganizationService>();
            builder.RegisterType<OrganizationTeamService>();

            builder.RegisterType<NoteRepositoryService>();
            builder.RegisterType<RepositoryMemberRoleService>();

            builder.RegisterType<PasswordStoreFactory>();

            //fido2认证服务
            builder.RegisterType<FIDO2Service>();
            //过滤器
            builder.RegisterType<CheckLoginFilter>();
            builder.RegisterType<CheckTokenFilter>();
            //注入日志服务日志

            builder.RegisterModule<NLogModule>();

            builder.RegisterType<LoggingService>().As<ILoggingService>();

            //分布式id生成器
            builder.RegisterType<SnowFlakeNetService>()
                .As<IDistributedIdGenerator>()
                .SingleInstance();
            ConfigFileService configFileService = new ConfigFileService();
            var config = configFileService.WebConfig;
            if (config.SecurityConfig.NeedEncryptionMachine)
            {
                //签名验签服务器
                builder.RegisterHttpApi<INetSignApi>().ConfigureHttpApiConfig(api =>
                {
                    api.HttpHost = new Uri(config.SecurityConfig.NetSignApi);
                });
                //服务器端签名和验签服务
                builder.RegisterType<NetSignService>()
                    .As<ISignatureService>()
                    .SingleInstance();
                //加密提供服务
                builder.RegisterType<SDFProvider>()
                   .As<ICryptographyProvider>()
                   .SingleInstance();

            }
            else
            {
                //服务器端签名和验签服务
                builder.RegisterType<FakeSignatureService>()
                    .As<ISignatureService>()
                    .SingleInstance();
                //加密提供服务
                builder.RegisterType<FakeCryptographyProvider>()
                   .As<ICryptographyProvider>()
                   .SingleInstance();

            }

            builder.RegisterType<EPassService>();

            builder.RegisterType<ImageSharpCaptchaGenerator>()
                .As<ICaptchaGenerator>();
            //实名认证服务
            builder.RegisterType<RealNameService>();
            builder.RegisterType<DataSignService>();
            builder.RegisterType<GMService>();
            //属性注入
            var controllerBaseType = typeof(ControllerBase);

            //注册VuePress生成器
            builder.RegisterType<VuePressBuilder>().As<BlogBuilderInterface>().Keyed<BlogBuilderInterface>(BlogBuilderType.VuePress);

            //批量扫描
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired(new AutowiredPropertySelector());

            builder.RegisterBuildCallback(container =>
            {
                AutoFacHelper.Container = (IContainer)container;
            });
        }
    }
}