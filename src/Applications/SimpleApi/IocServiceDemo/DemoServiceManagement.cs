using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microservice.Library.Extension;

namespace IocServiceDemo
{
    /// <summary>
    /// 服务管理类
    /// </summary>
    public static class DemoServiceManagement
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static DemoServiceManagement()
        {
            CurrentUsage = new ConcurrentDictionary<Type, int>();

            //添加接口所有未禁用的实现类
            typeof(DemoServiceManagement)
                .GetTypeInfo()
                .Assembly
                .GetTypes()
                .Where(t =>
                            !Options.DisableType.Contains(t) &&
                            t.GetInterfaces()
                                .Contains(typeof(IDemoService))
                      )
                .ForEach(t =>
                              CurrentUsage.TryAdd(t, 0)
                        );
        }

        /// <summary>
        /// 配置
        /// </summary>
        private static DemoServiceOptions Options = new DemoServiceOptions();

        /// <summary>
        /// 服务使用情况
        /// </summary>
        private static readonly ConcurrentDictionary<Type, int> CurrentUsage;

        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="options">配置</param>
        public static void SetOption(DemoServiceOptions options)
        {
            Options = options;
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <returns></returns>
        public static Type GetAvailableType()
        {
            //则返回一个未超过阈值的
            foreach (var item in CurrentUsage)
            {
                if (item.Value <= Options.Threshold)
                    return item.Key;
            }

            //如果都超过了阈值，则返回一个使用量最少的
            return CurrentUsage.OrderByDescending(o => o.Value).FirstOrDefault().Key;
        }

        /// <summary>
        /// 使用
        /// </summary>
        /// <param name="type"></param>
        public static Type Use(this Type type)
        {
            type.Check();
            CurrentUsage.AddOrUpdate(type, 1, (key, oldValue) => ++oldValue);
            return type;
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="type"></param>
        public static void Quit<T>(this T service) where T : IDemoService
        {
            var type = service.GetType();
            type.Check();
            CurrentUsage.AddOrUpdate(type, 0, (key, oldValue) => --oldValue);
        }

        /// <summary>
        /// 检查
        /// </summary>
        /// <param name="type"></param>
        private static void Check(this Type type)
        {
            if (Options.DisableType.Contains(type))
                throw new ApplicationException($"[{type.FullName}] 未启用");
        }
    }
}
