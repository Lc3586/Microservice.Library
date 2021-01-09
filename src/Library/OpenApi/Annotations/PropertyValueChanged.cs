using System;
using System.Collections.Generic;
using System.Text;

namespace Library.OpenApi.Annotations
{
    /// <summary>
    /// 属性值更改信息
    /// </summary>
    public class PropertyValueChanged
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 以前的值
        /// </summary>
        public object FormerValue { get; set; }

        /// <summary>
        /// 现在的值
        /// </summary>
        public object CurrentValue { get; set; }
    }
}
