using System;
using System.Collections.Generic;
using System.Text;

namespace Integrate_Model.System
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public class RoleDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public RoleType Type { get; set; }
    }

    public enum RoleType
    {
        /// <summary>
        /// 超级管理角色
        /// </summary>
        superAdminRole,
        /// <summary>
        /// 管理角色
        /// </summary>
        secAdminRole,
        /// <summary>
        /// 业务角色
        /// </summary>
        busiRole
    }
}
