using Microservice.Library.Configuration.Annotations;
using Microservice.Library.Configuration.Extention;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
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
            if (jsonFile != null)
                JsonFiles = new string[] { jsonFile };
            Init(disableCache);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonFiles">Json配置文件</param>
        /// <param name="disableCache">禁用缓存</param>
        public ConfigHelper(string[] jsonFiles = null, bool disableCache = false)
        {
            if (jsonFiles != null)
                JsonFiles = jsonFiles;
            Init(disableCache);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disableCache">禁用缓存</param>
        void Init(bool disableCache = false)
        {
            if ((JsonFiles == null || JsonFiles.Length == 0) && Config != null)
                return;

            Config = GetConfigFormFile(JsonFiles, disableCache);
        }

        /// <summary>
        /// 
        /// </summary>
        static BindingFlags _bindingFlags { get; }
            = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonFiles">Json配置文件</param>
        /// <param name="disableCache">禁用缓存</param>
        /// <returns></returns>
        static IConfiguration GetConfigFormFile(string[] jsonFiles, bool disableCache = false)
        {
            if (!disableCache)
                foreach (var jsonFile in jsonFiles)
                {
                    if (CacheExtention.ConfigDic.ContainsKey(jsonFile))
                        return CacheExtention.ConfigDic[jsonFile];
                }

            var builder = new ConfigurationBuilder()
                         .SetBasePath(AppContext.BaseDirectory);

            foreach (var jsonFile in jsonFiles)
            {
                builder.AddJsonFile(jsonFile, false, true);
            }

            var config = builder.Build();

            if (!disableCache)
                foreach (var jsonFile in jsonFiles)
                {
                    if (!CacheExtention.ConfigDic.ContainsKey(jsonFile))
                        CacheExtention.ConfigDic.Add(jsonFile, config);
                }

            return config;
        }

        /// <summary>
        /// 注册配置变更事件
        /// </summary>
        void RegisterChangeCallback<T>(IConfiguration config, T data, string[] jsonFiles, string section)
        {
            ChangeToken.OnChange(() => config.GetReloadToken(), state =>
            {
                ChangeCallback(state.data, state.jsonFiles, state.section);
            }, (data, jsonFiles, section));
        }

        /// <summary>
        /// 文件
        /// </summary>
        /// <remarks>默认 appsettings.json</remarks>
        string[] JsonFiles { get; set; } = new string[] { "appsettings.json" };

        /// <summary>
        /// 配置
        /// </summary>
        IConfiguration Config { get; set; }

        /// <summary>
        /// 配置变更回调事件
        /// </summary>
        /// <remarks>
        /// 参数: 数据, json文件, section
        /// </remarks>
        public Action<object, string[], string> ChangeCallback { get; set; }

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

            var config_section = Config.GetSection(section);
            var result = config_section.Get<T>();

            getValue(result, typeof(T));

            RegisterChangeCallback(config_section, result, JsonFiles, section);

            return result;

            void getValue(object obj, Type type)
            {
                foreach (var property in type.GetProperties(_bindingFlags))
                {
                    object value = null;

                    var jsonConfig = property.GetCustomAttribute<JsonConfigAttribute>();
                    if (jsonConfig == null)
                        goto check;

                    if (jsonConfig.JsonFiles == null || jsonConfig.JsonFiles.Length == 0)
                        throw new ApplicationException($"{nameof(JsonConfigAttribute)}: {property.DeclaringType.FullName} + {property.Name}, Json文件不可为空.");

                    var inner_config = GetConfigFormFile(jsonConfig.JsonFiles, jsonConfig.DisableCache);

                    value = string.IsNullOrEmpty(jsonConfig.Key)
                        ? inner_config.Get(property.PropertyType)
                        : inner_config.GetSection(jsonConfig.Key).Get(property.PropertyType);

                    property.SetValue(obj, value);

                    RegisterChangeCallback(inner_config, value, jsonConfig.JsonFiles, jsonConfig.Key);

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
