using Microservice.Library.Extension;
using Microsoft.Extensions.DependencyInjection;
using Model.Utils.Config;

namespace Api.Configures
{
    /// <summary>
    /// AutoMapper配置类
    /// </summary>
    public static class AutoMapperConfigura
    {
        /// <summary>
        /// 注册AutoMapper服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static IServiceCollection RegisterAutoMapper(this IServiceCollection services, SystemConfig config)
        {
            services.AddAutoMapper(s =>
            {
                s.AutoMapperGeneratorOptions.Types = config.AutoMapperAssemblys.GetTypes();
            });

            return services;
        }
    }
}
