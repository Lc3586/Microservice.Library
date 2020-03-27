using Library.OpenApi.Annotations;
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
        /// 最终要输出的属性
        /// </summary>
        List<string> lstExclude;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="excludedProperties">要额外输出的属性</param>
        public JsonPropertyContractResolver(string[] exceptionProperties, string[] ignoreProperties)
        {
            lstExclude = exceptionProperties?.ToList();

            if (lstExclude == null)
                lstExclude = new List<string>();

            if (ignoreProperties != null)
                lstExclude.RemoveAll(o => ignoreProperties.Contains(o));

            var type = typeof(TOpenApiSchema);

            var tag = type.GetMainTag();
            var hasTag = !string.IsNullOrEmpty(tag);
            var strictMode = type.GetCustomAttribute<OpenApiSchemaStrictModeAttribute>() != null;

            foreach (var prop in type.GetProperties())
            {
                if (prop.PropertyType.IsNotPublic)
                    continue;

                if (ignoreProperties?.Contains(prop.Name) == true)
                    continue;

                if (strictMode &&
                    !prop.CustomAttributes.Any(o =>
                        o.AttributeType == typeof(OpenApiSchemaAttribute)))
                    continue;

                if (prop.GetCustomAttribute<OpenApiIgnoreAttribute>() != null)
                    continue;

                if (prop.DeclaringType?.Name != type.Name)
                    if (hasTag)
                        if (!prop.HasTag(tag))
                            continue;

                lstExclude.Add(prop.Name);
            }
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var result = base.CreateProperties(type, memberSerialization).ToList();
            return lstExclude.Any() ? result.FindAll(p => lstExclude.Contains(p.PropertyName)) : result;
        }
    }
}
