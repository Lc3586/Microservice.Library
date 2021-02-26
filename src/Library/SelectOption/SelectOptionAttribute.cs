using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.SelectOption
{
    /// <summary>
    /// 选项属性
    /// </summary>
    public class SelectOptionAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SelectOptionAttribute()
        {
            this.NoDropDownFiel = false;
            this.DisplayType = OptionDisplayType.label;
            this.Split = ',';
            this.Order = 0;
            this.Group = null;
            this.GroupOrder = 0;
        }

        /// <summary>
        /// 标记为非下拉框详细信息展示字段,默认[false]
        /// </summary>
        public bool NoDropDownFiel { get; set; }

        /// <summary>
        /// 下拉框数据展示类型,默认[label]
        /// </summary>
        public OptionDisplayType DisplayType { get; set; }

        /// <summary>
        /// 数据拼接分隔符(默认[,])
        /// </summary>
        public char Split { get; set; }

        /// <summary>
        /// 排序值(升序)
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 分组排序值(升序)
        /// </summary>
        public int GroupOrder { get; set; }

        /// <summary>
        /// 文本值来源字段
        /// </summary>
        public string TextField { get; set; }
    }
}
