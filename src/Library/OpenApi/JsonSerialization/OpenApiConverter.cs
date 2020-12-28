using Library.OpenApi.Annotations;
using Library.OpenApi.Extention;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Library.OpenApi.JsonSerialization
{
    /// <summary>
    /// 自定义动态类型转换器
    /// </summary>
    /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
    public class OpenApiConverter<TOpenApiSchema> : ExpandoObjectConverter
    {
        /// <summary>
        /// 输出的属性
        /// </summary>
        Dictionary<string, List<string>> PropertyDic;

        /// <summary>
        /// 
        /// </summary>
        public OpenApiConverter()
        {
            PropertyDic = typeof(TOpenApiSchema).GetOrNullForPropertyDic(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyDic">输出的属性</param>
        public OpenApiConverter(Dictionary<string, List<string>> propertyDic)
        {
            PropertyDic = propertyDic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionProperties">特别输出的属性</param>
        /// <param name="ignoreProperties">特别忽略的属性</param>
        public OpenApiConverter(Dictionary<string, List<string>> exceptionProperties, Dictionary<string, List<string>> ignoreProperties)
        {
            PropertyDic = typeof(TOpenApiSchema).GetOrNullForPropertyDic(true, exceptionProperties, ignoreProperties);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var expandoObject = base.ReadJson(reader, objectType, existingValue, serializer);

            FilterExpandoObject(expandoObject, objectType);

            return expandoObject;
        }

        private void FilterExpandoObject(object expandoObject, Type objectType)
        {
            if (expandoObject == null)
                return;

            var type = objectType;

            var isEnumerable = false;
            if (type.IsArray)
            {
                type = type.Assembly.GetType(type.FullName.Replace("[]", string.Empty));
                isEnumerable = true;
            }
            else if (type.IsGenericType)
            {
                type = type.GenericTypeArguments[0];
                isEnumerable = true;
            }

            if (isEnumerable)
                Foreach(expandoObject as IList<object>, type);
            else
            {
                var expandoObjectDic = expandoObject as IDictionary<string, object>;
                var keys = expandoObjectDic.Keys.ToList();
                foreach (var key in keys)
                {
                    if (!PropertyDic[objectType.FullName].Contains(key))
                    {
                        expandoObjectDic.Remove(key);
                        continue;
                    }

                    var prop = type.GetProperty(key);
                    var schemaAttribute = prop.GetCustomAttribute<OpenApiSchemaAttribute>();
                    if (schemaAttribute?.Type == OpenApiSchemaType.model)
                        FilterExpandoObject(expandoObjectDic[key], prop.PropertyType);
                }
            }
        }

        private void Foreach(IList<object> expandoObjectList, Type objectType)
        {
            foreach (var expandoObject in expandoObjectList)
            {
                FilterExpandoObject(expandoObject, objectType);
            }
        }
    }
}
