using Library.Extension;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Elasticsearch.Annotations
{
    /// <summary>
    ///Elasticsearch索引拓展属性
    ///<!--LCTR 2019-07-31-->
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ElasticsearchIndiceExtensionAttribute : Attribute
    {
        public ElasticsearchIndiceExtensionAttribute()
        {
            Version = null;
            AutoCreate = true;
            AutoUpdateSetting = true;
            IndicesSubType = NestIndexSubType.None;
            Dynamic = true;
        }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 自动创建
        /// </summary>
        public bool AutoCreate { get; set; }

        /// <summary>
        /// 自动更新设置
        /// </summary>
        public bool AutoUpdateSetting { get; set; }

        /// <summary>
        /// 索引分库类型
        /// </summary>
        public NestIndexSubType IndicesSubType { get; set; }

        /// <summary>
        /// 动态映射
        /// </summary>
        public bool Dynamic { get; set; }
    }
}
