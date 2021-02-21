using Api.Controllers.Utils;
using Business.Interface.System;
using Library.Extension;
using Microsoft.AspNetCore.Mvc;
using Model.System.AuthorizeDTO;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Api.Controllers
{
    /// <summary>
    /// 权限接口
    /// </summary>
    [Route("/authorities")]
    [ApiPermission]
    [CheckModel]
    [SwaggerTag("权限接口")]
    public class AuthoritiesController : BaseApiController
    {
        #region DI

        public AuthoritiesController(IAuthoritiesBusiness authoritiesBusiness)
        {
            AuthoritiesBusiness = authoritiesBusiness;
        }

        readonly IAuthoritiesBusiness AuthoritiesBusiness;

        #endregion

        #region 授权接口

        /// <summary>
        /// 为用户授权角色
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        [HttpPost("authorize-role-for-user")]
        public async Task<object> AuthorizeRoleForUser([FromBody] RoleForUser data)
        {
            AuthoritiesBusiness.AuthorizeRoleForUser(data);
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 为会员授权角色
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        [HttpPost("authorize-role-for-member")]
        public async Task<object> AuthorizeRoleForMember([FromBody] RoleForMember data)
        {
            AuthoritiesBusiness.AuthorizeRoleForMember(data);
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 为用户授权菜单
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        [HttpPost("authorize-menu-for-user")]
        public async Task<object> AuthorizeMenuForUser([FromBody] MenuForUser data)
        {
            AuthoritiesBusiness.AuthorizeMenuForUser(data);
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 为用户授权资源
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        [HttpPost("authorize-resources-for-user")]
        public async Task<object> AuthorizeResourcesForUser([FromBody] ResourcesForUser data)
        {
            AuthoritiesBusiness.AuthorizeResourcesForUser(data);
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 为角色授权菜单
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        [HttpPost("authorize-menu-for-role")]
        public async Task<object> AuthorizeMenuForRole([FromBody] MenuForRole data)
        {
            AuthoritiesBusiness.AuthorizeMenuForRole(data);
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 为角色授权资源
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        [HttpPost("authorize-resources-for-role")]
        public async Task<object> AuthorizeResourcesForRole([FromBody] ResourcesForRole data)
        {
            AuthoritiesBusiness.AuthorizeResourcesForRole(data);
            return await Task.FromResult(Success());
        }

        #endregion

        #region 撤销授权接口

        /// <summary>
        /// 撤销角色的全部系统用户授权
        /// </summary>
        /// <param name="roleIds">角色Id集合</param>
        /// <returns></returns>
        [HttpPost("revocation-role-for-all-user")]
        public async Task<object> RevocationRoleForAllUser(IEnumerable<string> roleIds)
        {
            AuthoritiesBusiness.RevocationRoleForAllUser(roleIds?.ToList());
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 撤销角色的全部会员授权
        /// </summary>
        /// <param name="roleIds">角色Id集合</param>
        /// <returns></returns>
        [HttpPost("revocation-role-for-all-member")]
        public async Task<object> RevocationRoleForAllMember(IEnumerable<string> roleIds)
        {
            AuthoritiesBusiness.RevocationRoleForAllMember(roleIds?.ToList());
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 撤销用户的全部角色授权
        /// </summary>
        /// <param name="userIds">用户Id集合</param>
        /// <returns></returns>
        [HttpPost("revocation-all-role-for-user")]
        public async Task<object> RevocationRoleForUser(IEnumerable<string> userIds)
        {
            AuthoritiesBusiness.RevocationRoleForUser(userIds?.ToList());
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 撤销会员的全部角色授权
        /// </summary>
        /// <param name="memberIds">会员Id集合</param>
        /// <returns></returns>
        [HttpPost("revocation-all-role-for-member")]
        public async Task<object> RevocationRoleForMember(IEnumerable<string> memberIds)
        {
            AuthoritiesBusiness.RevocationRoleForMember(memberIds?.ToList());
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 撤销用户的全部菜单授权
        /// </summary>
        /// <param name="userIds">用户Id集合</param>
        /// <returns></returns>
        [HttpPost("revocation-all-menu-for-user")]
        public async Task<object> RevocationMenuForUser(IEnumerable<string> userIds)
        {
            AuthoritiesBusiness.RevocationMenuForUser(userIds?.ToList());
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 撤销用户的全部资源授权
        /// </summary>
        /// <param name="userIds">用户Id集合</param>
        /// <returns></returns>
        [HttpPost("revocation-all-resources-for-user")]
        public async Task<object> RevocationResourcesForUser(IEnumerable<string> userIds)
        {
            AuthoritiesBusiness.RevocationResourcesForUser(userIds?.ToList());
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 撤销角色的全部菜单授权
        /// </summary>
        /// <param name="roleIds">角色Id集合</param>
        /// <returns></returns>
        [HttpPost("revocation-all-menu-for-role")]
        public async Task<object> RevocationMenuForRole(IEnumerable<string> roleIds)
        {
            AuthoritiesBusiness.RevocationMenuForRole(roleIds?.ToList());
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 撤销角色的全部资源授权
        /// </summary>
        /// <param name="roleIds">角色Id集合</param>
        /// <returns></returns>
        [HttpPost("revocation-all-resources-for-role")]
        public async Task<object> RevocationResourcesForRole(IEnumerable<string> roleIds)
        {
            AuthoritiesBusiness.RevocationResourcesForRole(roleIds?.ToList());
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 撤销用户的角色授权
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        [HttpPost("revocation-role-for-user")]
        public async Task<object> RevocationRoleForUser(RoleForUser data)
        {
            AuthoritiesBusiness.RevocationRoleForUser(data);
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 撤销会员的角色授权
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        [HttpPost("revocation-role-for-member")]
        public async Task<object> RevocationRoleForMember(RoleForMember data)
        {
            AuthoritiesBusiness.RevocationRoleForMember(data);
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 撤销用户的菜单授权
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        [HttpPost("revocation-menu-for-user")]
        public async Task<object> RevocationMenuForUser(MenuForUser data)
        {
            AuthoritiesBusiness.RevocationMenuForUser(data);
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 撤销用户的资源授权
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        [HttpPost("revocation-resources-for-user")]
        public async Task<object> RevocationResourcesForUser(ResourcesForUser data)
        {
            AuthoritiesBusiness.RevocationResourcesForUser(data);
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 撤销角色的菜单授权
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        [HttpPost("revocation-menu-for-role")]
        public async Task<object> RevocationMenuForRole(MenuForRole data)
        {
            AuthoritiesBusiness.RevocationMenuForRole(data);
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 撤销角色的资源授权
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        [HttpPost("revocation-resources-for-role")]
        public async Task<object> RevocationResourcesForRole(ResourcesForRole data)
        {
            AuthoritiesBusiness.RevocationResourcesForRole(data);
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 撤销所有用户和角色的菜单授权
        /// </summary>
        /// <param name="menuIds">菜单Id集合</param>
        /// <returns></returns>
        [HttpPost("revocation-menu-for-all")]
        public async Task<object> RevocationMenuForAll(IEnumerable<string> menuIds)
        {
            AuthoritiesBusiness.RevocationMenuForAll(menuIds?.ToList());
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 撤销所有用户和角色的资源授权
        /// </summary>
        /// <param name="resourcesIds">资源Id集合</param>
        /// <returns></returns>
        [HttpPost("revocation-resources-for-all")]
        public async Task<object> RevocationResourcesForAll(IEnumerable<string> resourcesIds)
        {
            AuthoritiesBusiness.RevocationResourcesForAll(resourcesIds?.ToList());
            return await Task.FromResult(Success());
        }

        #endregion

        #region 获取授权接口

        /// <summary>
        /// 获取用户的授权数据
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="includeRole">包括授权角色</param>
        /// <param name="includeMenu">包括授权菜单</param>
        /// <param name="includeResources">包括授权资源</param>
        /// <param name="mergeRoleMenu">合并角色的授权菜单</param>
        /// <param name="mergeRoleResources">合并角色的授权资源</param>
        /// <returns></returns>
        [HttpPost("user-data")]
        [SwaggerResponse((int)HttpStatusCode.OK, "授权数据", typeof(Model.System.UserDTO.Authorities))]
        public async Task<object> GetUser(string userId, bool includeRole, bool includeMenu, bool includeResources, bool mergeRoleMenu = true, bool mergeRoleResources = true)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.GetUser(userId, includeRole, includeMenu, includeResources, mergeRoleMenu, mergeRoleResources)));
        }

        /// <summary>
        /// 获取会员的授权数据
        /// </summary>
        /// <param name="memberId">会员Id</param>
        /// <param name="includeRole">包括授权角色</param>
        /// <param name="includeMenu">包括授权菜单</param>
        /// <param name="includeResources">包括授权资源</param>
        /// <returns></returns>
        [HttpPost("member-data")]
        [SwaggerResponse((int)HttpStatusCode.OK, "授权数据", typeof(Model.Public.MemberDTO.Authorities))]
        public async Task<object> GetMember(string memberId, bool includeRole, bool includeMenu, bool includeResources)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.GetMember(memberId, includeRole, includeMenu, includeResources)));
        }

        /// <summary>
        /// 获取授权给用户的角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="includeMenu">包括授权菜单</param>
        /// <param name="includeResources">包括授权资源</param>
        /// <returns></returns>
        [HttpPost("user-role-data")]
        [SwaggerResponse((int)HttpStatusCode.OK, "授权数据", typeof(Model.System.RoleDTO.Authorities))]
        public async Task<object> GetUserRole(string userId, bool includeMenu, bool includeResources)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.GetUserRole(userId, includeMenu, includeResources)));
        }

        /// <summary>
        /// 获取授权给会员的角色
        /// </summary>
        /// <param name="memberId">会员Id</param>
        /// <param name="includeMenu">包括授权菜单</param>
        /// <param name="includeResources">包括授权资源</param>
        /// <returns></returns>
        [HttpPost("member-role-data")]
        [SwaggerResponse((int)HttpStatusCode.OK, "授权数据", typeof(Model.System.RoleDTO.Authorities))]
        public async Task<object> GetMemberRole(string memberId, bool includeMenu, bool includeResources)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.GetMemberRole(memberId, includeMenu, includeResources)));
        }

        /// <summary>
        /// 获取授权给用户的菜单
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="mergeRoleMenu">包括角色的授权菜单</param>
        /// <returns></returns>
        [HttpPost("user-menu-data")]
        [SwaggerResponse((int)HttpStatusCode.OK, "授权数据", typeof(Model.System.MenuDTO.Authorities))]
        public async Task<object> GetUserMenu(string userId, bool mergeRoleMenu)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.GetUserMenu(userId, mergeRoleMenu)));
        }

        /// <summary>
        /// 获取授权给会员的菜单
        /// </summary>
        /// <param name="memberId">会员Id</param>
        /// <returns></returns>
        [HttpPost("member-menu-data")]
        [SwaggerResponse((int)HttpStatusCode.OK, "授权数据", typeof(Model.System.MenuDTO.Authorities))]
        public async Task<object> GetMemberMenu(string memberId)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.GetMemberMenu(memberId)));
        }

        /// <summary>
        /// 获权授权给用户的资源
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="mergeRoleResources">包括角色的授权资源</param>
        /// <returns></returns>
        [HttpPost("user-resources-data")]
        [SwaggerResponse((int)HttpStatusCode.OK, "授权数据", typeof(Model.System.ResourcesDTO.Authorities))]
        public async Task<object> GetUserResources(string userId, bool mergeRoleResources)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.GetUserResources(userId, mergeRoleResources)));
        }

        /// <summary>
        /// 获权授权给会员的资源
        /// </summary>
        /// <param name="memberId">会员Id</param>
        /// <returns></returns>
        [HttpPost("member-resources-data")]
        [SwaggerResponse((int)HttpStatusCode.OK, "授权数据", typeof(Model.System.ResourcesDTO.Authorities))]
        public async Task<object> GetMemberResources(string memberId)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.GetMemberResources(memberId)));
        }

        /// <summary>
        /// 获取角色的授权数据
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="includeMenu">包括授权菜单</param>
        /// <param name="includeResources">包括授权资源</param>
        /// <returns></returns>
        [HttpPost("role-data")]
        [SwaggerResponse((int)HttpStatusCode.OK, "授权数据", typeof(Model.System.RoleDTO.Authorities))]
        public async Task<object> GetRole(string roleId, bool includeMenu, bool includeResources)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.GetRole(roleId, includeMenu, includeResources)));
        }

        /// <summary>
        /// 获取授权给角色的菜单
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        [HttpPost("role-menu-data")]
        [SwaggerResponse((int)HttpStatusCode.OK, "授权数据", typeof(Model.System.MenuDTO.Authorities))]
        public async Task<object> GetRoleMenu(string roleId)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.GetRoleMenu(roleId)));
        }

        /// <summary>
        /// 获取授权给角色的资源
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        [HttpPost("role-resources-data")]
        [SwaggerResponse((int)HttpStatusCode.OK, "授权数据", typeof(Model.System.ResourcesDTO.Authorities))]
        public async Task<object> GetRoleResources(string roleId)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.GetRoleResources(roleId)));
        }

        #endregion

        #region 验证授权接口

        /// <summary>
        /// 是否为管理员
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [HttpPost("is-admin-user")]
        [SwaggerResponse((int)HttpStatusCode.OK, "验证结果", typeof(bool))]
        public async Task<object> IsAdminUser(string userId)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.IsAdminUser(userId)));
        }

        /// <summary>
        /// 是否为管理角色
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        [HttpPost("is-admin-role")]
        [SwaggerResponse((int)HttpStatusCode.OK, "验证结果", typeof(bool))]
        public async Task<object> IsAdminRole(string roleId)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.IsAdminRole(roleId)));
        }

        /// <summary>
        /// 用户是否拥有角色授权
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        [HttpPost("user-has-role")]
        [SwaggerResponse((int)HttpStatusCode.OK, "验证结果", typeof(bool))]
        public async Task<object> UserHasRole(string userId, string roleId)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.UserHasRole(userId, roleId)));
        }

        /// <summary>
        /// 会员是否拥有角色授权
        /// </summary>
        /// <param name="memberId">会员Id</param>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        [HttpPost("member-has-role")]
        [SwaggerResponse((int)HttpStatusCode.OK, "验证结果", typeof(bool))]
        public async Task<object> MemberHasRole(string memberId, string roleId)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.MemberHasRole(memberId, roleId)));
        }

        /// <summary>
        /// 用户是否拥有菜单授权
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="menuId">菜单Id</param>
        /// <returns></returns>
        [HttpPost("user-has-menu")]
        [SwaggerResponse((int)HttpStatusCode.OK, "验证结果", typeof(bool))]
        public async Task<object> UserHasMenu(string userId, string menuId)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.UserHasMenu(userId, menuId)));
        }

        /// <summary>
        /// 用户是否拥有菜单授权
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="menuUri">菜单链接</param>
        /// <returns></returns>
        [HttpPost("user-has-menu-uri")]
        [SwaggerResponse((int)HttpStatusCode.OK, "验证结果", typeof(bool))]
        public async Task<object> UserHasMenuUri(string userId, string menuUri)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.UserHasMenuUri(userId, menuUri)));
        }

        /// <summary>
        /// 会员是否拥有菜单授权
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="menuId">菜单Id</param>
        /// <returns></returns>
        [HttpPost("member-has-menu")]
        [SwaggerResponse((int)HttpStatusCode.OK, "验证结果", typeof(bool))]
        public async Task<object> MemberHasMenu(string userId, string menuId)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.MemberHasMenu(userId, menuId)));
        }

        /// <summary>
        /// 会员是否拥有菜单授权
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="menuUri">菜单链接</param>
        /// <returns></returns>
        [HttpPost("member-has-menu-uri")]
        [SwaggerResponse((int)HttpStatusCode.OK, "验证结果", typeof(bool))]
        public async Task<object> MemberHasMenuUri(string userId, string menuUri)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.MemberHasMenuUri(userId, menuUri)));
        }

        /// <summary>
        /// 用户是否拥有资源授权
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="resourcesId">资源Id</param>
        /// <returns></returns>
        [HttpPost("user-has-resources")]
        [SwaggerResponse((int)HttpStatusCode.OK, "验证结果", typeof(bool))]
        public async Task<object> UserHasResources(string userId, string resourcesId)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.UserHasResources(userId, resourcesId)));
        }

        /// <summary>
        /// 用户是否拥有资源授权
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="resourcesUri">资源链接</param>
        /// <returns></returns>
        [HttpPost("user-has-resources-uri")]
        [SwaggerResponse((int)HttpStatusCode.OK, "验证结果", typeof(bool))]
        public async Task<object> UserHasResourcesUri(string userId, string resourcesUri)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.UserHasResourcesUri(userId, resourcesUri)));
        }

        /// <summary>
        /// 会员是否拥有资源授权
        /// </summary>
        /// <param name="memberId">会员Id</param>
        /// <param name="resourcesId">资源Id</param>
        /// <returns></returns>
        [HttpPost("member-has-resources")]
        [SwaggerResponse((int)HttpStatusCode.OK, "验证结果", typeof(bool))]
        public async Task<object> MemberHasResources(string memberId, string resourcesId)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.MemberHasResources(memberId, resourcesId)));
        }

        /// <summary>
        /// 会员是否拥有资源授权
        /// </summary>
        /// <param name="memberId">会员Id</param>
        /// <param name="resourcesUri">资源链接</param>
        /// <returns></returns>
        [HttpPost("member-has-resources-uri")]
        [SwaggerResponse((int)HttpStatusCode.OK, "验证结果", typeof(bool))]
        public async Task<object> MemberHasResourcesUri(string memberId, string resourcesUri)
        {
            return JsonContent(await Task.FromResult(AuthoritiesBusiness.MemberHasResourcesUri(memberId, resourcesUri)));
        }

        #endregion    
    }
}
