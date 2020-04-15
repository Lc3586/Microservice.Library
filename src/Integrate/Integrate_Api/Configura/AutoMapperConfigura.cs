using Integrate_Business.Config;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integrate_Api.Configura
{
    public class AutoMapperConfigura
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddAutoMapper(s =>
            {
                s.AutoMapperGeneratorOptions.Types = SystemConfig.systemConfig.FxTypes;
            });
        }
    }
}
