using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservice.Library.OpenApi.JsonExtension
{
    /// <summary>
    /// 自定义分解器
    /// </summary>
    public class JsonPropertyContractResolver : DefaultContractResolver
    {
        IEnumerable<string> lstExclude;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="excludedProperties">要输出的属性</param>
        public JsonPropertyContractResolver(IEnumerable<string> excludedProperties)
        {
            lstExclude = excludedProperties;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).ToList().FindAll(p => lstExclude.Contains(p.PropertyName));
        }
    }
}
