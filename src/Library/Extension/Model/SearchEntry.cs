namespace Microservice.Library.Extension.Model
{
    /// <summary>
    /// 统计查询配置项
    /// </summary>
    public struct SearchEntry
    {
        /// <summary>
        /// 统计的列
        /// </summary>
        public string StatisColoum { get; set; }

        /// <summary>
        /// 返回数据列名
        /// </summary>
        public string ResultName { get; set; }

        /// <summary>
        /// 统计方法名（Max,Min,Average,Count()等）
        /// </summary>
        public string FuncName { get; set; }
    }
}
