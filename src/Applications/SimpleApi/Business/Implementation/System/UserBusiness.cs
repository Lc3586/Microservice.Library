using AutoMapper;
using Business.Interface.System;
using Business.Util;
using Entity.System;
using FreeSql;
using Library.Container;
using Library.DataMapping.Gen;
using Library.FreeSql.Extention;
using Library.FreeSql.Gen;
using Library.Models;
using Library.OpenApi.Extention;
using Library.SelectOption;
using Model.System.UserDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Library.Extension;
using Model.System;
using Business.Filter;

namespace Business.Implementation.System
{
    /// <summary>
    /// 系统用户业务类
    /// </summary>
    public class UserBusiness : BaseBusiness, IUserBusiness
    {
        #region DI
        public UserBusiness(
            IFreeSqlProvider freeSqlProvider,
            IAutoMapperProvider autoMapperProvider,
            IOperationRecordBusiness operationRecordBusiness,
            IRoleBusiness roleBusiness)
        {
            Orm = freeSqlProvider.GetFreeSql();
            Repository = Orm.GetRepository<System_User, string>();
            Repository_UserRole = Orm.GetRepository<System_UserRole, string>();
            Repository_UserMenu = Orm.GetRepository<System_UserMenu, string>();
            Repository_UserResources = Orm.GetRepository<System_UserResources, string>();
            Mapper = autoMapperProvider.GetMapper();
            OperationRecordBusiness = operationRecordBusiness;
            RoleBusiness = roleBusiness;
        }

        #endregion

        #region 私有成员

        IFreeSql Orm { get; set; }

        IBaseRepository<System_User, string> Repository { get; set; }

        IBaseRepository<System_UserRole, string> Repository_UserRole { get; set; }

        IBaseRepository<System_UserMenu, string> Repository_UserMenu { get; set; }

        IBaseRepository<System_UserResources, string> Repository_UserResources { get; set; }

        IMapper Mapper { get; set; }

        IOperationRecordBusiness OperationRecordBusiness { get; set; }

        IRoleBusiness RoleBusiness { get; set; }

        #endregion

        #region 公共

        #region 基础功能

        public List<List> GetList(Pagination pagination)
        {
            var entityList = Repository.Select
                                    .ToList<System_User, List>(Orm, pagination, typeof(List).GetNamesWithTagAndOther(true, "_List"));

            var result = Mapper.Map<List<List>>(entityList);

            return result;
        }

        public List<SelectOption> DropdownList(string condition, Pagination pagination)
        {
            var fields = new[] {
                nameof(System_User.Account),
                nameof(System_User.Name)
            };

            var where = new List<string>();
            if (!condition.IsNullOrWhiteSpace())
            {
                var index = 0;
                var add = false;
                foreach (var item in condition)
                {
                    if (item != ' ')
                    {
                        if (where.Count < index + 1)
                            where.Add("");
                        where[index] += item;
                        add = false;
                    }
                    else
                    {
                        if (add)
                            continue;
                        index++;
                        add = true;
                    }
                }
            }

            var where_sql = where.Any() ? string.Join(" or ", where.Select(o => string.Join(" or ", fields.Select(p => $"a.\"{p}\" like '%{o}%'")))) : null;

            var type = typeof(System_User);

            var select = SelectExtension.Select<System_User, SelectOption>(a => new SelectOption
            {
                text = a.Account,
                value = a.Id,
                search = $"{a.Account} {a.Name}",
                options = new List<OptionInfo>
                {
                    new OptionInfo
                    {
                        display = type.GetDescription(nameof(a.Account)),
                        value = a.Account
                    },
                    new OptionInfo
                    {
                        display =type.GetDescription(nameof(a.Name)),
                        value = a.Name
                    },
                    new OptionInfo
                    {
                        display = type.GetDescription(nameof(a.CreateTime)),
                        value = a.CreateTime
                    }
                }
            });

            var list = from a in Orm.Select<System_User>()
                            .Where(where_sql)
                            .ToList<System_User, List>(Orm, pagination, typeof(List).GetNamesWithTagAndOther(true, "_List"))
                       select @select.Invoke(a);

            return list.ToList();
        }

