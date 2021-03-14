using Microsoft.Extensions.Hosting;
using SuperSocket;
using SuperSocket.WebSocket;
using SuperSocket.WebSocket.Server;

namespace Microservice.Library.SuperSocket.Model
{
    /// <summary>
    /// 服务器构造器
    /// </summary>
    /// <typeparam name="TPackageInfo">消息包类型</typeparam>
    public interface ISuperSocketProvider<TPackageInfo> where TPackageInfo : class
    {
        /// <summary>
        /// 获取服务构建器
        /// </summary>
        /// <returns></returns>
        SuperSocketHostBuilder<TPackageInfo> GetWebSocketServerHostBuilder();

        /// <summary>
        /// 获取服务构建器
        /// </summary>
        /// <returns></returns>
        ISuperSocketHostBuilder GetServerHostBuilder();

        /// <summary>
        /// 获取服务器
        /// </summary>
        /// <returns></returns>
        IHost GetWebSocketServer();

        /// <summary>
        /// 获取服务器
        /// </summary>
        /// <returns></returns>
        IHost GetServer();
    }
}
