using Library.OpenApi.Extention;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Library.OpenApi.JsonSerialization
{
    /// <summary>
    /// 自定义解析器
    /// </summary>
    public class OpenApiContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// 输出的属性
        /// </summary>
        readonly Dictionary<string, List<string>> PropertyDic;

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
        /// <param name="schemaType">架构类型</param>
        public OpenApiContractResolver(Type schemaType)
        {
            PropertyDic = schemaType.GetOrNullForPropertyDic(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemaType">架构类型</param>
        /// <param name="exceptionProperties">特别输出的属性</param>
        /// <param name="ignoreProperties">特别忽略的属性</param>
        public OpenApiContractResolver(Type schemaType, Dictionary<string, List<string>> exceptionProperties, Dictionary<string, List<string>> ignoreProperties)
        {
            PropertyDic = schemaType.GetOrNullForPropertyDic(true, exceptionProperties, ignoreProperties);
        }

        ///// <summary>
        ///// 创建属性
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="memberSerialization"></param>
        ///// <returns></returns>
        //protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        //{
        //    var result = base.CreateProperties(type, memberSerialization).ToList();
        //    return PropertyDic.Any() ? result.FindAll(p => PropertyDic[type.FullName].Contains(p.PropertyName)) : result;
        //}

        /// <summary>
        /// 判定协议
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override JsonContract ResolveContract(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var contract = base.ResolveContract(type);

            if (contract.GetType() == typeof(JsonObjectContract))
                Filter(contract as JsonObjectContract);
            //关于动态类型反序列化暂无实现思路
            //else if (contract.GetType() == typeof(JsonDictionaryContract))
            //    SetConvert(contract as JsonDictionaryContract, type);

            return contract;
        }

        /// <summary>
        /// 过滤属性
        /// </summary>
        /// <param name="contract">协议</param>
        private void Filter(JsonObjectContract contract)
        {
            if (PropertyDic == null)
                return;

            if (contract == null)
                return;

            //var removeProperties = contract.Properties.Where(o => !PropertyDic[contract.UnderlyingType.FullName].Contains(o.PropertyName)).Select(o => o.PropertyName).ToArray();
            //foreach (var item in removeProperties)
            //{
            //    contract.Properties.Remove(item);
            //}
            foreach (var property in contract.Properties)
            {
                if (!PropertyDic.ContainsKey(contract.UnderlyingType.FullName))
                    continue;

                if (!PropertyDic[contract.UnderlyingType.FullName].Contains(property.PropertyName))
                {
                    property.Ignored = true;
                    property.Writable = false;
                    property.Readable = false;
                }
            }
        }

        ///// <summary>
        ///// 过滤属性
        ///// </summary>
        ///// <param name="contract">协议</param>
        //private void SetConvert(JsonDictionaryContract contract, Type type)
        //{
        //    if (PropertyDic == null)
        //        return;

        //    if (contract == null)
        //        return;

        //    foreach (var item in type.GetProperties())
        //    {
        //        Console.WriteLine(item.Name);
        //    }

        //    contract.Converter = new OpenApiDynamicConverter(type);
        //}
    }
}
