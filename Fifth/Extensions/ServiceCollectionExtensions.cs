using AutoMapper;
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
    }
}