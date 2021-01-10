﻿using Library.OpenApi.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
        public List<string> UserIds { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [MinLength(1, ErrorMessage = "最少指定一个角色")]
        public List<string> RoleIds { get; set; }

        /// <summary>
        /// 撤销全部角色
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
        public List<string> UserIds { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        [MinLength(1, ErrorMessage = "最少指定一个菜单")]
        public List<string> MenuIds { get; set; }

        /// <summary>
        /// 撤销全部菜单
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
        public List<string> UserIds { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        [MinLength(1, ErrorMessage = "最少指定一个资源")]
        public List<string> ResourcesIds { get; set; }

        /// <summary>
        /// 撤销全部资源
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
        public List<string> RoleIds { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        [MinLength(1, ErrorMessage = "最少指定一个菜单")]
        public List<string> MenuIds { get; set; }

        /// <summary>
        /// 撤销全部菜单
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
        public List<string> RoleIds { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        [MinLength(1, ErrorMessage = "最少指定一个资源")]
        public List<string> ResourcesIds { get; set; }

        /// <summary>
        /// 撤销全部资源
        /// </summary>
        [OpenApiIgnore]
        [JsonIgnore]
        public bool All { get; set; } = false;
    }
}