using AutoMapper;
using Business.Interface.Common;
using Business.Interface.System;
using Business.Utils;
using Entity.Common;
using Entity.Public;
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
            Repository_Member = Orm.GetRepository<Public_Member, string>();
            Repository_MemberRole = Orm.GetRepository<Public_MemberRole, string>();
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

        IBaseRepository<Public_Member, string> Repository_Member { get; set; }

        IBaseRepository<Public_MemberRole, string> Repository_MemberRole { get; set; }

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
                o.Name
            });

            if (user == null)
                throw new ApplicationException("用户不存在或已被移除.");

            if (!user.Enable)
                throw new ApplicationException("用户账号已禁用.");

            return user;
        }

        private dynamic GetMemberWithCheck(string memberId)
        {
            var user = Repository_Member.Where(o => o.Id == memberId).ToOne(o => new
            {
                o.Id,
                o.Enable,
                o.Account,
                o.Name
            });

            if (user == null)
                throw new ApplicationException("会员不存在或已被移除.");

            if (!user.Enable)
                throw new ApplicationException("会员账号已禁用.");

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
                throw new ApplicationException("没有可供授权的角色.");

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(System_UserRole),
                    DataId = null,
                    Explain = $"授权角色给用户.",
                    Remark = $"被授权的用户: \r\n\t{string.Join(",", users.Select(o => $"[账号 {o.Account}, 姓名 {o.Name}]"))}\r\n" +
                            $"授权的角色: \r\n\t{string.Join(",", roles.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                Repository_UserRole.Insert(roles.SelectMany(o => users.Select(p => new System_UserRole
                {
                    UserId = p.Id,
                    RoleId = o.Id
                })));
            });

            if (!success)
                throw new ApplicationException("授权失败.", ex);
        }

        public void AuthorizeRoleForMember(RoleForMember data)
        {
            var members = data.MemberIds.Select(o => GetMemberWithCheck(o));

            var roles = Repository_Role.Where(o => data.RoleIds.Contains(o.Id) && o.Enable).ToList(o => new
            {
                o.Id,
                o.Type,
                o.Name
            });

            if (!roles.Any())
                throw new ApplicationException("没有可供授权的角色.");

            if (roles.Any(o => o.Type != RoleType.会员))
                throw new ApplicationException($"只能将角色类型为{RoleType.会员}的角色授权给会员.");

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(Public_MemberRole),
                    DataId = null,
                    Explain = $"授权角色给会员.",
                    Remark = $"被授权的会员: \r\n\t{string.Join(",", members.Select(o => $"[账号 {o.Account}, 姓名 {o.Name}]"))}\r\n" +
                            $"授权的角色: \r\n\t{string.Join(",", roles.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                Repository_MemberRole.Insert(roles.SelectMany(o => members.Select(p => new Public_MemberRole
                {
                    MemberId = p.Id,
                    RoleId = o.Id
                })));
            });

            if (!success)
                throw new ApplicationException("授权失败.", ex);
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
                throw new ApplicationException("没有可供授权的菜单.");

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(System_UserMenu),
                    DataId = null,
                    Explain = $"授权菜单给用户.",
                    Remark = $"被授权的用户: \r\n\t{string.Join(",", users.Select(o => $"[账号 {o.Account}, 姓名 {o.Name}]"))}\r\n" +
                            $"授权的菜单: \r\n\t{string.Join(",", menus.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                Repository_UserMenu.Insert(menus.SelectMany(o => users.Select(p => new System_UserMenu
                {
                    UserId = p.Id,
                    MenuId = o.Id
                })));
            });

            if (!success)
                throw new ApplicationException("授权失败.", ex);
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
                throw new ApplicationException("没有可供授权的资源.");

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(System_UserResources),
                    DataId = null,
                    Explain = $"授权资源给用户.",
                    Remark = $"被授权的用户: \r\n\t{string.Join(",", users.Select(o => $"[账号 {o.Account}, 姓名 {o.Name}]"))}\r\n" +
                            $"授权的资源: \r\n\t{string.Join(",", resources.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                Repository_UserResources.Insert(resources.SelectMany(o => users.Select(p => new System_UserResources
                {
                    UserId = p.Id,
                    ResourcesId = o.Id
                })));
            });

            if (!success)
                throw new ApplicationException("授权失败.", ex);
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
                throw new ApplicationException("没有可供授权的菜单.");

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
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
                throw new ApplicationException("授权失败.", ex);
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
                throw new ApplicationException("没有可供授权的资源.");

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
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
                throw new ApplicationException("授权失败.", ex);
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

        public void RevocationRoleForMember(List<string> memberIds, bool runTransaction = true)
        {
            RevocationRoleForMember(new RoleForMember
            {
                MemberIds = memberIds,
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
                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(System_UserRole),
                    DataId = null,
                    Explain = $"撤销用户的角色授权.",
                    Remark = $"被撤销授权的用户: \r\n\t{string.Join(",", users.Select(o => $"[账号 {o.Account}, 姓名 {o.Name}]"))}\r\n" +
                            $"撤销授权的角色: \r\n\t{string.Join(",", roles.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                var roleIds = roles.Select(o => o.RoleId);

                if (Repository_UserRole.Delete(o => data.UserIds.Contains(o.UserId) && (roleIds.Contains(o.RoleId))) < 0)
                    throw new ApplicationException("撤销授权失败.");
            };

            if (runTransaction)
            {
                (bool success, Exception ex) = Orm.RunTransaction(handler);

                if (!success)
                    throw new ApplicationException("撤销授权失败.", ex);
            }
            else
                handler.Invoke();
        }

        public void RevocationRoleForMember(RoleForMember data, bool runTransaction = true)
        {
            var members = data.MemberIds.Select(o => GetMemberWithCheck(o));

            var roles = Repository_MemberRole.Where(o => data.MemberIds.Contains(o.MemberId) && (data.All || data.RoleIds.Contains(o.RoleId))).ToList(o => new
            {
                o.RoleId,
                o.Role.Type,
                o.Role.Name
            });

            if (!roles.Any())
                return;

            Action handler = () =>
            {
                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(Public_MemberRole),
                    DataId = null,
                    Explain = $"撤销会员的角色授权.",
                    Remark = $"被撤销授权的会员: \r\n\t{string.Join(",", members.Select(o => $"[账号 {o.Account}, 姓名 {o.Name}]"))}\r\n" +
                            $"撤销授权的角色: \r\n\t{string.Join(",", roles.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                var roleIds = roles.Select(o => o.RoleId);

                if (Repository_MemberRole.Delete(o => data.MemberIds.Contains(o.MemberId) && (roleIds.Contains(o.RoleId))) < 0)
                    throw new ApplicationException("撤销授权失败.");
            };

            if (runTransaction)
            {
                (bool success, Exception ex) = Orm.RunTransaction(handler);

                if (!success)
                    throw new ApplicationException("撤销授权失败.", ex);
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
                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(System_UserMenu),
                    DataId = null,
                    Explain = $"撤销用户的菜单授权.",
                    Remark = $"被撤销授权的用户: \r\n\t{string.Join(",", users.Select(o => $"[账号 {o.Account}, 姓名 {o.Name}]"))}\r\n" +
                            $"撤销授权的菜单: \r\n\t{string.Join(",", menus.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                var menuIds = menus.Select(o => o.MenuId);

                if (Repository_UserMenu.Delete(o => data.UserIds.Contains(o.UserId) && (menuIds.Contains(o.MenuId))) < 0)
                    throw new ApplicationException("撤销授权失败.");
            };

            if (runTransaction)
            {
                (bool success, Exception ex) = Orm.RunTransaction(handler);

                if (!success)
                    throw new ApplicationException("撤销授权失败.", ex);
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
                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(System_UserResources),
                    DataId = null,
                    Explain = $"撤销用户的资源授权.",
                    Remark = $"被撤销授权的用户: \r\n\t{string.Join(",", users.Select(o => $"[账号 {o.Account}, 姓名 {o.Name}]"))}\r\n" +
                            $"撤销授权的资源: \r\n\t{string.Join(",", resourcess.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                var resourcesIds = resourcess.Select(o => o.ResourcesId);

                if (Repository_UserResources.Delete(o => data.UserIds.Contains(o.UserId) && (resourcesIds.Contains(o.ResourcesId))) < 0)
                    throw new ApplicationException("撤销授权失败.");
            };

            if (runTransaction)
            {
                (bool success, Exception ex) = Orm.RunTransaction(handler);

                if (!success)
                    throw new ApplicationException("撤销授权失败.", ex);
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
                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(System_RoleMenu),
                    DataId = null,
                    Explain = $"撤销角色的菜单授权.",
                    Remark = $"被撤销授权的角色: \r\n\t{string.Join(",", roles.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}\r\n" +
                            $"撤销授权的菜单: \r\n\t{string.Join(",", menus.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                var menuIds = menus.Select(o => o.MenuId);

                if (Repository_RoleMenu.Delete(o => data.RoleIds.Contains(o.RoleId) && (menuIds.Contains(o.MenuId))) < 0)
                    throw new ApplicationException("撤销授权失败.");
            };

            if (runTransaction)
            {
                (bool success, Exception ex) = Orm.RunTransaction(handler);

                if (!success)
                    throw new ApplicationException("撤销授权失败.", ex);
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
                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(System_RoleResources),
                    DataId = null,
                    Explain = $"撤销角色的资源授权.",
                    Remark = $"被撤销授权的角色: \r\n\t{string.Join(",", roles.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}\r\n" +
                            $"撤销授权的资源: \r\n\t{string.Join(",", resourcess.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                var resourcesIds = resourcess.Select(o => o.ResourcesId);

                if (Repository_RoleResources.Delete(o => data.RoleIds.Contains(o.RoleId) && (resourcesIds.Contains(o.ResourcesId))) < 0)
                    throw new ApplicationException("撤销授权失败.");
            };

            if (runTransaction)
            {
                (bool success, Exception ex) = Orm.RunTransaction(handler);

                if (!success)
                    throw new ApplicationException("撤销授权失败.", ex);
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
                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(System_UserMenu) + "+" + nameof(System_RoleMenu),
                    Explain = $"撤销所有用户和角色的菜单授权.",
                    Remark = $"撤销授权的菜单: \r\n\t{string.Join(",", menus.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                if (Repository_UserMenu.Delete(o => (menuIds.Contains(o.MenuId))) < 0)
                    throw new ApplicationException("撤销用户的菜单授权失败.");

                if (Repository_RoleMenu.Delete(o => (menuIds.Contains(o.MenuId))) < 0)
                    throw new ApplicationException("撤销角色的菜单授权失败.");
            };

            if (runTransaction)
            {
                (bool success, Exception ex) = Orm.RunTransaction(handler);

                if (!success)
                    throw new ApplicationException("撤销授权失败.", ex);
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
                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(System_UserResources) + "+" + nameof(System_RoleResources),
                    Explain = $"撤销所有用户和角色的资源授权.",
                    Remark = $"撤销授权的资源: \r\n\t{string.Join(",", resources.Select(o => $"[名称 {o.Name}, 类型 {o.Type}]"))}"
                });

                if (Repository_UserResources.Delete(o => (resourcesIds.Contains(o.ResourcesId))) < 0)
                    throw new ApplicationException("撤销用户的资源授权失败.");

                if (Repository_RoleResources.Delete(o => (resourcesIds.Contains(o.ResourcesId))) < 0)
                    throw new ApplicationException("撤销角色的资源授权失败.");
            };

            if (runTransaction)
            {
                (bool success, Exception ex) = Orm.RunTransaction(handler);

                if (!success)
                    throw new ApplicationException("撤销授权失败.", ex);
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

        public Model.Public.MemberDTO.Authorities GetMember(string memberId, bool includeRole, bool includeMenu, bool includeResources)
        {
            var member = Repository_Member.Where(o => o.Id == memberId)
                                    .ToOne(o => new Model.Public.MemberDTO.Authorities
                                    {
                                        Id = o.Id,
                                        Account = o.Account,
                                        Enable = o.Enable
                                    });

            if (member == null)
                throw new ApplicationException("会员用户不存在或已被移除.");

            if (!member.Enable)
                throw new ApplicationException("会员账号已禁用.");

            if (includeRole)
                member._Roles = GetMemberRole(memberId, includeMenu, includeResources);

            return member;
        }

        public List<Model.System.RoleDTO.Authorities> GetUserRole(string userId, bool includeMenu, bool includeResources)
        {
            var roles = Repository_Role.Where(o => o.Users.AsSelect().Where(p => p.Id == userId).Any() && o.Enable)
                                         .ToList(o => new Model.System.RoleDTO.Authorities
                                         {
                                             Id = o.Id,
                                             Name = o.Name,
                                             Type = o.Type,
                                             Code = o.Code
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

        public List<Model.System.RoleDTO.Authorities> GetMemberRole(string memberId, bool includeMenu, bool includeResources)
        {
            var roles = Repository_Role.Where(o => o.Members.AsSelect().Where(p => p.Id == memberId).Any() && o.Enable)
                                         .ToList(o => new Model.System.RoleDTO.Authorities
                                         {
                                             Id = o.Id,
                                             Name = o.Name,
                                             Type = o.Type,
                                             Code = o.Code
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
            var menus = Repository_Menu.Where(o => (o.Users.AsSelect()
                                                            .Where(p => p.Id == userId)
                                                            .Any()
                                                    || (mergeRoleMenu
                                                        && o.Roles.AsSelect()
                                                                .Where(p => p.Users.AsSelect()
                                                                                .Where(q => q.Id == userId)
                                                                                .Any() && p.Enable)
                                                                .Any())) && o.Enable)
                                    .ToList(o => new Model.System.MenuDTO.Authorities
                                    {
                                        Id = o.Id,
                                        Name = o.Name,
                                        Type = o.Type,
                                        Code = o.Code,
                                        Uri = o.Uri,
                                        Method = o.Method
                                    });

            return menus;
        }

        public List<Model.System.MenuDTO.Authorities> GetMemberMenu(string memberId)
        {
            var menus = Repository_Menu.Where(o => o.Roles.AsSelect()
                                                        .Where(p => p.Members.AsSelect()
                                                                            .Where(q => q.Id == memberId)
                                                                            .Any() && p.Enable)
                                                        .Any() && o.Enable)
                                    .ToList(o => new Model.System.MenuDTO.Authorities
                                    {
                                        Id = o.Id,
                                        Name = o.Name,
                                        Type = o.Type,
                                        Code = o.Code,
                                        Uri = o.Uri,
                                        Method = o.Method
                                    });

            return menus;
        }

        public List<Model.System.ResourcesDTO.Authorities> GetUserResources(string userId, bool mergeRoleResources)
        {
            var resources = Repository_Resources.Where(o => (o.Users.AsSelect()
                                                            .Where(p => p.Id == userId)
                                                            .Any()
                                                    || (mergeRoleResources
                                                        && o.Roles.AsSelect()
                                                                .Where(p => p.Users.AsSelect()
                                                                                .Where(q => q.Id == userId)
                                                                                .Any() && p.Enable)
                                                                .Any())) && o.Enable)
                                    .ToList(o => new Model.System.ResourcesDTO.Authorities
                                    {
                                        Id = o.Id,
                                        Name = o.Name,
                                        Type = o.Type,
                                        Code = o.Code,
                                        Uri = o.Uri
                                    });

            return resources;
        }

        public List<Model.System.ResourcesDTO.Authorities> GetMemberResources(string memberId)
        {
            var resources = Repository_Resources.Where(o => o.Roles.AsSelect()
                                                        .Where(p => p.Members.AsSelect()
                                                                            .Where(q => q.Id == memberId)
                                                                            .Any() && p.Enable)
                                                        .Any() && o.Enable)
                                    .ToList(o => new Model.System.ResourcesDTO.Authorities
                                    {
                                        Id = o.Id,
                                        Name = o.Name,
                                        Type = o.Type,
                                        Code = o.Code,
                                        Uri = o.Uri
                                    });

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
            return Repository_Menu.Where(o => o.Roles.AsSelect()
                                            .Where(p => p.Id == roleId)
                                            .Any() && o.Enable)
                                .ToList(o => new Model.System.MenuDTO.Authorities
                                {
                                    Id = o.Id,
                                    Name = o.Name,
                                    Type = o.Type,
                                    Code = o.Code,
                                    Uri = o.Uri,
                                    Method = o.Method
                                });
        }

        public List<Model.System.ResourcesDTO.Authorities> GetRoleResources(string roleId)
        {
            return Repository_Resources.Where(o => o.Roles.AsSelect()
                                            .Where(p => p.Id == roleId)
                                            .Any() && o.Enable)
                                .ToList(o => new Model.System.ResourcesDTO.Authorities
                                {
                                    Id = o.Id,
                                    Name = o.Name,
                                    Type = o.Type,
                                    Code = o.Code,
                                    Uri = o.Uri
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

        public bool UserHasRole(string userId, string roleId)
        {
            return Repository_UserRole.Where(o => o.UserId == userId && o.RoleId == roleId).Any();
        }

        public bool MemberHasRole(string memberId, string roleId)
        {
            return Repository_MemberRole.Where(o => o.MemberId == memberId && o.RoleId == roleId).Any();
        }

        public bool UserHasMenu(string userId, string menuId)
        {
            return Repository_Menu.Where(o => o.Id == menuId
                                            && (o.Users.AsSelect().Where(p => p.Id == userId).Any()
                                                || o.Roles.AsSelect().Where(p => p.Users.AsSelect().Where(q => q.Id == userId).Any()).Any()))
                                .Any();
        }

        public bool MemberHasMenu(string userId, string menuId)
        {
            return Repository_Menu.Where(o => o.Id == menuId
                                            && o.Roles.AsSelect().Where(p => p.Users.AsSelect().Where(q => q.Id == userId).Any()).Any())
                                .Any();
        }

        public bool UserHasResources(string userId, string resourcesId)
        {
            return Repository_Resources.Where(o => o.Id == resourcesId
                                            && (o.Users.AsSelect().Where(p => p.Id == userId).Any()
                                                || o.Roles.AsSelect().Where(p => p.Users.AsSelect().Where(q => q.Id == userId).Any()).Any()))
                                .Any();
        }

        public bool UserHasResourcesUri(string userId, string resourcesUri)
        {
            return Repository_Resources.Where(o => o.Uri == resourcesUri
                                            && (o.Users.AsSelect().Where(p => p.Id == userId).Any()
                                                || o.Roles.AsSelect().Where(p => p.Users.AsSelect().Where(q => q.Id == userId).Any()).Any()))
                                .Any();
        }

        public bool MemberHasResources(string userId, string resourcesId)
        {
            return Repository_Resources.Where(o => o.Id == resourcesId
                                            && o.Roles.AsSelect().Where(p => p.Users.AsSelect().Where(q => q.Id == userId).Any()).Any())
                                .Any();
        }

        public bool MemberHasResourcesUri(string userId, string resourcesUri)
        {
            return Repository_Resources.Where(o => o.Uri == resourcesUri
                                            && o.Roles.AsSelect().Where(p => p.Users.AsSelect().Where(q => q.Id == userId).Any()).Any())
                                .Any();
        }

        #endregion

        #endregion
    }
}
