using System;
using System.Collections.Generic;
using System.Text;

namespace Library.SelectOption
{
    /// <summary>
    /// 选项信息
    /// </summary>
    public class OptionInfo
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string field { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string display { get; set; }

        /// <summary>
        /// 数据展示类型
        /// </summary>
        public OptionDisplayType displayType { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object value { get; set; }

        /// <summary>
        /// 文本信息（特殊）
        /// </summary>
        public object text { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        public int order { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        public string group { get; set; }

        /// <summary>
        /// 分组排序值
        /// </summary>
        public int groupOrder { get; set; }
    }
}
