using AutoMapper;
using Business.Interface.System;
using Business.Utils;
using Entity.System;
using FreeSql;
using Library.DataMapping.Gen;
using Library.FreeSql.Extention;
using Library.FreeSql.Gen;
using Model.System;
using Model.System.AuthorizeDTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Implementation.System
{
    /// <summary>
    /// 权限业务类
    /// </summary>
    public class AuthoritiesBusiness : BaseBusiness, IAuthoritiesBusiness
    {
        #region DI
        public AuthoritiesBusiness(
            IFreeSqlProvider freeSqlProvider,
            IAutoMapperProvider autoMapperProvider,
            IOperationRecordBusiness operationRecordBusiness,
            IUserBusiness userBusiness,
            IRoleBusiness roleBusiness)
        {
            Orm = freeSqlProvider.GetFreeSql();
            Repository_User = Orm.GetRepository<System_User, string>();
            Repository_UserRole = Orm.GetRepository<System_UserRole, string>();
            Repository_UserMenu = Orm.GetRepository<System_UserMenu, string>();
            Repository_UserResources = Orm.GetRepository<System_UserResources, string>();
            Repository_Role = Orm.GetRepository<System_Role, string>();
            Repository_RoleMenu = Orm.GetRepository<System_RoleMenu, string>();
            Repository_RoleResources = Orm.GetRepository<System_RoleResources, string>();
            Repository_Menu = Orm.GetRepository<System_Menu, string>();
            Repository_Resources = Orm.GetRepository<System_Resources, string>();
            Mapper = autoMapperProvider.GetMapper();
            OperationRecordBusiness = operationRecordBusiness;
            RoleBusiness = roleBusiness;
        }

        #endregion

        #region 私有成员

        IFreeSql Orm { get; set; }

        IBaseRepository<System_User, string> Repository_User { get; set; }

        IBaseRepository<System_UserRole, string> Repository_UserRole { get; set; }

        IBaseRepository<System_UserMenu, string> Repository_UserMenu { get; set; }

        IBaseRepository<System_UserResources, string> Repository_UserResources { get; set; }

        IBaseRepository<System_Role, string> Repository_Role { get; set; }

        IBaseRepository<System_RoleMenu, string> Repository_RoleMenu { get; set; }

        IBaseRepository<System_RoleResources, string> Repository_RoleResources { get; set; }

        IBaseRepository<System_Menu, string> Repository_Menu { get; set; }

        IBaseRepository<System_Resources, string> Repository_Resources { get; set; }

        IMapper Mapper { get; set; }

        IOperationRecordBusiness OperationRecordBusiness { get; set; }

        IRoleBusiness RoleBusiness { get; set; }

        private dynamic GetUserWithCheck(string userId)
        {
            var user = Repository_User.Where(o => o.Id == userId).ToOne(o => new
            {
                o.Id,
                o.Enable,
                o.Account,
                o.Type,
                o.Name
            });

            if (user == null)
                throw new ApplicationException("用户不存在或已被移除.");

            if (!user.Enable)
                throw new ApplicationException("用户账号已禁用.");

            return user;
        }

        private dynamic GetRoleWithCheck(string roleId)
        {
            var role = Repository_Role.Where(o => o.Id == roleId).ToOne(o => new
            {
                o.Id,
                o.Enable,
                o.Type,
                o.Name
            });

            if (role == null)
                throw new ApplicationException("角色不存在或已被移除.");

            if (!role.Enable)
                throw new ApplicationException("角色账号已禁用.");

            return role;
        }

        #endregion

        #region 公共

        #region 授权

        public void AuthorizeRoleForUser(RoleForUser data)
        {
            var users = data.UserIds.Select(o => GetUserWithCheck(o));

            var roles = Repository_Role.Where(o => data.RoleIds.Contains(o.Id) && o.Enable).ToList(o => new
            {
                o.Id,
                o.Type,
                o.Name
            });

            if (!roles.Any())
                throw new ApplicationException("没有可供授权的角色");

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var orId = OperationRecordBusiness.Create(new System_OperationRecord
                {
                    DataType = nameof(System_UserRole),
                    DataId = null,
                    Explain = $"授权角色给用户.",
                    Remark = $"被授权的用户: \r\n\t{string.Join(",", users.Select(o => $"[账号 {o.Account}, 类型 {o.Type}, 姓名 {o.Name}]"))}\r\n" +
                            $"授权的角色: \r\n\t{string.Join(",", roles.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                Repository_UserRole.Insert(roles.SelectMany(o => users.Select(p => new System_UserRole
                {
                    UserId = p.Id,
                    RoleId = o.Id
                })));
            });

            if (!success)
                throw new ApplicationException("授权失败", ex);
        }

        public void AuthorizeMenuForUser(MenuForUser data)
        {
            var users = data.UserIds.Select(o => GetUserWithCheck(o));

            var menus = Repository_Menu.Where(o => data.MenuIds.Contains(o.Id) && o.Enable).ToList(o => new
            {
                o.Id,
                o.Type,
                o.Name
            });

            if (!menus.Any())
                throw new ApplicationException("没有可供授权的菜单");

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var orId = OperationRecordBusiness.Create(new System_OperationRecord
                {
                    DataType = nameof(System_UserMenu),
                    DataId = null,
                    Explain = $"授权菜单给用户.",
                    Remark = $"被授权的用户: \r\n\t{string.Join(",", users.Select(o => $"[账号 {o.Account}, 类型 {o.Type}, 姓名 {o.Name}]"))}\r\n" +
                            $"授权的菜单: \r\n\t{string.Join(",", menus.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                Repository_UserMenu.Insert(menus.SelectMany(o => users.Select(p => new System_UserMenu
                {
                    UserId = p.Id,
                    MenuId = o.Id
                })));
            });

            if (!success)
                throw new ApplicationException("授权失败", ex);
        }

        public void AuthorizeResourcesForUser(ResourcesForUser data)
        {
            var users = data.UserIds.Select(o => GetUserWithCheck(o));

            var resources = Repository_Resources.Where(o => data.ResourcesIds.Contains(o.Id) && o.Enable).ToList(o => new
            {
                o.Id,
                o.Type,
                o.Name
            });

            if (!resources.Any())
                throw new ApplicationException("没有可供授权的资源");

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var orId = OperationRecordBusiness.Create(new System_OperationRecord
                {
                    DataType = nameof(System_UserResources),
                    DataId = null,
                    Explain = $"授权资源给用户.",
                    Remark = $"被授权的用户: \r\n\t{string.Join(",", users.Select(o => $"[账号 {o.Account}, 类型 {o.Type}, 姓名 {o.Name}]"))}\r\n" +
                            $"授权的资源: \r\n\t{string.Join(",", resources.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                Repository_UserResources.Insert(resources.SelectMany(o => users.Select(p => new System_UserResources
                {
                    UserId = p.Id,
                    ResourcesId = o.Id
                })));
            });

            if (!success)
                throw new ApplicationException("授权失败", ex);
        }

        public void AuthorizeMenuForRole(MenuForRole data)
        {
            var roles = data.RoleIds.Select(o => GetRoleWithCheck(o));

            var menus = Repository_Menu.Where(o => data.MenuIds.Contains(o.Id) && o.Enable).ToList(o => new
            {
                o.Id,
                o.Type,
                o.Name
            });

            if (!menus.Any())
                throw new ApplicationException("没有可供授权的菜单");

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var orId = OperationRecordBusiness.Create(new System_OperationRecord
                {
                    DataType = nameof(System_RoleMenu),
                    DataId = null,
                    Explain = $"授权菜单给角色.",
                    Remark = $"被授权的角色: \r\n\t{string.Join(",", roles.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}\r\n" +
                            $"授权的菜单: \r\n\t{string.Join(",", menus.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                Repository_RoleMenu.Insert(menus.SelectMany(o => roles.Select(p => new System_RoleMenu
                {
                    RoleId = p.Id,
                    MenuId = o.Id
                })));
            });

            if (!success)
                throw new ApplicationException("授权失败", ex);
        }

        public void AuthorizeResourcesForRole(ResourcesForRole data)
        {
            var roles = data.RoleIds.Select(o => GetRoleWithCheck(o));

            var resources = Repository_Resources.Where(o => data.ResourcesIds.Contains(o.Id) && o.Enable).ToList(o => new
            {
                o.Id,
                o.Type,
                o.Name
            });

            if (!resources.Any())
                throw new ApplicationException("没有可供授权的资源");

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var orId = OperationRecordBusiness.Create(new System_OperationRecord
                {
                    DataType = nameof(System_RoleResources),
                    DataId = null,
                    Explain = $"授权资源给角色.",
                    Remark = $"被授权的角色: \r\n\t{string.Join(",", roles.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}\r\n" +
                            $"授权的资源: \r\n\t{string.Join(",", resources.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                Repository_RoleResources.Insert(resources.SelectMany(o => roles.Select(p => new System_RoleResources
                {
                    RoleId = p.Id,
                    ResourcesId = o.Id
                })));
            });

            if (!success)
                throw new ApplicationException("授权失败", ex);
        }

        #endregion

        #region 撤销授权

        public void RevocationRoleForUser(List<string> userIds, bool runTransaction = true)
        {
            RevocationRoleForUser(new RoleForUser
            {
                UserIds = userIds,
                All = true
            }, runTransaction);
        }

        public void RevocationMenuForUser(List<string> userIds, bool runTransaction = true)
        {
            RevocationMenuForUser(new MenuForUser
            {
                UserIds = userIds,
                All = true
            }, runTransaction);
        }

        public void RevocationResourcesForUser(List<string> userIds, bool runTransaction = true)
        {
            RevocationResourcesForUser(new ResourcesForUser
            {
                UserIds = userIds,
                All = true
            }, runTransaction);
        }

        public void RevocationMenuForRole(List<string> roleIds, bool runTransaction = true)
        {
            RevocationMenuForRole(new MenuForRole
            {
                RoleIds = roleIds,
                All = true
            }, runTransaction);
        }

        public void RevocationResourcesForRole(List<string> roleIds, bool runTransaction = true)
        {
            RevocationResourcesForRole(new ResourcesForRole
            {
                RoleIds = roleIds,
                All = true
            }, runTransaction);
        }

        public void RevocationRoleForUser(RoleForUser data, bool runTransaction = true)
        {
            var users = data.UserIds.Select(o => GetUserWithCheck(o));

            var roles = Repository_UserRole.Where(o => data.UserIds.Contains(o.UserId) && (data.All || data.RoleIds.Contains(o.RoleId))).ToList(o => new
            {
                o.RoleId,
                o.Role.Type,
                o.Role.Name
            });

            if (!roles.Any())
                return;

            Action handler = () =>
            {
                var orId = OperationRecordBusiness.Create(new System_OperationRecord
                {
                    DataType = nameof(System_UserRole),
                    DataId = null,
                    Explain = $"撤销用户的角色授权.",
                    Remark = $"被撤销授权的用户: \r\n\t{string.Join(",", users.Select(o => $"[账号 {o.Account}, 类型 {o.Type}, 姓名 {o.Name}]"))}\r\n" +
                            $"撤销授权的角色: \r\n\t{string.Join(",", roles.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                var roleIds = roles.Select(o => o.RoleId);

                if (Repository_UserRole.Delete(o => data.UserIds.Contains(o.UserId) && (roleIds.Contains(o.RoleId))) < 0)
                    throw new ApplicationException("撤销授权失败");
            };

            if (runTransaction)
            {
                (bool success, Exception ex) = Orm.RunTransaction(handler);

                if (!success)
                    throw new ApplicationException("撤销授权失败", ex);
            }
            else
                handler.Invoke();
        }

        public void RevocationMenuForUser(MenuForUser data, bool runTransaction = true)
        {
            var users = data.UserIds.Select(o => GetUserWithCheck(o));

            var menus = Repository_UserMenu.Where(o => data.UserIds.Contains(o.UserId) && (data.All || data.MenuIds.Contains(o.MenuId))).ToList(o => new
            {
                o.MenuId,
                o.Menu.Type,
                o.Menu.Name
            });

            if (!menus.Any())
                return;

            Action handler = () =>
            {
                var orId = OperationRecordBusiness.Create(new System_OperationRecord
                {
                    DataType = nameof(System_UserMenu),
                    DataId = null,
                    Explain = $"撤销用户的菜单授权.",
                    Remark = $"被撤销授权的用户: \r\n\t{string.Join(",", users.Select(o => $"[账号 {o.Account}, 类型 {o.Type}, 姓名 {o.Name}]"))}\r\n" +
                            $"撤销授权的菜单: \r\n\t{string.Join(",", menus.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                var menuIds = menus.Select(o => o.MenuId);

                if (Repository_UserMenu.Delete(o => data.UserIds.Contains(o.UserId) && (menuIds.Contains(o.MenuId))) < 0)
                    throw new ApplicationException("撤销授权失败");
            };

            if (runTransaction)
            {
                (bool success, Exception ex) = Orm.RunTransaction(handler);

                if (!success)
                    throw new ApplicationException("撤销授权失败", ex);
            }
            else
                handler.Invoke();
        }

        public void RevocationResourcesForUser(ResourcesForUser data, bool runTransaction = true)
        {
            var users = data.UserIds.Select(o => GetUserWithCheck(o));

            var resourcess = Repository_UserResources.Where(o => data.UserIds.Contains(o.UserId) && (data.All || data.ResourcesIds.Contains(o.ResourcesId))).ToList(o => new
            {
                o.ResourcesId,
                o.Resources.Type,
                o.Resources.Name
            });

            if (!resourcess.Any())
                return;

            Action handler = () =>
            {
                var orId = OperationRecordBusiness.Create(new System_OperationRecord
                {
                    DataType = nameof(System_UserResources),
                    DataId = null,
                    Explain = $"撤销用户的资源授权.",
                    Remark = $"被撤销授权的用户: \r\n\t{string.Join(",", users.Select(o => $"[账号 {o.Account}, 类型 {o.Type}, 姓名 {o.Name}]"))}\r\n" +
                            $"撤销授权的资源: \r\n\t{string.Join(",", resourcess.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                var resourcesIds = resourcess.Select(o => o.ResourcesId);

                if (Repository_UserResources.Delete(o => data.UserIds.Contains(o.UserId) && (resourcesIds.Contains(o.ResourcesId))) < 0)
                    throw new ApplicationException("撤销授权失败");
            };

            if (runTransaction)
            {
                (bool success, Exception ex) = Orm.RunTransaction(handler);

                if (!success)
                    throw new ApplicationException("撤销授权失败", ex);
            }
            else
                handler.Invoke();
        }

        public void RevocationMenuForRole(MenuForRole data, bool runTransaction = true)
        {
            var roles = data.RoleIds.Select(o => GetRoleWithCheck(o));

            var menus = Repository_RoleMenu.Where(o => data.RoleIds.Contains(o.RoleId) && (data.All || data.MenuIds.Contains(o.MenuId))).ToList(o => new
            {
                o.MenuId,
                o.Menu.Type,
                o.Menu.Name
            });

            if (!menus.Any())
                return;

            Action handler = () =>
            {
                var orId = OperationRecordBusiness.Create(new System_OperationRecord
                {
                    DataType = nameof(System_RoleMenu),
                    DataId = null,
                    Explain = $"撤销角色的菜单授权.",
                    Remark = $"被撤销授权的角色: \r\n\t{string.Join(",", roles.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}\r\n" +
                            $"撤销授权的菜单: \r\n\t{string.Join(",", menus.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                var menuIds = menus.Select(o => o.MenuId);

                if (Repository_RoleMenu.Delete(o => data.RoleIds.Contains(o.RoleId) && (menuIds.Contains(o.MenuId))) < 0)
                    throw new ApplicationException("撤销授权失败");
            };

            if (runTransaction)
            {
                (bool success, Exception ex) = Orm.RunTransaction(handler);

                if (!success)
                    throw new ApplicationException("撤销授权失败", ex);
            }
            else
                handler.Invoke();
        }

        public void RevocationResourcesForRole(ResourcesForRole data, bool runTransaction = true)
        {
            var roles = data.RoleIds.Select(o => GetRoleWithCheck(o));

            var resourcess = Repository_RoleResources.Where(o => data.RoleIds.Contains(o.RoleId) && (data.All || data.ResourcesIds.Contains(o.ResourcesId))).ToList(o => new
            {
                o.ResourcesId,
                o.Resources.Type,
                o.Resources.Name
            });

            if (!resourcess.Any())
                return;

            Action handler = () =>
            {
                var orId = OperationRecordBusiness.Create(new System_OperationRecord
                {
                    DataType = nameof(System_RoleResources),
                    DataId = null,
                    Explain = $"撤销角色的资源授权.",
                    Remark = $"被撤销授权的角色: \r\n\t{string.Join(",", roles.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}\r\n" +
                            $"撤销授权的资源: \r\n\t{string.Join(",", resourcess.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                var resourcesIds = resourcess.Select(o => o.ResourcesId);

                if (Repository_RoleResources.Delete(o => data.RoleIds.Contains(o.RoleId) && (resourcesIds.Contains(o.ResourcesId))) < 0)
                    throw new ApplicationException("撤销授权失败");
            };

            if (runTransaction)
            {
                (bool success, Exception ex) = Orm.RunTransaction(handler);

                if (!success)
                    throw new ApplicationException("撤销授权失败", ex);
            }
            else
                handler.Invoke();
        }

        public void RevocationMenuForAll(List<string> menuIds, bool runTransaction = true)
        {
            var menus = Repository_Menu.Where(o => menuIds.Contains(o.Id)).ToList(o => new
            {
                o.Id,
                o.Type,
                o.Name
            });

            if (!menus.Any())
                return;

            Action handler = () =>
            {
                var orId = OperationRecordBusiness.Create(new System_OperationRecord
                {
                    DataType = nameof(System_UserMenu) + "+" + nameof(System_RoleMenu),
                    Explain = $"撤销所有用户和角色的菜单授权.",
                    Remark = $"撤销授权的菜单: \r\n\t{string.Join(",", menus.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                if (Repository_UserMenu.Delete(o => (menuIds.Contains(o.MenuId))) < 0)
                    throw new ApplicationException("撤销用户的菜单授权失败");

                if (Repository_RoleMenu.Delete(o => (menuIds.Contains(o.MenuId))) < 0)
                    throw new ApplicationException("撤销角色的菜单授权失败");
            };

            if (runTransaction)
            {
                (bool success, Exception ex) = Orm.RunTransaction(handler);

                if (!success)
                    throw new ApplicationException("撤销授权失败", ex);
            }
            else
                handler.Invoke();
        }

        public void RevocationResourcesForAll(List<string> resourcesIds, bool runTransaction = true)
        {
            var resources = Repository_Resources.Where(o => resourcesIds.Contains(o.Id)).ToList(o => new
            {
                o.Id,
                o.Type,
                o.Name
            });

            if (!resources.Any())
                return;

            Action handler = () =>
            {
                var orId = OperationRecordBusiness.Create(new System_OperationRecord
                {
                    DataType = nameof(System_UserResources) + "+" + nameof(System_RoleResources),
                    Explain = $"撤销所有用户和角色的资源授权.",
                    Remark = $"撤销授权的资源: \r\n\t{string.Join(",", resources.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                if (Repository_UserResources.Delete(o => (resourcesIds.Contains(o.ResourcesId))) < 0)
                    throw new ApplicationException("撤销用户的资源授权失败");

                if (Repository_RoleResources.Delete(o => (resourcesIds.Contains(o.ResourcesId))) < 0)
                    throw new ApplicationException("撤销角色的资源授权失败");
            };

            if (runTransaction)
            {
                (bool success, Exception ex) = Orm.RunTransaction(handler);

                if (!success)
                    throw new ApplicationException("撤销授权失败", ex);
            }
            else
                handler.Invoke();
        }

        #endregion

        #region 获取授权

        public Model.System.UserDTO.Authorities GetUser(string userId, bool includeRole, bool includeMenu, bool includeResources, bool mergeRoleMenu = true, bool mergeRoleResources = true)
        {
            var user = Repository_User.Where(o => o.Id == userId)
                                    .ToOne(o => new Model.System.UserDTO.Authorities
                                    {
                                        Id = o.Id,
                                        Type = o.Type,
                                        Account = o.Account,
                                        Enable = o.Enable
                                    });

            if (user == null)
                throw new ApplicationException("用户不存在或已被移除.");

            if (!user.Enable)
                throw new ApplicationException("用户账号已禁用.");

            if (includeRole)
                user._Roles = GetUserRole(userId, includeMenu && !mergeRoleMenu, includeResources && !mergeRoleResources);

            if (includeMenu)
                user._Menus = GetUserMenu(userId, mergeRoleMenu);

            if (includeResources)
                user._Resources = GetUserResources(userId, mergeRoleResources);

            return user;
        }

        public List<Model.System.RoleDTO.Authorities> GetUserRole(string userId, bool includeMenu, bool includeResources)
        {
            var roles = Repository_UserRole.Where(o => o.UserId == userId && o.Role.Enable)
                                         .ToList(o => new Model.System.RoleDTO.Authorities
                                         {
                                             Id = o.RoleId,
                                             Name = o.Role.Name,
                                             Type = o.Role.Type,
                                             Code = o.Role.Code
                                         });

            if (includeMenu || includeResources)
                roles.ForEach(o =>
                {
                    if (includeMenu)
                        o._Menus = GetRoleMenu(o.Id);

                    if (includeResources)
                        o._Resources = GetRoleResources(o.Id);
                });

            return roles;
        }

        public List<Model.System.MenuDTO.Authorities> GetUserMenu(string userId, bool mergeRoleMenu)
        {
            var menus = Repository_UserMenu.Where(o => o.UserId == userId && o.Menu.Enable)
                                         .ToList(o => new Model.System.MenuDTO.Authorities
                                         {
                                             Id = o.MenuId,
                                             Name = o.Menu.Name,
                                             Type = o.Menu.Type,
                                             Code = o.Menu.Code,
                                             Uri = o.Menu.Uri,
                                             Method = o.Menu.Method
                                         });

            if (mergeRoleMenu)
                Repository_User.Where(o => o.Id == userId)
                            .ToOne(o => o.Roles.AsSelect().Where(p => p.Enable).ToList(p => p.Id))
                            .ForEach(o => menus.AddRange(GetRoleMenu(o)));

            return menus;
        }

        public List<Model.System.ResourcesDTO.Authorities> GetUserResources(string userId, bool mergeRoleResources)
        {
            var resources = Repository_UserResources.Where(o => o.UserId == userId && o.Resources.Enable)
                                         .ToList(o => new Model.System.ResourcesDTO.Authorities
                                         {
                                             Id = o.ResourcesId,
                                             Name = o.Resources.Name,
                                             Type = o.Resources.Type,
                                             Code = o.Resources.Code,
                                             Uri = o.Resources.Uri
                                         });

            if (mergeRoleResources)
                Repository_User.Where(o => o.Id == userId)
                            .ToOne(o => o.Roles.AsSelect().Where(p => p.Enable).ToList(p => p.Id))
                            .ForEach(o => resources.AddRange(GetRoleResources(o)));

            return resources;
        }

        public Model.System.RoleDTO.Authorities GetRole(string roleId, bool includeMenu, bool includeResources)
        {
            var role = Repository_Role.Where(o => o.Id == roleId)
                                    .ToOne(o => new Model.System.RoleDTO.Authorities
                                    {
                                        Id = o.Id,
                                        Name = o.Name,
                                        Type = o.Type,
                                        Code = o.Code
                                    });

            if (role == null)
                throw new ApplicationException("角色不存在或已被移除.");

            if (!role.Enable)
                throw new ApplicationException("角色已禁用.");

            if (includeMenu)
                role._Menus = GetRoleMenu(role.Id);

            if (includeResources)
                role._Resources = GetRoleResources(role.Id);

            return role;
        }

        public List<Model.System.MenuDTO.Authorities> GetRoleMenu(string roleId)
        {
            return Repository_RoleMenu.Where(o => o.RoleId == roleId && o.Menu.Enable)
                                         .ToList(o => new Model.System.MenuDTO.Authorities
                                         {
                                             Id = o.MenuId,
                                             Name = o.Menu.Name,
                                             Type = o.Menu.Type,
                                             Code = o.Menu.Code,
                                             Uri = o.Menu.Uri,
                                             Method = o.Menu.Method
                                         });
        }

        public List<Model.System.ResourcesDTO.Authorities> GetRoleResources(string roleId)
        {
            return Repository_RoleResources.Where(o => o.RoleId == roleId && o.Resources.Enable)
                                         .ToList(o => new Model.System.ResourcesDTO.Authorities
                                         {
                                             Id = o.ResourcesId,
                                             Name = o.Resources.Name,
                                             Type = o.Resources.Type,
                                             Code = o.Resources.Code,
                                             Uri = o.Resources.Uri
                                         });
        }

        #endregion

        #region 验证授权

        public bool IsAdminUser(string userId)
        {
            return Repository_UserRole.Where(o => o.UserId == userId && (o.Role.Type == RoleType.超级管理员 || o.Role.Type == RoleType.管理员)).Any();
        }

        public bool IsAdminRole(string roleId)
        {
            return Repository_Role.Where(o => o.Id == roleId && (o.Type == RoleType.超级管理员 || o.Type == RoleType.管理员)).Any();
        }

        public bool HasRole(string userId, string roleId)
        {
            return Repository_UserRole.Where(o => o.UserId == userId && o.RoleId == roleId).Any();
        }

        public bool HasMenu(string userId, string menuId)
        {
            var result = Repository_UserMenu.Where(o => o.UserId == userId && o.MenuId == menuId).Any();

            if (!result)
                Repository_RoleMenu.Where(o => o.MenuId == menuId && Repository_UserRole.Where(p => p.UserId == userId && p.RoleId == o.RoleId && o.Role.Enable).Any());

            return result;
        }

        public bool HasResources(string userId, string resourcesId)
        {
            var result = Repository_UserResources.Where(o => o.UserId == userId && o.ResourcesId == resourcesId).Any();

            if (!result)
                Repository_RoleResources.Where(o => o.ResourcesId == resourcesId && Repository_UserRole.Where(p => p.UserId == userId && p.RoleId == o.RoleId && o.Role.Enable).Any());

            return result;
        }

        #endregion

        #endregion
    }
}
