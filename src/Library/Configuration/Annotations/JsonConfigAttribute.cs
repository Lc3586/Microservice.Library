using System;

namespace Library.Configuration.Annotations
{
    /// <summary>
    /// Json配置文件属性
    /// </summary>
    public class JsonConfigAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonFile">Json文件</param>
        /// <param name="key">键</param>
        public JsonConfigAttribute(string jsonFile, string key = null)
        {
            JsonFile = jsonFile;
            Key = key;
        }

        /// <summary>
        /// Json文件
        /// </summary>
        public string JsonFile { get; set; }

        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 禁用缓存
        /// </summary>
        public bool DisableCache { get; set; } = false;
    }
}
