using System;
using System.Collections.Generic;
using System.Text;

namespace Integrate_Model.System
{
    /// <summary>
    /// 菜单权限信息
    /// </summary>
    public class MenuAuthoritieDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 父Id
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        public int OrderNum { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 默认展开
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 激活状态
        /// </summary>
        public bool ActiveFlag { get; set; }

        /// <summary>
        /// 最下级
        /// </summary>
        public bool IsLeaf { get; set; }

        /// <summary>
        /// 菜单路径
        /// </summary>
        public string MenuPath { get; set; }
    }
}
