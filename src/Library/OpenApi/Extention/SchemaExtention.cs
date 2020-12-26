using Library.OpenApi.Annotations;
using Microsoft.OpenApi.Any;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Library.OpenApi.Extention
{
    /// <summary>
    /// 架构相关拓展方法
    /// </summary>
    public static class SchemaExtention
    {
        static SchemaExtention()
        {
            foreach (var method in typeof(Enumerable).GetMethods())
            {
                switch (method.Name)
                {
                    case "Count":
                        if (method.GetParameters().Length != 1)
                            break;
                        EnumerableMethods.Add("Count", method);
                        break;
                    case "ElementAt":
                        EnumerableMethods.Add("ElementAt", method);
                        break;
                    default:
                        break;
                }
            }
        }

        #region 内部成员

        /// <summary>
        /// Enumerable类常用方法
        /// </summary>
        internal static readonly Dictionary<string, MethodInfo> EnumerableMethods = new Dictionary<string, MethodInfo>();

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

                after?.Invoke(type, prop);
            }
        }

        /// <summary>
        /// 反序列化后过滤属性
        /// </summary>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="exceptionProperties">特别输出的属性</param>
        /// <param name="ignoreProperties">特别忽略的属性</param>
        /// <remarks>如果在特别输出参数和特别忽略参数中同时指定了同一个属性，那么最终不会输出该属性</remarks>
        /// <returns></returns>
        internal static TOpenApiSchema ToOpenApiObjectFilterWhenAfter<TOpenApiSchema>(this string json, Dictionary<string, List<string>> exceptionProperties = null, Dictionary<string, List<string>> ignoreProperties = null)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default;

            var @object = JsonConvert.DeserializeObject<TOpenApiSchema>(json);

            if (@object.Equals(default(TOpenApiSchema)))
                return default;

            var type = typeof(TOpenApiSchema);

            var propertyDic = type.GetOrNullForPropertyDic(true, exceptionProperties, ignoreProperties);

            FilterOpenApiObject(@object, type, propertyDic);

            return @object;
        }

        /// <summary>
        /// 过滤属性
        /// </summary>
        /// <param name="object"></param>
        /// <param name="type"></param>
        /// <param name="propertyDic"></param>
        internal static void FilterOpenApiObject(object @object, Type type, Dictionary<string, List<string>> propertyDic)
        {
            if (@object == null)
                return;

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
                Foreach(@object, type, propertyDic);
            else
            {
                foreach (var prop in type.GetProperties())
                {
                    if (!propertyDic[type.FullName].Contains(prop.Name))
                    {
                        prop.SetValue(@object, default);
                        continue;
                    }

                    var schemaAttribute = prop.GetCustomAttribute<OpenApiSchemaAttribute>();
                    if (schemaAttribute?.Type == OpenApiSchemaType.model)
                        FilterOpenApiObject(prop.GetValue(@object), prop.PropertyType, propertyDic);
                }
            }
        }

        /// <summary>
        /// 遍历集合
        /// </summary>
        /// <param name="objectList"></param>
        /// <param name="type"></param>
        /// <param name="propertyDic"></param>
        internal static void Foreach(object objectList, Type type, Dictionary<string, List<string>> propertyDic)
        {
            var count = (int)EnumerableMethods["Count"].MakeGenericMethod(type)
                                                        .Invoke(objectList, new object[] { objectList });

            for (int i = 0; i < count; i++)
            {
                var @object = EnumerableMethods["ElementAt"].MakeGenericMethod(type)
                                                            .Invoke(null, new object[] { objectList, i });

                FilterOpenApiObject(@object, type, propertyDic);
            }
        }

        /// <summary>
        /// 过滤属性后反序列化
        /// </summary>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="exceptionProperties">特别输出的属性</param>
        /// <param name="ignoreProperties">特别忽略的属性</param>
        /// <remarks>如果在特别输出参数和特别忽略参数中同时指定了同一个属性，那么最终不会输出该属性</remarks>
        /// <returns></returns>
        internal static TOpenApiSchema ToOpenApiObjectFilterWhenBefor<TOpenApiSchema>(this string json, Dictionary<string, List<string>> exceptionProperties = null, Dictionary<string, List<string>> ignoreProperties = null)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default;

            var type = typeof(TOpenApiSchema);

            var propertyDic = type.GetOrNullForPropertyDic(true, exceptionProperties, ignoreProperties);

            var jt = JToken.Parse(json);

            FilterOpenApiObject(jt, type, propertyDic);

            return jt.ToObject<TOpenApiSchema>();
        }

        /// <summary>
        /// 过滤属性
        /// </summary>
        /// <param name="jt"></param>
        /// <param name="type"></param>
        /// <param name="propertyDic"></param>
        internal static void FilterOpenApiObject(JToken jt, Type type, Dictionary<string, List<string>> propertyDic)
        {
            if (jt == null)
                return;

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
            {
                Foreach(jt, type, propertyDic);
            }
            else
            {
                var jp = jt.First as JProperty;
                while (jp != null)
                {
                    var jp_next = jp.Next as JProperty;

                    if (!propertyDic[type.FullName].Contains(jp.Name))
                        jp.Remove();
                    else
                    {
                        var prop = type.GetProperty(jp.Name);
                        var schemaAttribute = prop.GetCustomAttribute<OpenApiSchemaAttribute>();
                        if (schemaAttribute?.Type == OpenApiSchemaType.model)
                            FilterOpenApiObject(jp.Value, prop.PropertyType, propertyDic);
                    }

                    jp = jp_next;
                }
            }
        }

        /// <summary>
        /// 遍历集合
        /// </summary>
        /// <param name="jt"></param>
        /// <param name="type"></param>
        /// <param name="propertyDic"></param>
        internal static void Foreach(JToken jt, Type type, Dictionary<string, List<string>> propertyDic)
        {
            foreach (var item in jt)
            {
                FilterOpenApiObject(item, type, propertyDic);
            }
        }

        #endregion

        /// <summary>
        /// 获取主标签名称
        /// <para>默认为类型名称</para>
        /// </summary>
        /// <param name="type">目标对象</param>
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
        /// <returns></returns>
        public static bool HasTag(this MemberInfo element)
        {
            var JI = element.GetCustomAttribute(typeof(OpenApiSubTagAttribute));
            return JI != null;
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
            return JI != null && ((OpenApiSubTagAttribute)JI).Name.Contains(name);
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
    }
}
