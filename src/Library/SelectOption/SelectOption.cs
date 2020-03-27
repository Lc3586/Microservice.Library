using System;
using System.Collections.Generic;
using System.Text;

namespace Library.SelectOption
{
    /// <summary>
    /// 选项集合
    /// </summary>
    public class SelectOption
    {
        /// <summary>
        /// 值
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 附加搜索值
        /// </summary>
        public string search { get; set; }

        /// <summary>
        /// 附加类型值
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 选项集合
        /// </summary>
        public List<OptionInfo> options { get; set; }
    }
}
