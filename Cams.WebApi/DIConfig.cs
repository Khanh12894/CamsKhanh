#region Using
using Microsoft.Extensions.DependencyInjection;
using XichLip.WebApi.Controllers;
using XichLip.WebApi.Interfaces;
using XichLip.WebApi.Repositories;
using WorkSimple.Infrastructure;
using WorkSimple.Infrastructure.Interfaces;
using XichLip.WebApi.Models.User;
using Microsoft.AspNetCore.Identity;
using WorkSimple.Infrastructure.Services;
using XichLip.WebApi.Models.Role;
using XichLip.WebApi.Interface;
#endregion Using

namespace XichLip.WebApi
{
    public class DIConfig
    {
        public static void Register(IServiceCollection services)
        {

            services.AddIdentity<UserModel, RoleModel>().AddErrorDescriber<WsIdentityErrorDescriber>();

            services.AddTransient<Audience>();
            services.AddTransient<IWSUnitOfWork, WSUnitOfWork>();
            services.AddTransient<WsLanguage<Resources.WsResource>>();
            services.AddTransient<IWSSecUnitOfWork, WSSecUnitOfWork>();
            services.AddTransient<IUserRepositories, UserRepositories>();
            services.AddTransient<IUserStore<UserModel>, WSUserStore>();
            services.AddTransient<IRoleStore<RoleModel>, WSRoleStore>();
            services.AddTransient<IUserPasswordStore<UserModel>, WSUserStore>();
            services.AddTransient<IUserTwoFactorStore<UserModel>, WSUserStore>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductTypeRepository, ProductTypeRepository>();
        }
    }
}


