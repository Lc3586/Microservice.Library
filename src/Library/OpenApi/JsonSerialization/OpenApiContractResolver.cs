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
    /// 自定义解析器
    /// </summary>
    /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
    public class OpenApiContractResolver<TOpenApiSchema> : DefaultContractResolver
    {
        /// <summary>
        /// 输出的属性
        /// </summary>
        Dictionary<string, List<string>> PropertyDic;

        /// <summary>
        /// 
        /// </summary>
        public OpenApiContractResolver()
        {
            PropertyDic = typeof(TOpenApiSchema).GetOrNullForPropertyDic(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyDic">输出的属性</param>
        public OpenApiContractResolver(Dictionary<string, List<string>> propertyDic)
        {
            PropertyDic = propertyDic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionProperties">特别输出的属性</param>
        /// <param name="ignoreProperties">特别忽略的属性</param>
        public OpenApiContractResolver(Dictionary<string, List<string>> exceptionProperties, Dictionary<string, List<string>> ignoreProperties)
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

        /// <summary>
        /// 判定协议
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override JsonContract ResolveContract(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            //var contract = base.ResolveContract(type);

            var contract = base.ResolveContract(type);

            //var _type = type;

            //var isEnumerable = false;
            //if (_type.IsArray)
            //{
            //    _type = _type.Assembly.GetType(_type.FullName.Replace("[]", string.Empty));
            //    isEnumerable = true;
            //}
            //else if (_type.IsGenericType)
            //{
            //    _type = _type.GenericTypeArguments[0];
            //    isEnumerable = true;
            //}

            //if (isEnumerable)
            //    contract = base.ResolveContract(type);
            ////contract = base.CreateArrayContract(type);
            //else
            //{
            //    contract = base.ResolveContract(type);
            //    //contract = base.CreateObjectContract(type);
            //}

            if (contract.GetType() == typeof(JsonObjectContract))
                Filter(contract as JsonObjectContract);

            return contract;
        }

        /// <summary>
        /// 过滤属性
        /// </summary>
        /// <param name="contract">协议</param>
        private void Filter(JsonObjectContract contract)
        {
            if (contract == null)
                return;

            var properties = contract.Properties.Select(o => o.PropertyName).ToList();
            foreach (var property in properties)
            {
                if (!PropertyDic[contract.UnderlyingType.FullName].Contains(property))
                    contract.Properties.Remove(property);
            }
        }
    }
}
