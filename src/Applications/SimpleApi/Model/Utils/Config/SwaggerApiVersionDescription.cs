using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Utils.Config
{
    /// <summary>
    /// swagger接口文档说明配置
    /// </summary>
    public class SwaggerApiVersionDescription
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
    }
}
