using Microservice.Library.OpenApi.Annotations;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Microservice.Library.OpenApi.Extention
{
    /// <summary>
    /// 接口文档相关扩展方法
    /// </summary>
    public static class OpenApiExtention
    {
        /// <summary>
        /// 获取默认架构
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static OpenApiSchema CreateDefaultSchema(this PropertyInfo property)
        {
            string type = OpenApiSchemaType.@string, format = null;

            if (ReferenceEquals(property.PropertyType, typeof(bool)))
                type = OpenApiSchemaType.@boolean;
            else if (ReferenceEquals(type, typeof(sbyte))
                || ReferenceEquals(type, typeof(byte)))
            {
                type = OpenApiSchemaType.integer;
                format = OpenApiSchemaFormat.integer_byte;
            }
            else if (ReferenceEquals(type, typeof(sbyte[]))
                || ReferenceEquals(type, typeof(byte[])))
            {
                type = OpenApiSchemaType.integer;
                format = OpenApiSchemaFormat.integer_byte_array;
            }
            else if (ReferenceEquals(type, typeof(short))
                || ReferenceEquals(type, typeof(ushort))
                || ReferenceEquals(type, typeof(int))
                || ReferenceEquals(type, typeof(uint)))
            {
                type = OpenApiSchemaType.integer;
                format = OpenApiSchemaFormat.integer_int32;
            }
            else if (ReferenceEquals(type, typeof(long)))
            {
                type = OpenApiSchemaType.integer;
                format = OpenApiSchemaFormat.integer_int64;
            }
            else if (ReferenceEquals(type, typeof(float)))
            {
                type = OpenApiSchemaType.number;
                format = OpenApiSchemaFormat.number_float;
            }
            else if (ReferenceEquals(type, typeof(double)))
            {
                type = OpenApiSchemaType.number;
                format = OpenApiSchemaFormat.number_double;
            }
            else if (ReferenceEquals(type, typeof(decimal)))
            {
                type = OpenApiSchemaType.number;
                format = OpenApiSchemaFormat.number_decimal;
            }
            else if (ReferenceEquals(type, typeof(DateTime)))
            {
                type = OpenApiSchemaType.@string;
                format = OpenApiSchemaFormat.string_datetime;
            }
            else if (ReferenceEquals(type, typeof(string)))
            {
                type = OpenApiSchemaType.@string;
            }
            return new OpenApiSchema() { Type = type, Format = format };
        }

        /// <summary>
        /// 获取或创建接口初始类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="innerModel">处理内部模型</param>
        /// <returns></returns>
        public static IOpenApiAny GetOrNullFor(this Type type, bool innerModel = true)
        {
            if (CacheExtention.OpenApiObjectDic.ContainsKey(type.FullName))
                return CacheExtention.OpenApiObjectDic[type.FullName];

            var example = new OpenApiObject();

            var propertyDic = type.GetPropertysOfTypeDic(false);

            type.FilterModel((_type, prop) =>
            {
                IOpenApiAny any = prop.PropertyType.IsArray
                    || prop.PropertyType.IsGenericType ?
                        new OpenApiArray { prop.CreateFor(innerModel) } :
                        prop.CreateFor(innerModel);

                example.Add(prop.Name, any);

                if (!propertyDic.ContainsKey(_type.FullName))
                    propertyDic.Add(_type.FullName, new List<string>() { prop.Name });
                else
                    propertyDic[_type.FullName].Add(prop.Name);

                if (!CacheExtention.AssemblyOfTypeDic.ContainsKey(_type.FullName))
                    CacheExtention.AssemblyOfTypeDic.Add(_type.FullName, _type.Assembly.FullName);
            });

            //type.SetPropertysOfTypeDic(propertyDic);

            if (!CacheExtention.OpenApiObjectDic.ContainsKey(type.FullName))
                CacheExtention.OpenApiObjectDic.Add(type.FullName, example);

            return example;
        }

        /// <summary>
        /// 创建接口初始类
        /// </summary>
        /// <param name="property"></param>
        /// <param name="innerModel">处理内部模型</param>
        /// <returns></returns>
        public static IOpenApiAny CreateFor(this PropertyInfo property, bool innerModel)
        {
            IOpenApiAny any;

            var schemaAttribute = property.GetCustomAttribute<OpenApiSchemaAttribute>();

            var specialValue = schemaAttribute?.Value != null;

            OpenApiSchema schema = schemaAttribute == null ?
                property.CreateDefaultSchema() :
                new OpenApiSchema()
                {
                    Type = schemaAttribute.Type,
                    Format = schemaAttribute.Format
                };

            switch (schema.Type)
            {
                case OpenApiSchemaType.boolean:
                    any = specialValue && schemaAttribute.Value.TryCast(out bool value_bool) ?
                        new OpenApiBoolean(value_bool) :
                        new OpenApiBoolean(OpenApiSchemaDefaultValue._bool);
                    break;
                case OpenApiSchemaType.integer:
                    switch (schema.Format)
                    {
                        case OpenApiSchemaFormat.integer_byte:
                            any = specialValue && schemaAttribute.Value.TryCast(out byte value_byte) ?
                                new OpenApiByte(value_byte) :
                                new OpenApiByte(OpenApiSchemaDefaultValue._byte);
                            break;
                        case OpenApiSchemaFormat.integer_byte_array:
                            any = specialValue && schemaAttribute.Value.TryCast(out byte[] value_byte_array) ?
                                new OpenApiByte(value_byte_array) :
                                new OpenApiByte(OpenApiSchemaDefaultValue._byte_array);
                            break;
                        case OpenApiSchemaFormat.integer_int32:
                        default:
                            any = specialValue && schemaAttribute.Value.TryCast(out int value_int) ?
                                new OpenApiInteger(value_int) :
                                new OpenApiInteger(OpenApiSchemaDefaultValue._int);
                            break;
                        case OpenApiSchemaFormat.integer_int64:
                            any = specialValue && schemaAttribute.Value.TryCast(out long value_long) ?
                                new OpenApiLong(value_long) :
                                new OpenApiLong(OpenApiSchemaDefaultValue._long);
                            break;
                    }
                    break;
                case OpenApiSchemaType.number:
                    switch (schema.Format)
                    {
                        case OpenApiSchemaFormat.number_float:
                            any = specialValue && schemaAttribute.Value.TryCast(out float value_float) ?
                                new OpenApiFloat(value_float) :
                                new OpenApiFloat(OpenApiSchemaDefaultValue._float);
                            break;
                        case OpenApiSchemaFormat.number_double:
                        case OpenApiSchemaFormat.number_decimal:
                        default:
                            any = specialValue && schemaAttribute.Value.TryCast(out double value_double) ?
                                new OpenApiDouble(value_double) :
                                new OpenApiDouble(OpenApiSchemaDefaultValue._double);
                            break;
                    }
                    break;
                case OpenApiSchemaType.@enum:
                    switch (schema.Format)
                    {
                        case OpenApiSchemaFormat.enum_description:
                            any = property.PropertyType.GetEnumOpenApiAny(schemaAttribute?.Value);
                            break;
                        default:
                            any = specialValue ?
                                new OpenApiString(schemaAttribute.Value.ToString()) :
                                new OpenApiString(Enum.GetValues(property.PropertyType).GetValue(0).ToString());
                            break;
                    }
                    break;
                case OpenApiSchemaType.model:
                    if (!innerModel)
                        return new OpenApiString(property.PropertyType.Name);

                    switch (schema.Format)
                    {
                        case OpenApiSchemaFormat.model_once:
                            any = property.PropertyType.GetOrNullFor(false);
                            break;
                        default:
                            any = property.PropertyType.GetOrNullFor(innerModel);
                            break;
                    }
                    break;
                case OpenApiSchemaType.@string:
                default:
                    switch (schema.Format)
                    {
                        case OpenApiSchemaFormat.string_date_original:
                            any = specialValue && schemaAttribute.Value.TryCast(out DateTime value_date_original) ?
                                new OpenApiDate(value_date_original) :
                                new OpenApiDate(OpenApiSchemaDefaultValue._dateTime);
                            break;
                        case OpenApiSchemaFormat.string_date:
                            any = specialValue && schemaAttribute.Value.TryCast(out DateTime value_date) ?
                                new OpenApiString(value_date.ToString("yyyy-MM-dd")) :
                                new OpenApiString(OpenApiSchemaDefaultValue._dateTime.ToString("yyyy-MM-dd"));
                            break;
                        case OpenApiSchemaFormat.string_datetime:
                            any = specialValue && schemaAttribute.Value.TryCast(out DateTime value_datetime) ?
                                new OpenApiString(value_datetime.ToString("yyyy-MM-dd HH:mm:ss")) :
                                new OpenApiString(OpenApiSchemaDefaultValue._dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                            break;
                        default:
                            any = specialValue && schemaAttribute.Value.TryCast(out string value_string) ?
                                new OpenApiString(value_string) :
                                new OpenApiString(OpenApiSchemaDefaultValue._string);
                            break;
                    }
                    break;
            }

            return any;
        }

        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <param name="default">默认值</param>
        /// <returns></returns>
        public static IOpenApiAny GetEnumOpenApiAny(this Type type, object @default = null)
        {
            Dictionary<string, string> NameAndDescriptionDic;
            if (CacheExtention.EnumNameAndDescriptionDic.ContainsKey(type.FullName))
                NameAndDescriptionDic = CacheExtention.EnumNameAndDescriptionDic[type.FullName];
            else
            {
                NameAndDescriptionDic = new Dictionary<string, string>();
                foreach (var item in Enum.GetValues(type))
                {
                    string description;
                    var attr = type.GetField(item.ToString()).GetCustomAttribute<DescriptionAttribute>();

                    if (attr == null)
                        description = $"{item} / {(int)item}";
                    else
                        description = $"{item} / {(int)item} ({attr.Description})";

                    NameAndDescriptionDic.Add(item.ToString(), description);
                }
                CacheExtention.EnumNameAndDescriptionDic.Add(type.FullName, NameAndDescriptionDic);
            }

            string value;
            if (@default == null)
                value = NameAndDescriptionDic?.FirstOrDefault().Key;
            else
                value = Enum.GetName(type, @default);

            return new OpenApiString($"{value} ({string.Join(", ", NameAndDescriptionDic.Select(o => $"{o.Key} : {o.Value}"))})");
        }
    }
}
