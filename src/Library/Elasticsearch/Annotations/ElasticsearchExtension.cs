using Microservice.Library.Extension;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.Elasticsearch.Annotations
{
    /// <summary>
    /// ES拓展方法
    /// </summary>
    public static class ElasticsearchExtension
    {
        /// <summary>
        /// 获取版本
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetVersion(this Type type)
        {
            var attributes_Indices = type.GetCustomAttributes(typeof(ElasticsearchIndiceExtensionAttribute), true);
            if (attributes_Indices == null || attributes_Indices.Length == 0)
                return null;
            return (attributes_Indices[0] as ElasticsearchIndiceExtensionAttribute).Version;
        }

        /// <summary>
        /// 获取关系名称
        /// <para>来源之一：ElasticsearchTypeAttribute.RelationName</para>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetRelationName(this Type type)
        {
            var attributes_Type = type.GetCustomAttributes(typeof(ElasticsearchTypeAttribute), true);
            if (attributes_Type == null || attributes_Type.Length == 0)
                return type.Name;
            return (attributes_Type[0] as ElasticsearchTypeAttribute).RelationName.ToLower();
        }

        /// <summary>
        /// 获取索引名称
        /// </summary>
        /// <param name="type"></param>
        /// <param name="state">日期(分库功能参数,如未指定,默认为当前日期）</param>
        /// <param name="all">通配符</param>
        /// <returns></returns>
        public static string GetIndicesName(this Type type, DateTime? state = null, string all = null)
        {
            if (state == null)
                state = DateTime.Now;
            string result = GetRelationName(type);
            var version = type.GetVersion();
            if (!version.IsNullOrEmpty())
                result = $"{result}_{version}";
            string sub = null;
            var attributes_Indices = type.GetCustomAttributes(typeof(ElasticsearchIndiceExtensionAttribute), true);
            if (attributes_Indices != null && attributes_Indices.Length > 0)
            {
                var sub_type = (attributes_Indices[0] as ElasticsearchIndiceExtensionAttribute).IndicesSubType;
                if (sub_type != NestIndexSubType.None && !all.IsNullOrEmpty())
                    return $"{result}_{all}".ToLower();

                switch (sub_type)
                {
                    case NestIndexSubType.None:
                        break;
                    case NestIndexSubType.Day:
                        sub = state.Value.ToString("yyyy_MM_dd");
                        break;
                    case NestIndexSubType.Week:
                        sub = state.Value.Year + "_" + state.Value.GetWeekOfYear().ToString();
                        break;
                    case NestIndexSubType.Month:
                        sub = state.Value.ToString("yyyy_MM");
                        break;
                    case NestIndexSubType.Quarter:
                        sub = state.Value.Year + "_" + (state.Value.Month / 3 + (state.Value.Month % 3 != 0 ? 1 : 0)).ToString();
                        break;
                    case NestIndexSubType.HalfYear:
                        sub = state.Value.Year + "_" + (state.Value.Month / 6 + (state.Value.Month % 6 != 0 ? 1 : 0)).ToString();
                        break;
                    case NestIndexSubType.Year:
                        sub = state.Value.Year.ToString();
                        break;
                    default:
                        break;
                }
            }

            if (sub.IsNullOrEmpty())
                return result.ToLower();
            else
                return $"{result}_{sub}".ToLower();
        }

        /// <summary>
        /// 是否自动创建
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsAutoCreate(this Type type)
        {
            var attributes_Indices = type.GetCustomAttributes(typeof(ElasticsearchIndiceExtensionAttribute), true);
            if (attributes_Indices == null || attributes_Indices.Length == 0)
                return false;
            return (attributes_Indices[0] as ElasticsearchIndiceExtensionAttribute).AutoCreate;
        }

        /// <summary>
        /// 是否自动更新设置
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsAutoUpdateSetting(this Type type)
        {
            var attributes_Indices = type.GetCustomAttributes(typeof(ElasticsearchIndiceExtensionAttribute), true);
            if (attributes_Indices == null || attributes_Indices.Length == 0)
                return false;
            return (attributes_Indices[0] as ElasticsearchIndiceExtensionAttribute).AutoUpdateSetting;
        }

        /// <summary>
        /// 类型是否开启动态映射
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDynamic(this Type type)
        {
            var attributes_Indices = type.GetCustomAttributes(typeof(ElasticsearchIndiceExtensionAttribute), true);
            if (attributes_Indices == null || attributes_Indices.Length == 0)
                return false;
            return (attributes_Indices[0] as ElasticsearchIndiceExtensionAttribute).Dynamic;
        }
    }
}
