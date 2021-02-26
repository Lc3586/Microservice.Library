using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.Elasticsearch.Annotations
{
    /// <summary>
    /// 索引分库类型
    /// <!--LCTR 2019-07-29-->
    /// </summary>
    public enum NestIndexSubType
    {
        /// <summary>
        /// 不分库
        /// </summary>
        None,
        /// <summary>
        /// 天
        /// </summary>
        Day,
        /// <summary>
        /// 周
        /// </summary>
        Week,
        /// <summary>
        /// 月
        /// </summary>
        Month,
        /// <summary>
        /// 季
        /// </summary>
        Quarter,
        /// <summary>
        /// 半年
        /// </summary>
        HalfYear,
        /// <summary>
        /// 年
        /// </summary>
        Year
    }
}
