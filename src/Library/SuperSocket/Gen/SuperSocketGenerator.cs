using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SuperSocket;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microservice.Library.SuperSocket.Model
{
    /// <summary>
    /// 微信服务生成器
    /// </summary>
    /// <typeparam name="TPackageInfo">消息包类型</typeparam>
    public class SuperSocketGenerator<TPackageInfo> : ISuperSocketProvider<TPackageInfo> where TPackageInfo : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public SuperSocketGenerator(SuperSocketGenOptions<TPackageInfo> options)
        {
            Options = options ?? new SuperSocketGenOptions<TPackageInfo>();
        }

        #region 私有成员

        SuperSocketGenOptions<TPackageInfo> Options { get; }

        ISuperSocketHostBuilder Builder { get; set; }

        #endregion

        public SuperSocketHostBuilder<TPackageInfo> GetWebSocketServerHostBuilder()
        {
            return null;
        }

        public ISuperSocketHostBuilder GetServerHostBuilder()
        {
            if (Builder != null)
                return Builder;

            var jttTypes = new Dictionary<Type, Type>();

            var supersocketHostBuilder = SuperSocketHostBuilder.Create<TPackageInfo>();

            IHostBuilder hostBuilder = null;

            if (Options.ServerOptions != null)
            {
                supersocketHostBuilder.UsePackageHandler(
                    Options.ServerOptions.PackageHandler,
                    Options.ServerOptions.ErrorHandler);

                supersocketHostBuilder.UseSessionHandler(
                    Options.ServerOptions.OnConnected,
                    Options.ServerOptions.OnClosed);

                if (Options.ServerOptions.InProcSessionContainer)
                    supersocketHostBuilder.UseInProcSessionContainer();

                hostBuilder = supersocketHostBuilder.ConfigureServices((context, services) =>
                {
                    Options.ServerOptions.ConfigureServices?.Invoke(context, services);
                })
                .ConfigureAppConfiguration((hostCtx, configApp) =>
                {
                    configApp.AddInMemoryCollection(new Dictionary<string, string>
                           {
                                { "serverOptions:name", Options.ServerOptions?.Name ?? $"SuperSocket {(Options.ServerOptions.UseUdp?"UDP":"TCP")} Server" },
                                { "serverOptions:listeners:0:ip", Options.ServerOptions?.IP },
                                { "serverOptions:listeners:0:port", Options.ServerOptions?.Port.ToString() },
                                { "serverOptions:listeners:0:backLog", Options.ServerOptions?.BackLog.ToString() }
                           });

                    Options.ServerOptions.ConfigureAppConfiguration?.Invoke(hostCtx, configApp);
                });
            }

            if (Options.LoggingOptions != null)
                hostBuilder = (hostBuilder ?? supersocketHostBuilder).ConfigureLogging((hostCtx, loggingBuilder) =>
                 {
                     if (Options.LoggingOptions?.Provider != null)
                         loggingBuilder.AddProvider(Options.LoggingOptions.Provider);
                     if (Options.LoggingOptions?.AddConsole == true)
                         loggingBuilder.AddConsole();
                     if (Options.LoggingOptions?.AddDebug != null)
                         loggingBuilder.AddDebug();
                 });

            Builder = (hostBuilder ?? supersocketHostBuilder) as ISuperSocketHostBuilder;

            if (Options.ServerOptions.UseUdp)
                Builder.UseUdp();

            return Builder;
        }

        public IHost GetWebSocketServer()
        {
            return null;
        }

        public IHost GetServer()
        {
            return GetServerHostBuilder()
                .Build();
        }
    }
}
