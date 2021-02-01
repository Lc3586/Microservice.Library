using Api.Filter.Soap;
using Library.Container;
using Library.Extension;
using Library.Soap.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Model.System.Config;
using SoapCore.Extensibility;
using System.Linq;
using SoapSerializer = SoapCore.SoapSerializer;

namespace Api.Configures
{
    /// <summary>
    /// Soap配置类
    /// </summary>
    public class SoapConfigure
    {
        static SoapServerOptions[] ServerOptions;

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void RegisterServices(IServiceCollection services, SystemConfig config)
        {
            ServerOptions = config.Soaps.Where(w => w.Enable && w.Type == SoapType.Server).Select(w => new SoapServerOptions
            {
                ServiceType = (w.ServiceType.Substring(0, w.ServiceType.IndexOf('.')), w.ServiceType),
                ImplementationType = (w.ImplementationType.Substring(0, w.ImplementationType.IndexOf('.')), w.ImplementationType),
                Path = w.Path,
                Serializer = (SoapSerializer)(int)w.Serializer,
                CustomResponse = w.CustomResponse,
                HttpContextGetter = () => AutofacHelper.GetService<IHttpContextAccessor>().HttpContext
            }).ToArray();

            services.AddSoapServer(ServerOptions)
                .AddSoapClient(options =>
                {
                    options.SoapClients = config.Soaps.Where(w => w.Enable && w.Type == SoapType.Client).Select(w => new SoapClientOptions
                    {
                        Name = w.Name,
                        ServiceType = (w.ServiceType.Substring(0, w.ServiceType.IndexOf('.')), w.ServiceType),
                        Uri = w.Uri
                    }).ToList();
                });

            //消息拦截器
            services.AddSingleton<IMessageFilter, MessageFilter>();
            //services.AddSingleton<IMessageInspector, MessageInspector>();
        }

        /// <summary>
        /// 配置应用
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        public static void RegisterEndpoint(IApplicationBuilder app, SystemConfig config)
        {
            app.AddSoapServer(ServerOptions);
        }
    }
}
