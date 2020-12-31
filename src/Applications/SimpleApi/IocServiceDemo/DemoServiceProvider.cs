using System;
using System.Collections.Generic;
using System.Text;

namespace IocServiceDemo
{
    /// <summary>
    /// 服务提供者实现类
    /// </summary>
    public class DemoServiceProvider : IDemoServiceProvider
    {
        public DemoServiceProvider(DemoServiceOptions options)
        {
            DemoServiceManagement.SetOption(options);
        }

        public IDemoService GetService()
        {
            return Activator.CreateInstance(GetDefaultType().Use()) as IDemoService;
        }

        public IDemoService GetService<T>() where T : class
        {
            return Activator.CreateInstance(typeof(T).Use()) as IDemoService;
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static object GetService(IServiceProvider serviceProvider)
        {
            return Activator.CreateInstance(GetDefaultType().Use());
        }

        /// <summary>
        /// 获取默认服务类型
        /// </summary>
        /// <returns></returns>
        private static Type GetDefaultType()
        {
            return DemoServiceManagement.GetAvailableType();
        }
    }
}
