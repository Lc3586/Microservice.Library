using Microservice.Library.OpenApi.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/* 
 * 授权相关业务模型
 */
namespace Model.System.AuthorizeDTO
{
    /// <summary>
    /// 授权角色给用户
    /// </summary>
    public class RoleForUser
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [MinLength(1, ErrorMessage = "最少指定一个用户")]
        public List<string> UserIds { get; set; } = new List<string>();

        /// <summary>
        /// 角色Id
        /// </summary>
        [MinLength(1, ErrorMessage = "最少指定一个角色")]
        public List<string> RoleIds { get; set; } = new List<string>();

        /// <summary>
        /// 撤销已授权给用户但未包含在RoleIds参数中的角色授权
        /// </summary>
        public bool RevocationOtherRoles { get; set; } = false;

        /// <summary>
        /// 全部角色
        /// </summary>
        [OpenApiIgnore]
        [JsonIgnore]
        public bool All { get; set; } = false;
    }

    /// <summary>
    /// 授权角色给会员
    /// </summary>
    public class RoleForMember
    {
        /// <summary>
        /// 会员Id
        /// </summary>
        [MinLength(1, ErrorMessage = "最少指定一个会员")]
        public List<string> MemberIds { get; set; } = new List<string>();

        /// <summary>
        /// 角色Id
        /// </summary>
        [MinLength(1, ErrorMessage = "最少指定一个角色")]
        public List<string> RoleIds { get; set; } = new List<string>();

        /// <summary>
        /// 撤销已授权给会员但未包含在RoleIds参数中的角色授权
        /// </summary>
        public bool RevocationOtherRoles { get; set; } = false;

        /// <summary>
        /// 全部角色
        /// </summary>
        [OpenApiIgnore]
        [JsonIgnore]
        public bool All { get; set; } = false;
    }

    /// <summary>
    /// 授权菜单给用户
    /// </summary>
    public class MenuForUser
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [MinLength(1, ErrorMessage = "最少指定一个用户")]
        public List<string> UserIds { get; set; } = new List<string>();

        /// <summary>
        /// 菜单Id
        /// </summary>
        [MinLength(1, ErrorMessage = "最少指定一个菜单")]
        public List<string> MenuIds { get; set; } = new List<string>();

        /// <summary>
        /// 全部菜单
        /// </summary>
        [OpenApiIgnore]
        [JsonIgnore]
        public bool All { get; set; } = false;
    }

    /// <summary>
    /// 授权资源给用户
    /// </summary>
    public class ResourcesForUser
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [MinLength(1, ErrorMessage = "最少指定一个用户")]
        public List<string> UserIds { get; set; } = new List<string>();

        /// <summary>
        /// 资源Id
        /// </summary>
        [MinLength(1, ErrorMessage = "最少指定一个资源")]
        public List<string> ResourcesIds { get; set; } = new List<string>();

        /// <summary>
        /// 全部资源
        /// </summary>
        [OpenApiIgnore]
        [JsonIgnore]
        public bool All { get; set; } = false;
    }

    /// <summary>
    /// 授权菜单给角色
    /// </summary>
    public class MenuForRole
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [MinLength(1, ErrorMessage = "最少指定一个角色")]
        public List<string> RoleIds { get; set; } = new List<string>();

        /// <summary>
        /// 菜单Id
        /// </summary>
        [MinLength(1, ErrorMessage = "最少指定一个菜单")]
        public List<string> MenuIds { get; set; } = new List<string>();

        /// <summary>
        /// 全部菜单
        /// </summary>
        [OpenApiIgnore]
        [JsonIgnore]
        public bool All { get; set; } = false;
    }

    /// <summary>
    /// 授权资源给角色
    /// </summary>
    public class ResourcesForRole
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [MinLength(1, ErrorMessage = "最少指定一个角色")]
        public List<string> RoleIds { get; set; } = new List<string>();

        /// <summary>
        /// 资源Id
        /// </summary>
        [MinLength(1, ErrorMessage = "最少指定一个资源")]
        public List<string> ResourcesIds { get; set; } = new List<string>();

        /// <summary>
        /// 全部资源
        /// </summary>
        [OpenApiIgnore]
        [JsonIgnore]
        public bool All { get; set; } = false;
    }
}
