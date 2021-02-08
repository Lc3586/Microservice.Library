﻿using AutoMapper;
using Business.Filter;
using Business.Interface.Common;
using Business.Interface.System;
using Business.Utils;
using Business.Utils.Pagination;
using Entity.Common;
using Entity.System;
using FreeSql;
using Library.DataMapping.Gen;
using Library.Extension;
using Library.FreeSql.Extention;
using Library.FreeSql.Gen;
using Library.OpenApi.Extention;
using Library.SelectOption;
using Model.Common;
using Model.System;
using Model.System.Pagination;
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
            IAuthoritiesBusiness authoritiesBusiness,
            IEntryLogBusiness entryLogBusiness)
        {
            Orm = freeSqlProvider.GetFreeSql();
            Repository = Orm.GetRepository<System_User, string>();
            Repository_UserRole = Orm.GetRepository<System_UserRole, string>();
            Repository_UserMenu = Orm.GetRepository<System_UserMenu, string>();
            Repository_UserResources = Orm.GetRepository<System_UserResources, string>();
            Mapper = autoMapperProvider.GetMapper();
            OperationRecordBusiness = operationRecordBusiness;
            AuthoritiesBusiness = authoritiesBusiness;
            EntryLogBusiness = entryLogBusiness;
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

        IEntryLogBusiness EntryLogBusiness { get; set; }

        #endregion

        #region 公共

        #region 基础功能

        public List<List> GetList(PaginationDTO pagination)
        {
            var entityList = Repository.Select
                                    .GetPagination(pagination)
                                    .ToList<System_User, List>(typeof(List).GetNamesWithTagAndOther(true, "_List"));

            var result = Mapper.Map<List<List>>(entityList);

            return result;
        }

        public List<SelectOption> DropdownList(string condition, PaginationDTO pagination)
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
                        display =type.GetDescription(nameof(a.HeadimgUrl)),
                        value = a.HeadimgUrl,
                        displayType = OptionDisplayType.image
                    },
                    new OptionInfo
                    {
                        display =type.GetDescription(nameof(a.Name)),
                        value = a.Name
                    },
                    new OptionInfo
                    {
                        display =type.GetDescription(nameof(a.Tel)),
                        value = a.Tel
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
                            .GetPagination(pagination)
                            .ToList<System_User, List>(typeof(List).GetNamesWithTagAndOther(true, "_List"))
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
        public void Create(Create data, bool withOP = true)
        {
            var newData = Mapper.Map<System_User>(data).InitEntity();

            newData.Password = $"{newData.Account}{newData.Password}".ToMD5String();

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                Repository.Insert(newData);

                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(System_User),
                    DataId = newData.Id,
                    Explain = $"创建用户[账号 {newData.Account}, 姓名 {newData.Name}]."
                }, withOP);
            });

            if (!success)
                throw new ApplicationException("创建用户失败.", ex);
        }

        [AdministratorOnly]
        public Edit GetEdit(string id)
        {
            var entity = Repository.GetAndCheckNull(id);

            var result = Mapper.Map<Edit>(entity);

            return result;
        }

        [AdministratorOnly]
        public void Edit(Edit data, bool withOP = true)
        {
            var editData = Mapper.Map<System_User>(data).ModifyEntity();

            var entity = Repository.GetAndCheckNull(editData.Id);

            var changed_ = string.Join(",",
                                       entity.GetPropertyValueChangeds<System_User, Edit>(editData)
                                            .Select(p => $"\r\n\t {p.Description}：{p.FormerValue} 更改为 {p.CurrentValue}"));

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(System_User),
                    DataId = entity.Id,
                    Explain = $"修改用户[账号 {entity.Account}, 姓名 {entity.Name}].",
                    Remark = $"变更详情: \r\n\t{changed_}"
                }, withOP);

                if (Repository.UpdateDiy
                      .SetSource(editData.ModifyEntity())
                      .UpdateColumns(typeof(Edit).GetNamesWithTagAndOther(false, "_Edit").ToArray())
                      .ExecuteAffrows() <= 0)
                    throw new ApplicationException("修改用户失败.");
            });

            if (!success)
                throw ex;
        }

        [AdministratorOnly]
        public void Delete(List<string> ids)
        {
            var entityList = Repository.Select.Where(c => ids.Contains(c.Id)).ToList(c => new { c.Id, c.Account, c.Name });

            var orList = new List<Common_OperationRecord>();

            foreach (var entity in entityList)
            {
                orList.Add(new Common_OperationRecord
                {
                    DataType = nameof(System_User),
                    DataId = entity.Id,
                    Explain = $"删除用户[账号 {entity.Account}, 姓名 {entity.Name}]."
                });
            }

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                AuthoritiesBusiness.RevocationRoleForUser(ids, false);

                AuthoritiesBusiness.RevocationMenuForUser(ids, false);

                AuthoritiesBusiness.RevocationResourcesForUser(ids, false);

                var orIds = OperationRecordBusiness.Create(orList);

                if (Repository.Delete(o => ids.Contains(o.Id)) <= 0)
                    throw new ApplicationException("未删除任何数据.");
            });

            if (!success)
                throw new ApplicationException("删除用户失败.", ex);
        }

        #endregion

        #region 拓展功能

        public void Login(Login data)
        {
            var user = Repository.Where(o => o.Account == data.Account)
                .ToOne(o => new { o.Id, o.Account, o.Name, o.HeadimgUrl, o.Enable, o.Password });

            if (user == null)
                throw new ApplicationException("账号不存在或已被移除.");

            if (!user.Enable)
                throw new ApplicationException("账号已被禁用.");

            if (!$"{data.Account}{data.Password}".ToMD5String().Equals(user.Password))
                throw new ApplicationException("密码错误.");

            EntryLogBusiness.Create(new Common_EntryLog
            {
                UserType = UserType.系统用户,
                Account = user.Account,
                Name = user.Name,
                HeadimgUrl = user.HeadimgUrl,
                IsAdmin = AuthoritiesBusiness.IsAdminUser(user.Id),
                Remark = "使用账号密码信息登录系统."
            });
        }

        public void Login(string openId)
        {
            var user = Repository.Where(o => o.WeChatUserInfos.AsSelect().Where(p => p.OpenId == openId).Any())
                .ToOne(o => new { o.Id, o.Account, o.Name, o.HeadimgUrl, o.Enable, o.Password });

            if (user == null)
                throw new ApplicationException("账号还未绑定微信.");

            if (!user.Enable)
                throw new ApplicationException("账号已被禁用.");

            EntryLogBusiness.Create(new Common_EntryLog
            {
                UserType = UserType.系统用户,
                Account = user.Account,
                Name = user.Name,
                HeadimgUrl = user.HeadimgUrl,
                IsAdmin = AuthoritiesBusiness.IsAdminUser(user.Id),
                Remark = "使用微信信息登录系统."
            });
        }

        public void UpdatePassword(UpdatePassword data)
        {
            var editData = Mapper.Map<System_User>(data).ModifyEntity();

            var entity = Repository.GetAndCheckNull(editData.Id);

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(System_User),
                    DataId = entity.Id,
                    Explain = $"更新密码[账号 {entity.Account}, 姓名 {entity.Name}]."
                });

                if (Repository.UpdateDiy
                      .SetSource(editData.ModifyEntity())
                      .UpdateColumns(typeof(UpdatePassword).GetNamesWithTagAndOther(false, "_Edit").ToArray())
                      .ExecuteAffrows() <= 0)
                    throw new ApplicationException("更新密码失败");
            });

            if (!success)
                throw ex;
        }

        public OperatorDetail GetOperatorDetail(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            return Repository.Where(o => o.Id == id)
                .ToOne(o => new OperatorDetail
                {
                    Account = o.Account,
                    Name = o.Name,
                    Tel = o.Tel
                });
        }

        #endregion

        #endregion
    }
}
