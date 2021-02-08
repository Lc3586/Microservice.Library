using System.Collections.Generic;

namespace Business.Interface.System
{
    /// <summary>
    /// 权限业务接口类
    /// </summary>
    public interface IAuthoritiesBusiness
    {
        #region 授权

        /// <summary>
        /// 授权角色给用户
        /// </summary>
        /// <param name="data">数据</param>
        void AuthorizeRoleForUser(Model.System.AuthorizeDTO.RoleForUser data);

        /// <summary>
        /// 授权角色给会员
        /// </summary>
        /// <param name="data">数据</param>
        void AuthorizeRoleForMember(Model.System.AuthorizeDTO.RoleForMember data);

        /// <summary>
        /// 授权菜单给用户
        /// </summary>
        /// <param name="data">数据</param>
        void AuthorizeMenuForUser(Model.System.AuthorizeDTO.MenuForUser data);

        /// <summary>
        /// 授权资源给用户
        /// </summary>
        /// <param name="data">数据</param>
        void AuthorizeResourcesForUser(Model.System.AuthorizeDTO.ResourcesForUser data);

        /// <summary>
        /// 授权菜单给角色
        /// </summary>
        /// <param name="data">数据</param>
        void AuthorizeMenuForRole(Model.System.AuthorizeDTO.MenuForRole data);

        /// <summary>
        /// 授权资源给角色
        /// </summary>
        /// <param name="data">数据</param>
        void AuthorizeResourcesForRole(Model.System.AuthorizeDTO.ResourcesForRole data);

        #endregion

        #region 撤销授权

        /// <summary>
        /// 撤销用户的全部角色授权
        /// </summary>
        /// <param name="userIds">用户Id</param>
        /// <param name="runTransaction">运行事务（默认运行）</param>
        void RevocationRoleForUser(List<string> userIds, bool runTransaction = true);

        /// <summary>
        /// 撤销会员的全部角色授权
        /// </summary>
        /// <param name="memberIds">会员Id</param>
        /// <param name="runTransaction">运行事务（默认运行）</param>
        void RevocationRoleForMember(List<string> memberIds, bool runTransaction = true);

        /// <summary>
        /// 撤销用户的全部菜单授权
        /// </summary>
        /// <param name="userIds">用户Id</param>
        /// <param name="runTransaction">运行事务（默认运行）</param>
        void RevocationMenuForUser(List<string> userIds, bool runTransaction = true);

        /// <summary>
        /// 撤销用户的全部资源授权
        /// </summary>
        /// <param name="userIds">用户Id</param>
        /// <param name="runTransaction">运行事务（默认运行）</param>
        void RevocationResourcesForUser(List<string> userIds, bool runTransaction = true);

        /// <summary>
        /// 撤销角色的全部菜单授权
        /// </summary>
        /// <param name="roleIds">角色Id</param>
        /// <param name="runTransaction">运行事务（默认运行）</param>
        void RevocationMenuForRole(List<string> roleIds, bool runTransaction = true);

        /// <summary>
        /// 撤销角色的全部资源授权
        /// </summary>
        /// <param name="roleIds">角色Id</param>
        /// <param name="runTransaction">运行事务（默认运行）</param>
        void RevocationResourcesForRole(List<string> roleIds, bool runTransaction = true);

        /// <summary>
        /// 撤销用户的角色授权
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="runTransaction">运行事务（默认运行）</param>
        void RevocationRoleForUser(Model.System.AuthorizeDTO.RoleForUser data, bool runTransaction = true);

        /// <summary>
        /// 撤销会员的角色授权
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="runTransaction">运行事务（默认运行）</param>
        void RevocationRoleForMember(Model.System.AuthorizeDTO.RoleForMember data, bool runTransaction = true);

        /// <summary>
        /// 撤销用户的菜单授权
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="runTransaction">运行事务（默认运行）</param>
        void RevocationMenuForUser(Model.System.AuthorizeDTO.MenuForUser data, bool runTransaction = true);

        /// <summary>
        /// 撤销用户的资源授权
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="runTransaction">运行事务（默认运行）</param>
        void RevocationResourcesForUser(Model.System.AuthorizeDTO.ResourcesForUser data, bool runTransaction = true);

        /// <summary>
        /// 撤销角色的菜单授权
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="runTransaction">运行事务（默认运行）</param>
        void RevocationMenuForRole(Model.System.AuthorizeDTO.MenuForRole data, bool runTransaction = true);

        /// <summary>
        /// 撤销角色的资源授权
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="runTransaction">运行事务（默认运行）</param>
        void RevocationResourcesForRole(Model.System.AuthorizeDTO.ResourcesForRole data, bool runTransaction = true);

        /// <summary>
        /// 撤销所有用户和角色的菜单授权
        /// </summary>
        /// <param name="menuIds">菜单Id</param>
        /// <param name="runTransaction">运行事务（默认运行）</param>
        void RevocationMenuForAll(List<string> menuIds, bool runTransaction = true);

        /// <summary>
        /// 撤销所有用户和角色的资源授权
        /// </summary>
        /// <param name="resourcesIds">资源Id</param>
        /// <param name="runTransaction">运行事务（默认运行）</param>
        void RevocationResourcesForAll(List<string> resourcesIds, bool runTransaction = true);

        #endregion

        #region 获取授权

