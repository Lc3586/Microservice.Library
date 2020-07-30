using Library.Soap;
using Library.Soap.Application;
using Library.Soap.Gen;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SoapCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SoapServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Soap服务端
        /// <para>还需要调用app.AddSoapServer()</para>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddSoapServer(
            this IServiceCollection services,
            params SoapServerOptions[] options)
        {
            foreach (var server in options)
            {
                services.TryAddScoped(
                    Assembly
                        .Load(server.ServiceType.Assembly)
                        .GetType(server.ServiceType.Type, true, true),
                    Assembly
                        .Load(server.ImplementationType.Assembly)
                        .GetType(server.ImplementationType.Type, true, true));
            }

            services.AddMvc(x => x.EnableEndpointRouting = false);
            services.AddSoapCore();

            return services;
        }

        /// <summary>
        /// 添加Soap服务端
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IApplicationBuilder AddSoapServer(
            this IApplicationBuilder app,
            params SoapServerOptions[] options)
        {
            foreach (var server in options)
            {
                var channelType = Assembly
                                        .Load(server.ServiceType.Assembly)
                                        .GetType(server.ServiceType.Type, true, true);

                app.UseSoapEndpoint(channelType, server.Path, new BasicHttpBinding(), server.Serializer);
            }

            app.UseMvc();

            return app;
        }

        /// <summary>
        /// 注册Soap客户端
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddSoapClient(
            this IServiceCollection services,
            Action<SaopClientGeneratorOptions> setupAction = null)
        {
            //注册生成器和依赖
            services.AddTransient(s => s.GetRequiredService<IOptions<SaopClientGeneratorOptions>>().Value);
            services.AddTransient<ISoapClientProvider, SoapClientGenerator>();

            if (setupAction != null) services.ConfigureSoapClient(setupAction);

            services.AddSoapCore();
            return services;
        }

        /// <summary>
        /// 配置Soap客户端
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        public static void ConfigureSoapClient(
            this IServiceCollection services,
            Action<SaopClientGeneratorOptions> setupAction)
        {
            services.Configure(setupAction);
        }
    }
}
