using Library.FreeSql.Application;
using Library.FreeSql.Gen;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FreeSqlServiceCollectionExtensions
    {
        public static IServiceCollection AddFreeSql(
            this IServiceCollection services,
            Action<FreeSqlGenOptions> setupAction = null)
        {
            services.AddTransient(setupAction);

            services.AddSingleton<IFreeSqlProvider, FreeSqlGenerator>();

            return services;
        }

        public static IServiceCollection AddFreeSql<TMark>(
            this IServiceCollection services,
            Action<FreeSqlGenOptions> setupAction = null)
        {
            services.AddTransient(setupAction);

            services.AddSingleton<IFreeSqlProvider, FreeSqlGenerator<TMark>>();

            return services;
        }

        private static void AddTransient(
            this IServiceCollection services,
            Action<FreeSqlGenOptions> setupAction = null)
        {
            //注册自定义配置程序，将高级配置（<FreeSqlGen）应用于低级配置（FreeSqlGeneratorOptions,FreeSqlDevOptions,FreeSqlDbContextOptions）。
            services.AddTransient<IConfigureOptions<FreeSqlGeneratorOptions>, ConfigureFreeSqlGeneratorOptions>();
            services.AddTransient<IConfigureOptions<FreeSqlDevOptions>, ConfigureFreeSqlDevOptions>();
            services.AddTransient<IConfigureOptions<FreeSqlDbContextOptions>, ConfigureFreeSqlDbContextOptions>();
            //注册生成器和依赖
            services.AddTransient(s => s.GetRequiredService<IOptions<FreeSqlGenOptions>>().Value);

            if (setupAction != null) services.ConfigureFreeSql(setupAction);
        }

        public static void ConfigureFreeSql(
            this IServiceCollection services,
            Action<FreeSqlGenOptions> setupAction)
        {
            services.Configure(setupAction);
        }
    }
}
