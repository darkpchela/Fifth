using AutoMapper;
using Fifth.Interfaces;
using Fifth.Interfaces.DataAccess;
using Fifth.Services;
using Fifth.Services.DataContext;
using Fifth.Services.DataContext.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;

namespace Fifth.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutomapperProfiles(this IServiceCollection services, params Assembly[] otherMapperAssemblies)
        {
            List<Assembly> assemblies = new List<Assembly>(otherMapperAssemblies)
            {
                Assembly.GetExecutingAssembly()
            };
            services.AddAutoMapper(assemblies);
            return services;
        }

        public static IServiceCollection AddAppContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("Default")));
            services.AddSingleton<IGameProcessesContext, GameProcessesContext>();
            services.AddTransient<IGameProcessRepository, GameProcessRepository>();
            services.AddTransient<ISessionDataRepository, SessionDataRepository>();
            services.AddTransient<ISessionTagRepository, SessionTagRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddGameServices(this IServiceCollection services)
        {
            services.AddTransient<IGameProcessManager, GameProcessManager>();
            services.AddTransient<ITagsProvider, TagsProvider>();
            services.AddTransient<IGamesManager, GamesManager>();
            return services;
        }

        public static IServiceCollection AddAuthenticationService(this IServiceCollection services)
        {
            services.AddTransient<IAppAuthenticationService, AppAuthenticationService>();
            return services;
        }
    }
}