namespace Microservice.Library.Extension.Model
{
    /// <summary>
    /// 统计数据模型
    /// </summary>
    public class DbStatisData
    {
        /// <summary>
        /// 分组查询的列
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 统计后的数值
        /// </summary>
        public double? Value { get; set; }
    }
}
