using System;
using System.Collections.Generic;

namespace Library.Soap.Gen
{
    /// <summary>
    /// Soap构造器接口类
    /// </summary>
    public interface ISoapClientProvider
    {
        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>客户端</returns>
        object GetClient(string name);

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="type">客户端类型</param>
        /// <returns>客户端</returns>
        object GetClient(Type type);

        /// <summary>
        /// 获取所有客户端
        /// <para>Name 名称</para>
        /// <para>Type 类型</para>
        /// <para>Client 客户端</para>
        /// </summary>
        /// <returns></returns>
        IEnumerable<(string Name, Type Type, object Client)> GetClients();

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <typeparam name="TChannel">客户端类型</typeparam>
        /// <returns>客户端</returns>
        TChannel GetClient<TChannel>() where TChannel : class;
    }
}
