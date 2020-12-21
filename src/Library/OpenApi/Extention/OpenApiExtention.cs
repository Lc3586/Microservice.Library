using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using Library.OpenApi.Annotations;
using Library.OpenApi.JsonSerialization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Library.OpenApi.Extention
{
    public static class OpenApiExtention
    {
        /// <summary>
        /// 获取主标签名称
        /// <para>默认为类型名称</para>
        /// </summary>
        /// <param name="type">目标对象</param>
        /// <remarks>LCTR 2020-03-10</remarks>
        /// <returns></returns>
        public static string GetMainTag(this Type type)
        {
            var JI = type.GetCustomAttribute(typeof(OpenApiMainTagAttribute));
            return JI == null ? null : ((OpenApiMainTagAttribute)JI).Name;
        }

        /// <summary>
        /// 是否包含标签
        /// </summary>
        /// <param name="element">目标成员</param>
        /// <param name="name">标签名称</param>
        /// <returns></returns>
        public static bool HasTag(this MemberInfo element)
        {
            var JI = element.GetCustomAttribute(typeof(OpenApiSubTagAttribute));
            return JI == null ? false : true;
        }

        /// <summary>
        /// 是否包含标签
        /// </summary>
        /// <param name="element">目标成员</param>
        /// <param name="name">标签名称</param>
        /// <returns></returns>
        public static bool HasTag(this MemberInfo element, string name)
        {
            var JI = element.GetCustomAttribute(typeof(OpenApiSubTagAttribute));
            return JI == null ? false : ((OpenApiSubTagAttribute)JI).Name.Contains(name);
        }

        /// <summary>
        /// 获取动态类型
        /// </summary>
        /// <typeparam name="TOpenApiSchema"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static dynamic CreateDynamicObjectWithTag<TOpenApiSchema>(this TOpenApiSchema obj)
        {
            dynamic dObject = new ExpandoObject();
            var dic = (IDictionary<string, object>)dObject;
            foreach (var prop in typeof(TOpenApiSchema).GetPropertysWithTag())
            {
                dic[prop.Name] = prop.GetValue(obj);
            }
            return dObject;
        }

        /// <summary>
        /// 获取属性集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="inherit">仅继承成员</param>
        /// <param name="otherTags">需要额外匹配的标签</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertysWithTagAndOther(this Type type, bool inherit, params string[] otherTags)
        {
            var tag = type.GetMainTag();
            return type.GetPropertysWithTags(inherit, (otherTags == null ? new string[] { tag } : otherTags.Concat(new string[] { tag })).ToArray());
        }

        /// <summary>
        /// 获取属性集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tags">标签集合</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertysWithTag(this Type type, params string[] tags)
        {
            return type.GetProperties().Where(o => tags.Any(t => o.HasTag(t)));
        }

        /// <summary>
        /// 获取属性集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tags">标签集合</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertysWithTag(this Type type, bool inherit = false)
        {
            var tag = type.GetMainTag();
            return type.GetPropertysWithTags(inherit, tag);
        }

        /// <summary>
        /// 获取属性集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="inherit">仅继承成员</param>
        /// <param name="tags">需要匹配的标签</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertysWithTags(this Type type, bool inherit = false, params string[] tags)
        {
            var strictMode = type.GetCustomAttribute<OpenApiSchemaStrictModeAttribute>() != null;
            var props = inherit ? (type.BaseType ?? type).GetProperties() : type.GetProperties();
            if (tags?.Any() != true)
                return props.Where(o => !strictMode || o.HasTag());
            else
                return props.Where(o => (strictMode && o.DeclaringType?.Name == o.ReflectedType.Name) || tags.Any(t => o.HasTag(t)));
        }

        /// <summary>
        /// 获取字段名称集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tags">标签集合</param>
        /// <returns></returns>
        public static IEnumerable<string> GetNamesWithTag(this Type type, params string[] tags)
        {
            return type.GetPropertysWithTag(tags).Select(o => o.Name);
        }

        /// <summary>
        /// 获取字段名称集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="inherit">仅继承成员</param>
        /// <returns></returns>
        public static IEnumerable<string> GetNamesWithTag(this Type type, bool inherit = false)
        {
            return type.GetPropertysWithTag(inherit).Select(o => o.Name);
        }

        /// <summary>
        /// 获取字段名称集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="inherit">仅继承成员</param>
        /// <param name="otherTags">需要额外匹配的标签</param>
        /// <returns></returns>
        public static IEnumerable<string> GetNamesWithTagAndOther(this Type type, bool inherit = false, params string[] otherTags)
        {
            return type.GetPropertysWithTagAndOther(inherit, otherTags).Select(o => o.Name);
        }

        /// <summary>
        /// 获取字段名称集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tag">标签</param>
        /// <param name="inherit">仅继承成员</param>
        /// <returns></returns>
        public static IEnumerable<string> GetNamesWithTag(this Type type, string tag, bool inherit = false)
        {
            return type.GetPropertysWithTags(inherit, tag).Select(o => o.Name);
        }

        /// <summary>
        /// 获取字段名称和值集合
        /// </summary>
        /// <param name=""></param>
        /// <param name="inherit">仅继承成员</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetNameAndValueWithTag<TOpenApiSchema>(this TOpenApiSchema obj, bool inherit = false)
        {
            return typeof(TOpenApiSchema).GetPropertysWithTag(inherit).ToDictionary(k => k.Name, v => v.GetValue(obj));
        }

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
        /// 转换类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="typedValue"></param>
        /// <returns></returns>
        private static bool TryCast<T>(this object value, out T typedValue)
        {
            try
            {
                typedValue = (T)Convert.ChangeType(value, typeof(T));
                return true;
            }
            catch (InvalidCastException)
            {
                typedValue = default(T);
                return false;
            }
        }

        /// <summary>
        /// 过滤模型
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="after">处理成员之后</param>
        /// <param name="befor">处理成员之前(返回false,跳过该成员)</param>
        /// <param name="innerModel">处理内部模型</param>
        internal static void FilterModel(this Type type, Action<Type, PropertyInfo> after, Func<Type, PropertyInfo, bool> befor = null, bool innerModel = false)
        {
            if (type.IsArray)
                type = type.Assembly.GetType(type.FullName.Replace("[]", string.Empty));
            else if (type.IsGenericType)
                type = type.GenericTypeArguments[0];

            var tag = type.GetMainTag();
            var hasTag = !string.IsNullOrEmpty(tag);
            var strictMode = type.GetCustomAttribute<OpenApiSchemaStrictModeAttribute>() != null;

            foreach (var prop in type.GetProperties())
            {
                if (befor?.Invoke(type, prop) == false)
                    continue;

                if (prop.PropertyType.IsNotPublic)
                    continue;

                var schemaAttribute = prop.GetCustomAttribute<OpenApiSchemaAttribute>();

                if (strictMode && schemaAttribute == null)
                    continue;

                if (prop.GetCustomAttribute<OpenApiIgnoreAttribute>() != null)
                    continue;

                if (prop.DeclaringType?.Name != type.Name)
                    if (hasTag)
                        if (!prop.HasTag(tag))
                            continue;

                if (innerModel && schemaAttribute?.Type == OpenApiSchemaType.model)
                    prop.PropertyType.FilterModel(after, befor, schemaAttribute?.Format != OpenApiSchemaFormat.model_once);

                after.Invoke(type, prop);
            }
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

            var propertyDic = type.GetOrNullForPropertyDic(innerModel);
            foreach (var item in propertyDic)
            {
                var _type = Assembly.Load(CacheExtention.AssemblyOfTypeDic[item.Key]).GetType(item.Key);

                item.Value.ForEach(p =>
                {
                    var prop = _type.GetProperty(p);

                    IOpenApiAny any = prop.PropertyType.IsArray
                        || prop.PropertyType.IsGenericType ?
                            new OpenApiArray { prop.CreateFor(innerModel) } :
                            prop.CreateFor(innerModel);

                    example.Add(p, any);
                });
            }

            if (!CacheExtention.OpenApiObjectDic.ContainsKey(type.FullName))
                CacheExtention.OpenApiObjectDic.Add(type.FullName, example);

            return example;
        }

        /// <summary>
        /// 获取或创建架构属性信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="innerModel">处理内部模型</param>
        /// <param name="exceptionProperties">特别输出的属性</param>
        /// <param name="ignoreProperties">特别忽略的属性</param>
        /// <remarks>如果在特别输出参数和特别忽略参数中同时指定了同一个属性，那么最终不会输出该属性</remarks>
        /// <returns></returns>
        public static Dictionary<string, List<string>> GetOrNullForPropertyDic(this Type type, bool innerModel = true, Dictionary<string, List<string>> exceptionProperties = null, Dictionary<string, List<string>> ignoreProperties = null)
        {
            var propertyDic = type.GetPropertysOfTypeDic();

            if (propertyDic.Any())
                goto end;

            type.FilterModel((_type, prop) =>
            {
                if (!propertyDic.ContainsKey(_type.FullName))
                    propertyDic.Add(_type.FullName, new List<string>() { prop.Name });
                else
                    propertyDic[_type.FullName].Add(prop.Name);

                if (!CacheExtention.AssemblyOfTypeDic.ContainsKey(_type.FullName))
                    CacheExtention.AssemblyOfTypeDic.Add(_type.FullName, _type.Assembly.FullName);
            },
            null,
            //(_type, prop) =>
            //{
            //    return exceptionProperties?[_type]?.Contains(prop.Name) == true || ignoreProperties?[_type]?.Contains(prop.Name) != true;
            //},
            innerModel);

            type.SetPropertysOfTypeDic(propertyDic);

            end:
            if (exceptionProperties != null)
                foreach (var item in exceptionProperties)
                {
                    if (!propertyDic.ContainsKey(item.Key))
                    {
                        propertyDic.Add(item.Key, item.Value);
                    }
                    else
                        propertyDic[item.Key] = propertyDic[item.Key].Union(item.Value).ToList();
                }

            if (ignoreProperties != null)
                foreach (var item in ignoreProperties)
                {
                    if (propertyDic.ContainsKey(item.Key))
                        propertyDic[item.Key] = propertyDic[item.Key].Except(item.Value).ToList();
                }

            return propertyDic;
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
                            any = GetEnumOpenApiAny(property.PropertyType, schemaAttribute?.Value);
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
        private static IOpenApiAny GetEnumOpenApiAny(this Type type, object @default = null)
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
