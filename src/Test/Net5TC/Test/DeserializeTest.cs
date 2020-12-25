using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Coldairarrow.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.OpenApi.JsonSerialization;
using Library.OpenApi.Extention;
using Library.OpenApi.Annotations;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Net5TC.Test
{
    /// <summary>
    /// 反序列化测试
    /// </summary>
    [SimpleJob(RuntimeMoniker.Net461, baseline: true)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [SimpleJob(RuntimeMoniker.NetCoreApp50)]
    [RPlotExporter]
    public class DeserializeTest
    {
        public static string Json = string.Empty;

        public List<DTO.DB_ADTO.List> ObjectList;

        public Dictionary<string, MethodInfo> EnumerableMethods = new Dictionary<string, MethodInfo>();

        [GlobalSetup]
        public void Setup()
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

        [Benchmark(Baseline = true, Description = "不过滤属性的反序列化")]
        public void ToObjectWithoutFilter()
        {
            ObjectList = JsonConvert.DeserializeObject<List<DTO.DB_ADTO.List>>(Json);
        }

        [Benchmark(Description = "反序列化后过滤属性")]
        public void ToObjectFilterWhenAfter()
        {
            ObjectList = ToOpenApiObject<List<DTO.DB_ADTO.List>>(Json);

            T ToOpenApiObject<T>(string json)
            {
                if (string.IsNullOrWhiteSpace(json))
                    return default;

                var @object = JsonConvert.DeserializeObject<T>(Json);

                if (@object.Equals(default(T)))
                    return default;

                var type = typeof(T);

                var propertyDic = type.GetOrNullForPropertyDic(true);

                FilterOpenApiObject(@object, type, propertyDic);

                return @object;
            }

            void Foreach(object objectList, Type type, Dictionary<string, List<string>> propertyDic)
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

            void FilterOpenApiObject(object @object, Type type, Dictionary<string, List<string>> propertyDic)
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
        }

        [Benchmark(Description = "过滤属性后反序列化")]
        public void ToObjectFilterWhenBefor()
        {
            ObjectList = ToOpenApiObject<List<DTO.DB_ADTO.List>>(Json);

            T ToOpenApiObject<T>(string json)
            {
                if (string.IsNullOrWhiteSpace(json))
                    return default;

                var type = typeof(T);

                var propertyDic = type.GetOrNullForPropertyDic(true);

                var jt = JToken.Parse(json);

                FilterOpenApiObject(jt, type, propertyDic);

                return jt.ToObject<T>();
            }

            void Foreach(JToken jt, Type type, Dictionary<string, List<string>> propertyDic)
            {
                foreach (var item in jt)
                {
                    FilterOpenApiObject(item, type, propertyDic);
                }
            }

            void FilterOpenApiObject(JToken jt, Type type, Dictionary<string, List<string>> propertyDic)
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
        }
    }
}