        public Detail GetDetail(string id)
        {
            var entity = Repository.GetAndCheckNull(id);

            var result = Mapper.Map<Detail>(entity);

            return result;
        }

        [AdministratorOnly]
        public AjaxResult Create(Create data)
        {
            var newData = Mapper.Map<System_User>(data).InitEntity();

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                Repository.Insert(newData);

                var orId = OperationRecordBusiness.Create(new System_OperationRecord
                {
                    DataType = nameof(System_User),
                    DataId = newData.Id,
                    Explain = $"{DateTime.Now:yyyy年MM月dd日 HH时mm分ss秒}: 创建用户[账号 {newData.Account}, 类型 {entity.Type}, 姓名 {newData.Name}]."
                });
            });

            if (!success)
                throw new ApplicationException("创建用户失败", ex);

            return Success();
        }

        [AdministratorOnly]
        public AjaxResult<Edit> GetEdit(string id)
        {
            var entity = Repository.GetAndCheckNull(id);

            var result = Mapper.Map<Edit>(entity);

            return Success(result);
        }

        [AdministratorOnly]
        public AjaxResult Edit(Edit data)
        {
            var editData = Mapper.Map<System_User>(data).ModifyEntity();

            var entity = Repository.GetAndCheckNull(editData.Id);

            var changed_ = string.Join(",",
                                       entity.GetPropertyValueChangeds<System_User, Edit>(editData)
                                            .Select(p => $"\r\n\t {p.Description}：{p.FormerValue} 更改为 {p.CurrentValue}"));

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var orId = OperationRecordBusiness.Create(new System_OperationRecord
                {
                    DataType = nameof(System_User),
                    DataId = entity.Id,
                    Explain = $"{DateTime.Now:yyyy年MM月dd日 HH时mm分ss秒}: 修改用户[账号 {entity.Account}, 类型 {entity.Type}, 姓名 {entity.Name}].",
                    Remark = $"变更详情: \r\n\t{changed_}"
                });

                if (Repository.UpdateDiy
                      .SetSource(editData.ModifyEntity())
                      .UpdateColumns(typeof(Edit).GetNamesWithTagAndOther(false, "_Edit").ToArray())
                      .ExecuteAffrows() <= 0)
                    throw new ApplicationException("修改用户失败");
            });

            if (!success)
                throw ex;

            return Success();
        }

        [AdministratorOnly]
        public AjaxResult Delete(List<string> ids)
        {
            var _ids = ids.Select(o => o);

            var entityList = Repository.Select.Where(c => _ids.Contains(c.Id)).ToList(c => new { c.Id, c.Account, c.Name, c.Type });

            var orList = new List<System_OperationRecord>();

            foreach (var entity in entityList)
            {
                orList.Add(new System_OperationRecord
                {
                    DataType = nameof(System_User),
                    DataId = entity.Id,
                    Explain = $"{DateTime.Now:yyyy年MM月dd日 HH时mm分ss秒}: 删除用户[账号 {entity.Account}, 类型 {entity.Type}, 姓名 {entity.Name}]."
                });
            }

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                if (Repository_UserRole.Delete(o => _ids.Contains(o.UserId)) < 0)
                    throw new ApplicationException("撤销角色授权失败");

                if (Repository_UserMenu.Delete(o => _ids.Contains(o.UserId)) < 0)
                    throw new ApplicationException("撤销菜单授权失败");

                if (Repository_UserResources.Delete(o => _ids.Contains(o.UserId)) < 0)
                    throw new ApplicationException("撤销资源授权失败");

                if (Repository.Delete(o => _ids.Contains(o.Id)) <= 0)
                    throw new ApplicationException("未删除任何数据");

                var orIds = OperationRecordBusiness.Create(orList);
            });

            if (!success)
                throw new ApplicationException("删除用户失败", ex);

            return Success();
        }

        #endregion

        #region 拓展功能

        public bool IsAdmin(string id)
        {
            return Repository.Where(o => o.Id == id && o.Roles.AsSelect().Any(o => o.Type == RoleType.超级管理员 || o.Type == RoleType.管理员)).Any();
        }

        public Base GetBase(string id, bool includeRole, bool includeMenu, bool includeResources, bool mergeRoleMenu, bool mergeRoleResources)
        {
            var entity = Repository.GetAndCheckNull(id);

            var result = Mapper.Map<Base>(entity);

            if (includeRole)
                result._Roles = GetRolesBase(id, includeMenu && !mergeRoleMenu, includeResources && !mergeRoleResources);

            if (includeMenu)
                result._Menus = GetMenuAuthoritiesBase(id, mergeRoleMenu);

            if (includeResources)
                result._Resources = GetResourcesAuthoritiesBase(id, mergeRoleResources);

            return result;
        }

        public List<Model.System.RoleDTO.Base> GetRolesBase(string id, bool includeMenu, bool includeResources)
        {
            var roleIdList = Repository.Select
                                     .Where(o => o.Id == id)
                                     .IncludeMany(o => o.Roles)
                                     .ToOne(o => o.Roles
                                                 .AsSelect()
                                                 .ToList(o => o.Id));

            var result = new List<Model.System.RoleDTO.Base>();

            roleIdList.ForEach(o => result.Add(RoleBusiness.GetBase(o, includeMenu, includeResources)));

            return result;
        }

        public List<Model.System.MenuDTO.Base> GetMenuAuthoritiesBase(string id, bool includeRoleAuthorities)
        {
            var menuList = Repository.Select
                                        .Where(o => o.Id == id)
                                        .ToOne(o => o.Menus
                                                    .AsSelect()
                                                    .ToList<System_Menu, Model.System.MenuDTO.Base>
                                                    (
                                                        Orm,
                                                        null,
                                                        typeof(Model.System.MenuDTO.Base).GetNamesWithTag(true)
                                                    ));

            if (includeRoleAuthorities)
            {
                var roleMenuList = Repository.Select
                                        .Where(o => o.Id == id)
                                        .IncludeMany(o => o.Roles)
                                        .ToOne(o => o.Roles
                                                        .AsSelect()
                                                        .ToList(p => p.Menus
                                                                        .AsSelect()
                                                                        .ToList<System_Menu, Model.System.MenuDTO.Base>
                                                                        (
                                                                            Orm,
                                                                            null,
                                                                            typeof(Model.System.MenuDTO.Base).GetNamesWithTag(true)
                                                                        ))
                                                        .SelectMany(p => p));

                menuList.AddRange(roleMenuList);
            }

            var result = Mapper.Map<List<Model.System.MenuDTO.Base>>(menuList);

            return result;
        }

        public List<Model.System.ResourcesDTO.Base> GetResourcesAuthoritiesBase(string id, bool includeRoleAuthorities)
        {
            var menuList = Repository.Select
                                        .Where(o => o.Id == id)
                                        .ToOne(o => o.Resources
                                                    .AsSelect()
                                                    .ToList<System_Resources, Model.System.ResourcesDTO.Base>
                                                    (
                                                        Orm,
                                                        null,
                                                        typeof(Model.System.ResourcesDTO.Base).GetNamesWithTag(true)
                                                    ));

            if (includeRoleAuthorities)
            {
                var roleMenuList = Repository.Select
                                        .Where(o => o.Id == id)
                                        .IncludeMany(o => o.Roles)
                                        .ToOne(o => o.Roles
                                                        .AsSelect()
                                                        .ToList(p => p.Resources
                                                                        .AsSelect()
                                                                        .ToList<System_Resources, Model.System.ResourcesDTO.Base>
                                                                        (
                                                                            Orm,
                                                                            null,
                                                                            typeof(Model.System.ResourcesDTO.Base).GetNamesWithTag(true)
                                                                        ))
                                                        .SelectMany(p => p));

                menuList.AddRange(roleMenuList);
            }

            var result = Mapper.Map<List<Model.System.ResourcesDTO.Base>>(menuList);

            return result;
        }

        #endregion

        #endregion
    }
}
