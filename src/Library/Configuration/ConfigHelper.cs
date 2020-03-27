using Library.Container;
using Microsoft.Extensions.Configuration;
using System;

namespace Library.Configuration
{
    /// <summary>
    /// 配置文件帮助类
    /// </summary>
    public class ConfigHelper
    {
        static ConfigHelper()
        {
            Init();
        }

        public static void Configure(string jsonFile)
        {
            Init();
        }

        private static void Init(string jsonFile = "appsettings.json")
        {
            IConfiguration config = AutofacHelper.GetService<IConfiguration>();
            if (config == null)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile(jsonFile);

                config = builder.Build();
            }

            _Config = config;
        }

        private static IConfiguration _Config { get; set; }

        private static IConfiguration Config { get { return _Config; } }

        /// <summary>
        /// 从AppSettings获取key的值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            return Config[key];
        }

        /// <summary>
        /// 从AppSettings获取key的值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static T GetValue<T>(string key)
        {
            return Config.GetValue<T>(key);
        }

        /// <summary>
        /// 从AppSettings获取key的值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="Default">默认值</param>
        /// <returns></returns>
        public static T GetValue<T>(string key, T Default = default)
        {
            T Value = Config.GetValue<T>(key);
            return Value == null ? Default : Value;
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="nameOfCon">连接字符串名</param>
        /// <returns></returns>
        public static string GetConnectionString(string nameOfCon)
        {
            return Config.GetConnectionString(nameOfCon);
        }

        /// <summary>
        /// 获取模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section">板块</param>
        /// <returns></returns>
        public static T GetModel<T>(string section)
        {
            return Config.GetSection(section).Get<T>();
        }
    }
}
