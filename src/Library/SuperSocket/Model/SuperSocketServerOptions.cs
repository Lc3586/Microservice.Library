using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperSocket;
using SuperSocket.Channel;
using System;
using System.Threading.Tasks;

namespace Microservice.Library.SuperSocket.Model
{
    /// <summary>
    /// 服务器配置
    /// </summary>
    /// <typeparam name="TPackageInfo">消息包类型</typeparam>
    public class SuperSocketServerOptions<TPackageInfo> where TPackageInfo : class
    {
        /// <summary>
        /// 名称
        /// </summary>
        /// <remarks>默认 $"{JTTVersion} Server"</remarks>
        public string Name { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        /// <remarks>默认 Any</remarks>
        public string IP { get; set; } = "Any";

        /// <summary>
        /// 端口
        /// </summary>
        /// <remarks>默认 4040</remarks>
        public int Port { get; set; } = 4040;

        /// <summary>
        /// 日志备份数量
        /// </summary>
        /// <remarks>默认10000</remarks>
        public int BackLog { get; set; } = 10000;

        /// <summary>
        /// 使用Udp协议
        /// </summary>
        public bool UseUdp { get; set; } = false;

        /// <summary>
        /// 处理消息包
        /// </summary>
        public Func<IAppSession, TPackageInfo, ValueTask> PackageHandler { get; set; }

        /// <summary>
        /// 处理异常
        /// </summary>
        public Func<IAppSession, PackageHandlingException<TPackageInfo>, ValueTask<bool>> ErrorHandler { get; set; }

        /// <summary>
        /// 已建立连接
        /// </summary>
        public Func<IAppSession, ValueTask> OnConnected { get; set; }

        /// <summary>
        /// 连接已关闭
        /// </summary>
        public Func<IAppSession, CloseEventArgs, ValueTask> OnClosed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool InProcSessionContainer { get; set; }

        /// <summary>
        /// 配置服务
        /// </summary>
        public Action<HostBuilderContext, IServiceCollection> ConfigureServices { get; set; }

        /// <summary>
        /// 配置应用
        /// </summary>
        public Action<HostBuilderContext, IConfigurationBuilder> ConfigureAppConfiguration { get; set; }
    }
}
