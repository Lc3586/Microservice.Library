using Microservice.Library.Soap.Application;
using Microservice.Library.Soap.Filter;
using Microservice.Library.Soap.Gen;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SoapCore;
using SoapCore.Extensibility;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SoapServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Soap服务端
        /// </summary>
        /// <remarks>
        /// 示例：
        /// services.AddSoapServer();
        /// services.AddMvc();
        /// </remarks>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddSoapServer(
            this IServiceCollection services,
            params SoapServerOptions[] options)
        {
            services.AddSoapCore();

            bool messageInspector2Added = false;
            foreach (var server in options)
            {
                var serviceType = Assembly
                        .Load(server.ServiceType.Assembly)
                        .GetType(server.ServiceType.Type, true, true);

                var implementationType = Assembly
                        .Load(server.ImplementationType.Assembly)
                        .GetType(server.ImplementationType.Type, true, true);

                services.TryAddScoped(serviceType, implementationType);

                if (server.CustomResponse != null)
                {
                    if (!messageInspector2Added)
                    {
                        services.AddSingleton<IMessageInspector2, MessageInspector2>();
                        messageInspector2Added = true;
                    }

                    MessageInspector2.CustomResponses.Add(serviceType, (server.HttpContextGetter, server.CustomResponse));
                }
            }

            return services;
        }

        /// <summary>
        /// 添加Soap服务端
        /// </summary>
        /// <remarks>
        /// 示例：
        ///     app.AddSoapServer();
        ///     app.UseMvc();
        /// </remarks>
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

                app.UseSoapEndpoint(channelType, endpointOptions =>
                {
                    endpointOptions.Path = server.Path;
                    if (server.EncoderOptions != null)
                        endpointOptions.EncoderOptions = server.EncoderOptions;
                    endpointOptions.SoapSerializer = server.Serializer;

                    server.SetupSoapEndpoint?.Invoke(endpointOptions);
                });
            }

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
