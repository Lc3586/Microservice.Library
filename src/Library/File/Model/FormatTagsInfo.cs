using System;

namespace Microservice.Library.File.Model
{
    /// <summary>
    /// 格式标签信息
    /// </summary>
    public class FormatTagsInfo
    {
        /// <summary>
        /// 主品牌
        /// </summary>
        public string Major_Brand { get; set; }

        /// <summary>
        /// 次要版本
        /// </summary>
        public string Minor_Version { get; set; }

        /// <summary>
        /// 兼容性品牌
        /// </summary>
        public string Compatible_Brands { get; set; }

        /// <summary>
        /// 编码器
        /// </summary>
        public string Encoder { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Creation_Time { get; set; }
    }
}
