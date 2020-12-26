using Library.OpenApi.Annotations;
using Library.OpenApi.Extention;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Library.OpenApi.JsonSerialization
{
    /// <summary>
    /// 自定义分解器
    /// </summary>
    /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
    public class JsonPropertyContractResolver<TOpenApiSchema> : DefaultContractResolver
    {
        /// <summary>
        /// 输出的属性
        /// </summary>
        Dictionary<string, List<string>> PropertyDic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionProperties">特别输出的属性</param>
        /// <param name="ignoreProperties">特别忽略的属性</param>
        public JsonPropertyContractResolver(Dictionary<string, List<string>> exceptionProperties, Dictionary<string, List<string>> ignoreProperties)
        {
            PropertyDic = typeof(TOpenApiSchema).GetOrNullForPropertyDic(true, exceptionProperties, ignoreProperties);
        }

        /// <summary>
        /// 创建属性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var result = base.CreateProperties(type, memberSerialization).ToList();
            return PropertyDic.Any() ? result.FindAll(p => PropertyDic[type.FullName].Contains(p.PropertyName)) : result;
        }
    }
}
