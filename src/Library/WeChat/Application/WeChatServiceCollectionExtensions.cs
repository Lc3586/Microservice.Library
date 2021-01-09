﻿using Library.WeChat.Application;
using Library.WeChat.Gen;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入扩展方法
    /// </summary>
    public static class WeChatServiceCollectionExtensions
    {
        /// <summary>
        /// 添加微信服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddWeChatService(
            this IServiceCollection services,
            Action<WeChatGenOptions> setupAction = null)
        {
            //注册自定义配置程序，将高级配置（<WeChatGenOptions）应用于低级配置（WeChatServiceOptions）。
            services.AddTransient<IConfigureOptions<WeChatServiceOptions>, ConfigureWeChatServiceOptions>();

            //注册生成器和依赖
            services.AddTransient(s => s.GetRequiredService<IOptions<WeChatGenOptions>>().Value);

            services.AddSingleton<IWeChatServiceProvider, WeChatServiceGenerator>();

            if (setupAction != null) services.ConfigureWeChatService(setupAction);

            return services;
        }

        /// <summary>
        /// 配置微信服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        public static void ConfigureWeChatService(
            this IServiceCollection services,
            Action<WeChatGenOptions> setupAction)
        {
            services.Configure(setupAction);
        }
    }
}
