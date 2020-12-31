using System;
using System.Collections.Generic;
using System.Text;

namespace IocServiceDemo
{
    /// <summary>
    /// 服务提供者接口类
    /// </summary>
    public interface IDemoServiceProvider
    {
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <returns></returns>
        IDemoService GetService();

        /// <summary>
        /// 获取指定服务
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns></returns>
        IDemoService GetService<T>() where T : class;
    }
}
