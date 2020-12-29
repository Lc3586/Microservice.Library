using Library.OpenApi.Annotations;
using Library.OpenApi.Extention;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Library.OpenApi.JsonSerialization
{
    /// <summary>
    /// 自定义动态类型转换器
    /// </summary>
    /// <remarks>未完工</remarks>
    internal class OpenApiDynamicConverter : ExpandoObjectConverter
    {
        /// <summary>
        /// 输出的属性
        /// </summary>
        readonly Type SchemaType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemaType">架构类型</param>
        public OpenApiDynamicConverter(Type schemaType)
        {
            SchemaType = schemaType;
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
            //var expandoObject = base.ReadJson(reader, objectType, existingValue, serializer);

            return ReadValue(reader, SchemaType);

            //return expandoObject;
        }

        private object ReadValue(JsonReader reader, Type objectType)
        {
            if (!MoveToContent(reader))
            {
                throw new ApplicationException("Unexpected end when reading ExpandoObject.");
            }

            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    return ReadObject(reader, objectType);
                case JsonToken.StartArray:
                    // netcoreapp3.0 uses EmptyPartition for empty enumerable. Treat as an empty array.
                    var _objectType = objectType.IsArray ? objectType.Assembly.GetType(objectType.FullName.Replace("[]", string.Empty)) : objectType.GenericTypeArguments[0];
                    return ReadList(reader, _objectType);
                default:
                    if (IsPrimitiveToken(reader.TokenType))
                    {
                        return reader.Value;
                    }

                    throw new ApplicationException(string.Format(CultureInfo.InvariantCulture, "Unexpected token when converting ExpandoObject: {0}", reader.TokenType));
            }
        }

        private bool MoveToContent(JsonReader reader)
        {
            JsonToken t = reader.TokenType;
            while (t == JsonToken.None || t == JsonToken.Comment)
            {
                if (!reader.Read())
                {
                    return false;
                }

                t = reader.TokenType;
            }

            return true;
        }

        private bool IsPrimitiveToken(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.String:
                case JsonToken.Boolean:
                case JsonToken.Undefined:
                case JsonToken.Null:
                case JsonToken.Date:
                case JsonToken.Bytes:
                    return true;
                default:
                    return false;
            }
        }

        private object ReadList(JsonReader reader, Type objectType)
        {
            IList<object> list = new List<object>();

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.Comment:
                        break;
                    default:
                        object v = ReadValue(reader, objectType);

                        list.Add(v);
                        break;
                    case JsonToken.EndArray:
                        return list;
                }
            }

            throw new ApplicationException("Unexpected end when reading ExpandoObject.");
        }

        private object ReadObject(JsonReader reader, Type objectType)
        {
            var expandoObject = Activator.CreateInstance(objectType) as IDictionary<string, object>;
            //IDictionary<string, object> expandoObject = new ExpandoObject();

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        string propertyName = reader.Value.ToString();

                        if (!reader.Read())
                        {
                            throw new ApplicationException("Unexpected end when reading ExpandoObject.");
                        }

                        object v = ReadValue(reader, objectType);

                        expandoObject[propertyName] = v;
                        break;
                    case JsonToken.Comment:
                        break;
                    case JsonToken.EndObject:
                        return expandoObject;
                }
            }

            throw new ApplicationException("Unexpected end when reading ExpandoObject.");
        }

        /// <summary>
        /// 将动态类型转为指定类型实体
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object DynamicToEntity(ExpandoObject data, Type type)
        {
            var entity = Activator.CreateInstance(type);

            var dic = data as IDictionary<string, object>;

            foreach (var item in dic)
            {
                if (item.Value == null)
                    continue;

                var type_value = item.Value.GetType();

                if (type_value == typeof(DBNull))
                    continue;

                var prop = type.GetProperty(item.Key, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);

                if (prop == null)
                    continue;

                object value = item.Value;

                if (type_value != prop.PropertyType)
                {
                    if (type_value == typeof(List<object>))
                    {
                        var valueList = Activator.CreateInstance(prop.PropertyType);
                        var dicValueList = value as List<object>;

                        foreach (var valueItem in dicValueList)
                        {
                            prop.PropertyType.GetMethod("Add")
                                                .Invoke(valueList, new object[] {
                                                    DynamicToEntity(valueItem as ExpandoObject, prop.PropertyType.GenericTypeArguments[0])
                                                });
                        }

                        value = valueList;
                    }
                    else if (type_value == typeof(ExpandoObject))
                        value = DynamicToEntity(item.Value as ExpandoObject, prop.PropertyType);
                    else if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        NullableConverter newNullableConverter = new NullableConverter(prop.PropertyType);
                        try
                        {
                            if (!newNullableConverter.CanConvertFrom(item.Value.GetType()))
                            {
                                value = Convert.ChangeType(item.Value, newNullableConverter.UnderlyingType);
                            }
                            else
                                value = newNullableConverter.ConvertFrom(item.Value);
                        }
                        catch
                        {
                            value = newNullableConverter.ConvertFromString(item.Value?.ToString());
                        }
                    }
                    else
                    {
                        value = Convert.ChangeType(item.Value, prop.PropertyType);
                    }
                }
                prop.SetValue(entity, value);
            }
            return entity;
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(ExpandoObject));
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="JsonConverter"/> can write JSON.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this <see cref="JsonConverter"/> can write JSON; otherwise, <c>false</c>.
        /// </value>
        public override bool CanWrite => false;



        //private void FilterExpandoObject(object expandoObject, Type objectType)
        //{
        //    if (expandoObject == null)
        //        return;

        //    var type = objectType;

        //    var isEnumerable = false;
        //    if (type.IsArray)
        //    {
        //        type = type.Assembly.GetType(type.FullName.Replace("[]", string.Empty));
        //        isEnumerable = true;
        //    }
        //    else if (type.IsGenericType)
        //    {
        //        type = type.GenericTypeArguments[0];
        //        isEnumerable = true;
        //    }

        //    if (isEnumerable)
        //        Foreach(expandoObject as IList<object>, type);
        //    else
        //    {
        //        var expandoObjectDic = expandoObject as IDictionary<string, object>;
        //        var keys = expandoObjectDic.Keys.ToList();
        //        foreach (var key in keys)
        //        {
        //            if (!PropertyDic[objectType.FullName].Contains(key))
        //            {
        //                expandoObjectDic.Remove(key);
        //                continue;
        //            }

        //            var prop = type.GetProperty(key);
        //            var schemaAttribute = prop.GetCustomAttribute<OpenApiSchemaAttribute>();
        //            if (schemaAttribute?.Type == OpenApiSchemaType.model)
        //                FilterExpandoObject(expandoObjectDic[key], prop.PropertyType);
        //        }
        //    }
        //}

        //private void Foreach(IList<object> expandoObjectList, Type objectType)
        //{
        //    foreach (var expandoObject in expandoObjectList)
        //    {
        //        FilterExpandoObject(expandoObject, objectType);
        //    }
        //}
    }
}
