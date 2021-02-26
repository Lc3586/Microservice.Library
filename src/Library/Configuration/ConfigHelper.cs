using Microservice.Library.Configuration.Annotations;
using Microservice.Library.Configuration.Extention;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace Microservice.Library.Configuration
{
    /// <summary>
    /// 配置帮助类
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration">配置</param>
        public ConfigHelper(IConfiguration configuration)
        {
            Config = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonFile">Json配置文件</param>
        /// <param name="disableCache">禁用缓存</param>
        public ConfigHelper(string jsonFile = null, bool disableCache = false)
        {
            Init(jsonFile, disableCache);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonFile">Json配置文件</param>
        /// <param name="disableCache">禁用缓存</param>
        void Init(string jsonFile = null, bool disableCache = false)
        {
            if (jsonFile == null)
            {
                if (Config != null)
                    return;

                Config = GetConfigFormFile("appsettings.json", disableCache);
            }
            else
                Config = GetConfigFormFile(jsonFile, disableCache);
        }
        static BindingFlags _bindingFlags { get; }
            = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonFile">Json配置文件</param>
        /// <param name="disableCache">禁用缓存</param>
        /// <returns></returns>
        static IConfiguration GetConfigFormFile(string jsonFile, bool disableCache = false)
        {
            if (!disableCache)
                if (CacheExtention.ConfigDic.ContainsKey(jsonFile))
                    return CacheExtention.ConfigDic[jsonFile];

            var config = new ConfigurationBuilder()
                         .SetBasePath(AppContext.BaseDirectory)
                         .AddJsonFile(jsonFile)
                         .Build();

            if (!disableCache)
                if (!CacheExtention.ConfigDic.ContainsKey(jsonFile))
                    CacheExtention.ConfigDic.Add(jsonFile, config);

            return config;
        }

        /// <summary>
        /// 配置
        /// </summary>
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
            //防止无限递归
            Type firstType = null;

            var result = Config.GetSection(section).Get<T>();

            getValue(result, typeof(T));

            return result;

            void getValue(object obj, Type type)
            {
                foreach (var property in type.GetProperties(_bindingFlags))
                {
                    object value = null;

                    var jsonConfig = property.GetCustomAttribute<JsonConfigAttribute>();
                    if (jsonConfig == null)
                        goto check;

                    if (string.IsNullOrWhiteSpace(jsonConfig.JsonFile))
                        throw new ApplicationException($"{nameof(JsonConfigAttribute)}: {property.DeclaringType.FullName} + {property.Name}, Json文件不可为空.");

                    var config = GetConfigFormFile(jsonConfig.JsonFile, jsonConfig.DisableCache);

                    value = string.IsNullOrEmpty(jsonConfig.Key)
                        ? config.Get(property.PropertyType)
                        : config.GetSection(jsonConfig.Key).Get(property.PropertyType);

                    property.SetValue(obj, value);

                    check:

                    var check = property.GetCustomAttribute<PropertyConfigAttribute>();
                    if (check != null)
                    {
                        if (value == null)
                        {
                            value = Activator.CreateInstance(property.PropertyType);
                            property.SetValue(obj, value);
                        }

                        if (firstType == null)
                            firstType = property.PropertyType;
                        else if (firstType == property.PropertyType)
                            throw new ApplicationException($"递归无法结束, 请检查类型 {type.FullName}中的属性 {property.Name}.");

                        getValue(value, property.PropertyType);
                    }
                }
            }
        }
    }
}