        /// <summary>
        /// 获取用户的授权数据
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="includeRole">包括授权角色</param>
        /// <param name="includeMenu">包括授权菜单</param>
        /// <param name="includeResources">包括授权资源</param>
        /// <param name="mergeRoleMenu">合并角色的授权菜单</param>
        /// <param name="mergeRoleResources">合并角色的授权资源</param>
        /// <returns>
        /// <para>用户授权信息</para>
        /// <para>角色授权信息</para>
        /// <para>菜单授权信息</para>
        /// <para>资源授权信息</para>
        /// </returns>
        Model.System.UserDTO.Authorities GetUser(string userId, bool includeRole, bool includeMenu, bool includeResources, bool mergeRoleMenu = true, bool mergeRoleResources = true);

        /// <summary>
        /// 获取会员的授权数据
        /// </summary>
        /// <param name="memberId">会员Id</param>
        /// <param name="includeRole">包括授权角色</param>
        /// <param name="includeMenu">包括授权菜单</param>
        /// <param name="includeResources">包括授权资源</param>
        /// <returns>
        /// <para>用户授权信息</para>
        /// <para>角色授权信息</para>
        /// <para>菜单授权信息</para>
        /// <para>资源授权信息</para>
        /// </returns>
        Model.Public.MemberDTO.Authorities GetMember(string memberId, bool includeRole, bool includeMenu, bool includeResources);

        /// <summary>
        /// 获取授权给用户的角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="includeMenu">包括授权菜单</param>
        /// <param name="includeResources">包括授权资源</param>
        /// <returns>
        /// <para>角色授权信息</para>
        /// <para>菜单授权信息</para>
        /// <para>资源授权信息</para>
        /// </returns>
        List<Model.System.RoleDTO.Authorities> GetUserRole(string userId, bool includeMenu, bool includeResources);

        /// <summary>
        /// 获取授权给会员的角色
        /// </summary>
        /// <param name="memberId">会员Id</param>
        /// <param name="includeMenu">包括授权菜单</param>
        /// <param name="includeResources">包括授权资源</param>
        /// <returns>
        /// <para>角色授权信息</para>
        /// <para>菜单授权信息</para>
        /// <para>资源授权信息</para>
        /// </returns>
        List<Model.System.RoleDTO.Authorities> GetMemberRole(string memberId, bool includeMenu, bool includeResources);

        /// <summary>
        /// 获取授权给用户的菜单
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="mergeRoleMenu">包括角色的授权菜单</param>
        /// <returns>
        /// <para>菜单授权信息</para>
        /// </returns>
        List<Model.System.MenuDTO.Authorities> GetUserMenu(string userId, bool mergeRoleMenu);

        /// <summary>
        /// 获取授权给会员的菜单
        /// </summary>
        /// <param name="memberId">会员Id</param>
        /// <returns>
        /// <para>菜单授权信息</para>
        /// </returns>
        List<Model.System.MenuDTO.Authorities> GetMemberMenu(string memberId);

        /// <summary>
        /// 获权授权给用户的资源
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="mergeRoleResources">包括角色的授权资源</param>
        /// <returns>
        /// <para>资源授权信息</para>
        /// </returns>
        List<Model.System.ResourcesDTO.Authorities> GetUserResources(string userId, bool mergeRoleResources);

        /// <summary>
        /// 获权授权给会员的资源
        /// </summary>
        /// <param name="memberId">会员Id</param>
        /// <returns>
        /// <para>资源授权信息</para>
        /// </returns>
        List<Model.System.ResourcesDTO.Authorities> GetMemberResources(string memberId);

        /// <summary>
        /// 获取角色的授权数据
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="includeMenu">包括授权菜单</param>
        /// <param name="includeResources">包括授权资源</param>
        /// <returns>
        /// <para>角色授权信息</para>
        /// <para>菜单授权信息</para>
        /// <para>资源授权信息</para>
        /// </returns>
        Model.System.RoleDTO.Authorities GetRole(string roleId, bool includeMenu, bool includeResources);

        /// <summary>
        /// 获取授权给角色的菜单
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns>
        /// <para>菜单授权信息</para>
        /// </returns>
        List<Model.System.MenuDTO.Authorities> GetRoleMenu(string roleId);

        /// <summary>
        /// 获取授权给角色的资源
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns>
        /// <para>资源授权信息</para>
        /// </returns>
        List<Model.System.ResourcesDTO.Authorities> GetRoleResources(string roleId);

        #endregion

        #region 验证授权

        /// <summary>
        /// 是否为管理员
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        bool IsAdminUser(string userId);

        /// <summary>
        /// 是否为管理角色
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        bool IsAdminRole(string roleId);

        /// <summary>
        /// 用户是否拥有角色授权
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        bool UserHasRole(string userId, string roleId);

        /// <summary>
        /// 会员是否拥有角色授权
        /// </summary>
        /// <param name="memberId">会员Id</param>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        bool MemberHasRole(string memberId, string roleId);

        /// <summary>
        /// 用户是否拥有菜单授权
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="menuId">菜单Id</param>
        /// <returns></returns>
        bool UserHasMenu(string userId, string menuId);

        /// <summary>
        /// 会员是否拥有菜单授权
        /// </summary>
        /// <param name="memberId">会员Id</param>
        /// <param name="menuId">菜单Id</param>
        /// <returns></returns>
        bool MemberHasMenu(string memberId, string menuId);

        /// <summary>
        /// 用户是否拥有资源授权
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="resourcesId">资源Id</param>
        /// <returns></returns>
        bool UserHasResources(string userId, string resourcesId);

        /// <summary>
        /// 会员是否拥有资源授权
        /// </summary>
        /// <param name="memberId">会员Id</param>
        /// <param name="resourcesId">资源Id</param>
        /// <returns></returns>
        bool MemberHasResources(string memberId, string resourcesId);

        #endregion
    }
}
