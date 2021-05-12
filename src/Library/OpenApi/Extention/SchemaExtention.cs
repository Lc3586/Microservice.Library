using Microservice.Library.OpenApi.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Microservice.Library.OpenApi.Extention
{
    /// <summary>
    /// 架构相关拓展方法
    /// </summary>
    public static class SchemaExtention
    {
        #region 内部成员

        /// <summary>
        /// 获取数组和泛型集合元素类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static Type GetGenericType(this Type type)
        {
            return type.IsArray
                ? type.Assembly.GetType(type.FullName.Replace("[]", string.Empty))
                : (type.IsGenericType && type.GenericTypeArguments.Length == 1 && typeof(IEnumerable).IsAssignableFrom(type)
                    ? type.GenericTypeArguments[0]
                    : type);
        }

        /// <summary>
        /// 获取数组和泛型集合元素类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericType">泛型元素类型</param>
        /// <returns>是否为数组或泛型集合</returns>
        internal static bool GetGenericType(this Type type, out Type genericType)
        {
            genericType = GetGenericType(type);
            return type.IsArray || type.IsGenericType;
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="typedValue"></param>
        /// <returns></returns>
        internal static bool TryCast<T>(this object value, out T typedValue)
        {
            try
            {
                typedValue = (T)Convert.ChangeType(value, typeof(T));
                return true;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (InvalidCastException)
            {
                typedValue = default;
                return false;
            }
#pragma warning restore CA1031 // Do not catch general exception types
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
            type = type.GetGenericType();

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

                after?.Invoke(type, prop);
            }
        }

        #endregion

        /// <summary>
        /// 获取主标签名称
        /// </summary>
        /// <param name="type">目标对象</param>
        /// <returns></returns>
        public static string GetMainTag(this Type type)
        {
            var attribute = type.GetCustomAttribute<OpenApiMainTagAttribute>();
            return attribute == null ? null : attribute.Name;
        }

        /// <summary>
        /// 是否包含标签
        /// </summary>
        /// <param name="element">目标成员</param>
        /// <returns></returns>
        public static bool HasTag(this MemberInfo element) => element.GetCustomAttribute<OpenApiSubTagAttribute>() != null;

        /// <summary>
        /// 是否包含标签
        /// </summary>
        /// <param name="element">目标成员</param>
        /// <param name="name">标签名称</param>
        /// <returns></returns>
        public static bool HasTag(this MemberInfo element, string name) => element.GetCustomAttribute<OpenApiSubTagAttribute>()?.Name.Contains(name) == true;

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
        /// <param name="inherit">仅继承成员</param>
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
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="obj"></param>
        /// <param name="inherit">仅继承成员</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetNameAndValueWithTag<TOpenApiSchema>(this TOpenApiSchema obj, bool inherit = false)
        {
            return typeof(TOpenApiSchema).GetPropertysWithTag(inherit).ToDictionary(k => k.Name, v => v.GetValue(obj));
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
            var propertyDic = type.GetPropertysOfTypeDic(exceptionProperties?.Any() == true && ignoreProperties?.Any() == true);

            if (propertyDic.Any())
                goto end;

            type.FilterModel((_type, prop) =>
            {
                if (!propertyDic.ContainsKey(_type.FullName))
                    propertyDic.Add(_type.FullName, new List<string>() { prop.Name });
                else
                    propertyDic[_type.FullName].Add(prop.Name);

                if (!CacheExtention.AssemblyOfTypeDic.ContainsKey(_type.FullName))
                    CacheExtention.AssemblyOfTypeDic.AddOrUpdate(_type.FullName, _type.Assembly.FullName, (key, old) => _type.Assembly.FullName);
            },
            null,
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
        /// 获取数据更改信息
        /// </summary>
        /// <typeparam name="TComparison">比对数据的类型</typeparam>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="former">以前的数据</param>
        /// <param name="current">当前的数据</param>
        /// <returns></returns>
        public static List<PropertyValueChanged> GetPropertyValueChangeds<TComparison, TOpenApiSchema>(this TComparison former, TComparison current)
        {
            var tag = typeof(TOpenApiSchema).GetMainTag();

            return typeof(TComparison).GetProperties()
                .Where(p => p.HasTag(tag))
                .Select(p => new PropertyValueChanged
                {
                    Description = p.GetCustomAttribute<DescriptionAttribute>(true)?.Description,
                    Name = p.Name,
                    FormerValue = p.GetValue(former),
                    CurrentValue = p.GetValue(current)
                })
                .Where(p => p.FormerValue?.Equals(p.CurrentValue) == false)
                .ToList();
        }
    }
}
