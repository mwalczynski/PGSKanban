using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PgsKanban.Api.Attributes;
using PgsKanban.BusinessLogic.Implementation;
using PgsKanban.BusinessLogic.Interfaces;
using PgsKanban.BusinessLogic.Managers;
using PgsKanban.BusinessLogic.Services;
using PgsKanban.BusinessLogic.Services.ReCaptcha;
using PgsKanban.BusinessLogic.Services.ReCaptcha.Interfaces;
using PgsKanban.DataAccess.Implementation;
using PgsKanban.DataAccess.Interfaces;
using PgsKanban.DataAccess.Models;
using PgsKanban.Import;

namespace PgsKanban.Api.Config
{
    public class DependencyRegistration
    {
        public static AutofacServiceProvider Register(IConfigurationRoot configuration, IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.Register(_ => configuration).As<IConfigurationRoot>().SingleInstance();
            builder.RegisterType<UserManager<User>>();
            builder.RegisterType<PasswordHasher<User>>();
            builder.RegisterType<UserStore<User>>().As<IUserStore<User>>();
            builder.RegisterType<DisableIfNoLocalProviderAvailable>();
            builder.RegisterType<Obfuscation>().As<IObfuscator>();
            
            RegisterRepositories(builder);
            RegisterServices(builder);

            builder.Populate(services);
            return new AutofacServiceProvider(builder.Build());
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<AuthService>().As<IAuthService>();
            builder.RegisterType<ConfirmAccountService>().As<IConfirmAccountService>();
            builder.RegisterType<ChangePasswordService>().As<IChangePasswordService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<BoardService>().As<IBoardService>();
            builder.RegisterType<ListService>().As<IListService>();
            builder.RegisterType<ImportService>().As<IImportService>();
            builder.RegisterType<ExternalLoginService>().As<IExternalLoginService>();
            builder.RegisterType<ExternalLoginProviderManager>().As<IExternalLoginProviderManager>();
            builder.RegisterType<CardService>().As<ICardService>();
            builder.RegisterType<CacheService>().As<ICacheService>();
            builder.RegisterType<ReCaptchaValidation>().As<IReCaptchaValidation>();
            builder.RegisterType<MessageSender>().As<IMessageSender>();
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<BoardRepository>().As<IBoardRepository>();
            builder.RegisterType<UserBoardRepository>().As<IUserBoardRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<ListRepository>().As<IListRepository>();
            builder.RegisterType<CardRepository>().As<ICardRepository>();
            builder.RegisterType<CommentRepository>().As<ICommentRepository>();

        }
    }
}
