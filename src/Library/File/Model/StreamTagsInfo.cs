using System;

namespace Microservice.Library.File.Model
{
    /// <summary>
    /// 流标签信息
    /// </summary>
    public class StreamTagsInfo
    {
        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 处理器名字
        /// </summary>
        public string Handler_Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Creation_Time { get; set; }
    }
}
