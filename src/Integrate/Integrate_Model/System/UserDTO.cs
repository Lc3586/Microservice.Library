using System;
using System.Collections.Generic;
using System.Text;

namespace Integrate_Model.System
{
    /// <summary>
    /// 用户信息业务模型
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// 获取缓存键名
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public static string GetCacheKey(string userId) { return "User_" + userId; }

        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime Birthdate { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 国籍
        /// </summary>
        public string Nationality { get; set; }

        /// <summary>
        /// 家庭电话
        /// </summary>
        public string HomeTelephone { get; set; }

        /// <summary>
        /// 家庭住址
        /// </summary>
        public string HomeAddress { get; set; }

        /// <summary>
        /// 移动电话
        /// </summary>
        public string MobileTelephone { get; set; }

        /// <summary>
        /// 办公电话
        /// </summary>
        public string OfficeTelephone { get; set; }

        /// <summary>
        /// 用户来源
        /// </summary>
        public string UserSource { get; set; }

        /// <summary>
        /// 账号启动状态
        /// </summary>
        public bool AccountEnabled { get; set; }

        /// <summary>
        /// 账号锁定状态
        /// </summary>
        public bool AccountLocked { get; set; }

        /// <summary>
        /// 激活状态
        /// </summary>
        public bool ActiveFlag { get; set; }

        /// <summary>
        /// 是否已修改初始密码
        /// </summary>
        public bool InitialPasswordChanged { get; set; }

        /// <summary>
        /// 角色列表
        /// </summary>
        public List<RoleDTO> Roles { get; set; }

        /// <summary>
        /// 菜单权限
        /// </summary>
        public List<MenuAuthoritieDTO> MenuAuthorities { get; set; }
    }
}
