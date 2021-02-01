using System;
using System.Collections.Generic;
using System.Text;

namespace Model.System.Config
{
    /// <summary>
    /// swagger接口文档多版本说明配置
    /// </summary>
    public class SwaggerApiMultiVersionDescription
    {
        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }

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
