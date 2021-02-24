using System.Collections.Generic;

namespace Model.Utils.Config
{
    /// <summary>
    /// Swagger配置
    /// </summary>
    public class SwaggerSetting
    {
        /// <summary>
        /// 说明文档相对路径
        /// </summary>
        public List<string> XmlComments { get; set; }

        /// <summary>
        /// 接口版本说明
        /// </summary>
        public SwaggerApiVersionDescription ApiVersion { get; set; }

        /// <summary>
        /// 启用接口多版本说明
        /// </summary>
        public bool EnableApiMultiVersion { get; set; }

        /// <summary>
        /// 接口多版本说明
        /// </summary>
        public List<SwaggerApiMultiVersionDescription> ApiMultiVersion { get; set; }
    }
}
