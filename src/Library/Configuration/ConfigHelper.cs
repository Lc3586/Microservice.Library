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
        public ConfigHelper(string jsonFile = null)
        {
            Init(jsonFile);
        }

        void Init(string jsonFile = null)
        {
            if (jsonFile == null)
            {
                if (Config != null)
                    return;
                IConfiguration config = AutofacHelper.GetService<IConfiguration>();
                if (config == null)
                    config = GetConfigFormFile();
                Config = config;
            }
            else
                Config = GetConfigFormFile(jsonFile);
        }

        static IConfiguration GetConfigFormFile(string jsonFile = "appsettings.json")
        {
            return new ConfigurationBuilder()
                         .SetBasePath(AppContext.BaseDirectory)
                         .AddJsonFile(jsonFile)
                         .Build();
        }

        static IConfiguration Config { get; set; }

        /// <summary>
        /// 从AppSettings获取key的值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            return Config[key];
        }

        /// <summary>
        /// 从AppSettings获取key的值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public T GetValue<T>(string key)
        {
            return Config.GetValue<T>(key);
        }

        /// <summary>
        /// 从AppSettings获取key的值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="Default">默认值</param>
        /// <returns></returns>
        public T GetValue<T>(string key, T Default = default)
        {
            T Value = Config.GetValue<T>(key);
            return Value == null ? Default : Value;
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="nameOfCon">连接字符串名</param>
        /// <returns></returns>
        public string GetConnectionString(string nameOfCon)
        {
            return Config.GetConnectionString(nameOfCon);
        }

        /// <summary>
        /// 获取模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section">板块</param>
        /// <returns></returns>
        public T GetModel<T>(string section)
        {
            return Config.GetSection(section).Get<T>();
        }
    }
}
