using Microservice.Library.SuperSocket.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加SuperSocket服务
        /// </summary>
        /// <typeparam name="TPackageInfo">消息包类型</typeparam>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddSuperSocket<TPackageInfo>(
            this IServiceCollection services,
            Action<SuperSocketGenOptions<TPackageInfo>> setupAction = null) where TPackageInfo : class
        {
            //注册生成器和依赖
            services.AddTransient(s => s.GetRequiredService<IOptions<SuperSocketGenOptions<TPackageInfo>>>().Value);

            services.AddSingleton<ISuperSocketProvider<TPackageInfo>, SuperSocketGenerator<TPackageInfo>>();

            if (setupAction != null) services.ConfigureSuperSocket(setupAction);

            return services;
        }

        /// <summary>
        /// 配置SuperSocket服务
        /// </summary>
        /// <typeparam name="TPackageInfo">消息包类型</typeparam>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        public static void ConfigureSuperSocket<TPackageInfo>(
            this IServiceCollection services,
            Action<SuperSocketGenOptions<TPackageInfo>> setupAction) where TPackageInfo : class
        {
            services.Configure(setupAction);
        }
    }
}
