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
    /// <typeparam name="T">指定接口架构类型</typeparam>
    public class JsonPropertyContractResolver<TOpenApiSchema> : DefaultContractResolver
    {
        /// <summary>
        /// 输出的属性
        /// </summary>
        Dictionary<Type, List<string>> LstClude;

        /// <summary>
        /// 忽略的属性
        /// </summary>
        Dictionary<Type, List<string>> IgnoreProperties;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionProperties">特别输出的属性</param>
        /// <param name="ignoreProperties">特别忽略的属性</param>
        public JsonPropertyContractResolver(Dictionary<Type, List<string>> exceptionProperties, Dictionary<Type, List<string>> ignoreProperties)
        {
            LstClude = exceptionProperties;

            if (LstClude == null)
                LstClude = new Dictionary<Type, List<string>>();

            IgnoreProperties = ignoreProperties;

            if (IgnoreProperties != null)
                foreach (var lst in LstClude)
                {
                    lst.Value.RemoveAll(o => ignoreProperties[lst.Key].Contains(o));
                }

            typeof(TOpenApiSchema).FilterModel((type, prop) =>
            {
                if (!LstClude.ContainsKey(type))
                    LstClude.Add(type, new List<string>());

                LstClude[type].Add(prop.Name);
            },
            (type, prop) =>
            {
                return IgnoreProperties?[type]?.Contains(prop.Name) != true;
            },
            true);
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var result = base.CreateProperties(type, memberSerialization).ToList();
            return LstClude.Any() ? result.FindAll(p => LstClude[type].Contains(p.PropertyName)) : result;
        }
    }
}
