using AutoMapper;
using Business.Filter;
using Business.Interface.System;
using Business.Utils;
using Entity.System;
using FreeSql;
using Library.DataMapping.Gen;
using Library.Extension;
using Library.FreeSql.Extention;
using Library.FreeSql.Gen;
using Library.Models;
using Library.OpenApi.Extention;
using Library.SelectOption;
using Model.System.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;

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
            IAuthoritiesBusiness authoritiesBusiness)
        {
            Orm = freeSqlProvider.GetFreeSql();
            Repository = Orm.GetRepository<System_User, string>();
            Repository_UserRole = Orm.GetRepository<System_UserRole, string>();
            Repository_UserMenu = Orm.GetRepository<System_UserMenu, string>();
            Repository_UserResources = Orm.GetRepository<System_UserResources, string>();
            Mapper = autoMapperProvider.GetMapper();
            OperationRecordBusiness = operationRecordBusiness;
            AuthoritiesBusiness = authoritiesBusiness;
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

        IAuthoritiesBusiness AuthoritiesBusiness { get; set; }

        #endregion

        #region 公共

        #region 基础功能

        public List<List> GetList(Pagination pagination)
        {
            var entityList = Repository.Select
                                    .ToList<System_User, List>(pagination, typeof(List).GetNamesWithTagAndOther(true, "_List"));

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
                            .ToList<System_User, List>(pagination, typeof(List).GetNamesWithTagAndOther(true, "_List"))
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
        public void Create(Create data)
        {
            var newData = Mapper.Map<System_User>(data).InitEntity();

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                Repository.Insert(newData);

                var orId = OperationRecordBusiness.Create(new System_OperationRecord
                {
                    DataType = nameof(System_User),
                    DataId = newData.Id,
                    Explain = $"创建用户[账号 {newData.Account}, 类型 {newData.Type}, 姓名 {newData.Name}]."
                });
            });

            if (!success)
                throw new ApplicationException("创建用户失败", ex);
        }

        [AdministratorOnly]
        public Edit GetEdit(string id)
        {
            var entity = Repository.GetAndCheckNull(id);

            var result = Mapper.Map<Edit>(entity);

            return result;
        }

        [AdministratorOnly]
        public void Edit(Edit data)
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
                    Explain = $"修改用户[账号 {entity.Account}, 类型 {entity.Type}, 姓名 {entity.Name}].",
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
        }

        [AdministratorOnly]
        public void Delete(List<string> ids)
        {
            var entityList = Repository.Select.Where(c => ids.Contains(c.Id)).ToList(c => new { c.Id, c.Account, c.Name, c.Type });

            var orList = new List<System_OperationRecord>();

            foreach (var entity in entityList)
            {
                orList.Add(new System_OperationRecord
                {
                    DataType = nameof(System_User),
                    DataId = entity.Id,
                    Explain = $"删除用户[账号 {entity.Account}, 类型 {entity.Type}, 姓名 {entity.Name}]."
                });
            }

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                AuthoritiesBusiness.RevocationRoleForUser(ids, false);

                AuthoritiesBusiness.RevocationMenuForUser(ids, false);

                AuthoritiesBusiness.RevocationResourcesForUser(ids, false);

                var orIds = OperationRecordBusiness.Create(orList);

                if (Repository.Delete(o => ids.Contains(o.Id)) <= 0)
                    throw new ApplicationException("未删除任何数据");
            });

            if (!success)
                throw new ApplicationException("删除用户失败", ex);
        }

        #endregion

        #region 拓展功能



        #endregion

        #endregion
    }
}
