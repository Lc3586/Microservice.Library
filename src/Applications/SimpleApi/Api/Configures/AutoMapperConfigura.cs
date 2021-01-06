using Microsoft.Extensions.DependencyInjection;
using Model.System;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.TypeTool;

namespace Api.Configures
{
    /// <summary>
    /// AutoMapper配置类
    /// </summary>
    public class AutoMapperConfigura
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void RegisterServices(IServiceCollection services, SystemConfig config)
        {
            services.AddAutoMapper(s =>
            {
                s.AutoMapperGeneratorOptions.Types = TypeHelper.GetTypes(config.AutoMapperAssemblys.ToArray());
            });
        }
    }
}
