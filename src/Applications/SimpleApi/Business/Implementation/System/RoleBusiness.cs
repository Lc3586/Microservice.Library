﻿using AutoMapper;
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
using Microsoft.AspNetCore.Http;
using Model.System.RoleDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Library.Extension;
using Library.FreeSql.Annotations;
using System.ComponentModel;
using System.Reflection;
using Model.System;
using Business.Filter;

namespace Business.Implementation.System
{
    /// <summary>
    /// 角色业务类
    /// </summary>
    public class RoleBusiness : BaseBusiness, IRoleBusiness
    {
        #region DI
        public RoleBusiness(
            IFreeSqlProvider freeSqlProvider,
            IAutoMapperProvider autoMapperProvider,
            IOperationRecordBusiness operationRecordBusiness,
            IAuthoritiesBusiness authoritiesBusiness)
        {
            Orm = freeSqlProvider.GetFreeSql();
            Repository = Orm.GetRepository<System_Role, string>();
            Repository_RoleMenu = Orm.GetRepository<System_RoleMenu, string>();
            Repository_RoleResources = Orm.GetRepository<System_RoleResources, string>();
            Mapper = autoMapperProvider.GetMapper();
            OperationRecordBusiness = operationRecordBusiness;
            AuthoritiesBusiness = authoritiesBusiness;
        }

        #endregion

        #region 私有成员

        IFreeSql Orm { get; set; }

        IBaseRepository<System_Role, string> Repository { get; set; }

        IBaseRepository<System_RoleMenu, string> Repository_RoleMenu { get; set; }

        IBaseRepository<System_RoleResources, string> Repository_RoleResources { get; set; }

        IMapper Mapper { get; set; }

        IOperationRecordBusiness OperationRecordBusiness { get; set; }

        IAuthoritiesBusiness AuthoritiesBusiness { get; set; }

        #endregion

        #region 公共

        #region 基础功能

        public List<List> GetList(Pagination pagination)
        {
            var entityList = Repository.Select
                                    .ToList<System_Role, List>(pagination, typeof(List).GetNamesWithTagAndOther(true, "_List"));

            var result = Mapper.Map<List<List>>(entityList);

            return result;
        }

        public List<TreeList> GetTreeList(TreeListParamter paramter, bool deep = false)
        {
            if (paramter.ParentId.IsNullOrWhiteSpace())
                paramter.ParentId = null;

            var entityList = Repository.Select
                                    .Where(o => o.ParentId == paramter.ParentId)
                                    .ToList<System_Role, TreeList>(null, typeof(List).GetNamesWithTagAndOther(true, "_List"));

            var result = Mapper.Map<List<TreeList>>(entityList);

            if (result.Any())
                result.ForEach(o =>
                {
                    if (!paramter.Rank.HasValue || paramter.Rank > 0)
                        o.Childs_ = GetTreeList(new TreeListParamter
                        {
                            ParentId = o.Id,
                            Rank = --paramter.Rank,
                            RoleType = paramter.RoleType
                        }, true);
                });
            else if (deep)
                result = null;

            return result;
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
            var newData = Mapper.Map<System_Role>(data).InitEntity();

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                if (newData.ParentId.IsNullOrWhiteSpace())
                {
                    newData.ParentId = null;
                    newData.Level = 0;
                    newData.RootId = null;
                }
                else
                {
                    var parent = Repository.GetAndCheckNull(newData.ParentId);
                    newData.Level = parent.Level + 1;
                    newData.RootId = parent.RootId == null ? parent.Id : parent.RootId;
                }

                newData.Sort = Repository.Where(o => o.ParentId == newData.ParentId).Max(o => o.Sort) + 1;

                Repository.Insert(newData);

                var orId = OperationRecordBusiness.Create(new System_OperationRecord
                {
                    DataType = nameof(System_Role),
                    DataId = newData.Id,
                    Explain = $"创建角色[名称 {newData.Name}, 类型 {newData.Type}]."
                });
            });

            if (!success)
                throw new ApplicationException("创建角色失败", ex);

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
            var editData = Mapper.Map<System_Role>(data).ModifyEntity();

            var entity = Repository.GetAndCheckNull(editData.Id);

            var changed_ = string.Join(",",
                                       entity.GetPropertyValueChangeds<System_Role, Edit>(editData)
                                            .Select(p => $"\r\n\t {p.Description}：{p.FormerValue} 更改为 {p.CurrentValue}"));

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                if (editData.ParentId != entity.ParentId)
                {
                    if (editData.ParentId.IsNullOrWhiteSpace())
                    {
                        editData.ParentId = null;
                        editData.Level = 0;
                        editData.RootId = null;
                    }
                    else
                    {
                        var parent = Repository.GetAndCheckNull(editData.ParentId);
                        editData.Level = parent.Level + 1;
                        editData.RootId = parent.RootId == null ? parent.Id : parent.RootId;
                    }

                    editData.Sort = Repository.Where(o => o.ParentId == editData.ParentId).Max(o => o.Sort) + 1;

                    if (Repository.UpdateDiy
                             .Where(o => o.ParentId == entity.ParentId && o.Id != entity.Id && o.Sort > entity.Sort)
                             .Set(o => o.Sort - 1)
                             .ExecuteAffrows() < 0)
                        throw new ApplicationException("重新排序失败.");
                }

                var orId = OperationRecordBusiness.Create(new System_OperationRecord
                {
                    DataType = nameof(System_Role),
                    DataId = entity.Id,
                    Explain = $"修改角色[名称 {entity.Name}, 类型 {entity.Type}].",
                    Remark = $"变更详情: \r\n\t{changed_}"
                });

                if (Repository.UpdateDiy
                      .SetSource(editData.ModifyEntity())
                      .UpdateColumns(typeof(Edit).GetNamesWithTagAndOther(false, "_Edit").ToArray())
                      .ExecuteAffrows() <= 0)
                    throw new ApplicationException("修改角色失败");
            });

            if (!success)
                throw ex;

            return Success();
        }

        [AdministratorOnly]
        public AjaxResult Enable(string id, bool enable)
        {
            var entity = Repository.GetAndCheckNull(id);

            entity.Enable = enable;

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var orId = OperationRecordBusiness.Create(new System_OperationRecord
                {
                    DataType = nameof(System_Role),
                    DataId = entity.Id,
                    Explain = $"{(enable ? "启用" : "禁用")}角色[名称 {entity.Name}, 类型 {entity.Type}]."
                });

                if (Repository.Update(entity) <= 0)
                    throw new ApplicationException($"{(enable ? "启用" : "禁用")}角色失败");
            });

            if (!success)
                throw ex;

            return Success();
        }

        [AdministratorOnly]
        public AjaxResult Sort(Sort data)
        {
            if (data.Span == 0 && (data.Type != Model.System.SortType.top || data.Type != Model.System.SortType.low))
                return Success();

            var current = Repository.Where(o => o.Id == data.Id)
                                    .ToOne(o => new
                                    {
                                        o.Id,
                                        o.ParentId,
                                        o.Name,
                                        o.Type,
                                        o.Sort
                                    });

            if (current.Id == null)
                return Error("数据不存在或已被移除.");

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var newSort = current.Sort;

                switch (data.Type)
                {
                    case Model.System.SortType.top:
                        if (Repository.UpdateDiy
                                 .Where(o => o.ParentId == current.ParentId && o.Id != current.Id)
                                 .Set(o => o.Sort + 1)
                                 .ExecuteAffrows() < 0)
                            throw new ApplicationException("角色排序失败.");

                        newSort = 0;
                        break;
                    case Model.System.SortType.up:
                        if (Repository.UpdateDiy
                                 .Where(o => o.ParentId == current.ParentId && o.Id != current.Id && o.Sort < current.Sort && o.Sort >= current.Sort - data.Span)
                                 .Set(o => o.Sort + 1)
                                 .ExecuteAffrows() < 0)
                            throw new ApplicationException("角色排序失败.");

                        newSort -= data.Span;
                        break;
                    case Model.System.SortType.down:
                        if (Repository.UpdateDiy
                                 .Where(o => o.ParentId == current.ParentId && o.Id != current.Id && o.Sort > current.Sort && o.Sort <= current.Sort + data.Span)
                                 .Set(o => o.Sort - 1)
                                 .ExecuteAffrows() < 0)
                            throw new ApplicationException("角色排序失败.");

                        newSort += data.Span;
                        break;
                    case Model.System.SortType.low:
                        newSort = Repository.Select.Max(o => o.Sort);

                        if (Repository.UpdateDiy
                                 .Where(o => o.ParentId == current.ParentId && o.Id != current.Id)
                                 .Set(o => o.Sort - 1)
                                 .ExecuteAffrows() < 0)
                            throw new ApplicationException("角色排序失败.");
                        break;
                    default:
                        throw new ApplicationException($"不支持的排序类型 {data.Type}.");
                }

                var orId = OperationRecordBusiness.Create(new System_OperationRecord
                {
                    DataType = nameof(System_Role),
                    DataId = current.Id,
                    Explain = $"角色排序[名称 {current.Name}, 类型 {current.Type}]."
                });

                if (Repository.UpdateDiy
                        .Where(o => o.Id == current.Id)
                        .Set(o => o.Sort, newSort)
                        .ExecuteAffrows() <= 0)
                    throw new ApplicationException("角色排序失败.");
            });

            if (!success)
                throw ex;

            return Success();
        }

        [AdministratorOnly]
        public AjaxResult DragSort(DragSort data)
        {
            if (data.Id == data.TargetId)
                return Success();

            var current = Repository.Where(o => o.Id == data.Id)
                                    .ToOne(o => new
                                    {
                                        o.Id,
                                        o.ParentId,
                                        o.Name,
                                        o.Type,
                                        o.Sort
                                    });

            if (current.Id == null)
                return Error("数据不存在或已被移除.");

            var target = Repository.Where(o => o.Id == data.TargetId)
                                    .ToOne(o => new
                                    {
                                        o.Id,
                                        o.ParentId,
                                        o.Sort,
                                        o.Level,
                                        o.RootId
                                    });

            if (target.Id == null)
                return Error("目标数据不存在.");

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                if (current.ParentId != target.ParentId)
                {
                    if (Repository.UpdateDiy
                             .Where(o => o.ParentId == current.ParentId && o.Id != current.Id && o.Sort > current.Sort)
                             .Set(o => o.Sort - 1)
                             .ExecuteAffrows() < 0)
                        throw new ApplicationException("角色排序失败.");

                    if (Repository.UpdateDiy
                             .Where(o => o.ParentId == target.ParentId && o.Sort >= target.Sort)
                             .Set(o => o.Sort + 1)
                             .ExecuteAffrows() < 0)
                        throw new ApplicationException("角色排序失败.");

                    if (current.ParentId.IsNullOrWhiteSpace())
                    {
                        if (Repository.UpdateDiy
                                .Where(o => o.Id == current.Id)
                                .Set(o => o.Sort, target.Sort)
                                .Set(o => o.ParentId, null)
                                .Set(o => o.Level, 0)
                                .Set(o => o.RootId, null)
                                .ExecuteAffrows() <= 0)
                            throw new ApplicationException("角色排序失败.");
                    }
                    else
                    {
                        if (Repository.UpdateDiy
                                .Where(o => o.Id == current.Id)
                                .Set(o => o.Sort, target.Sort)
                                .Set(o => o.ParentId, target.ParentId)
                                .Set(o => o.Level, target.Level)
                                .Set(o => o.RootId, target.RootId)
                                .ExecuteAffrows() <= 0)
                            throw new ApplicationException("角色排序失败.");
                    }
                }
                else
                {
                    if (Repository.UpdateDiy
                             .Where(o => o.ParentId == current.ParentId && o.Id != current.Id && (current.Sort > target.Sort ? (o.Sort < current.Sort && o.Sort >= target.Sort) : (o.Sort > current.Sort && o.Sort <= target.Sort)))
                             .Set(o => current.Sort > target.Sort ? o.Sort + 1 : o.Sort - 1)
                             .ExecuteAffrows() < 0)
                        throw new ApplicationException("角色排序失败.");

                    if (Repository.UpdateDiy
                            .Where(o => o.Id == current.Id)
                            .Set(o => o.Sort, target.Sort)
                            .ExecuteAffrows() <= 0)
                        throw new ApplicationException("角色排序失败.");
                }

                _ = OperationRecordBusiness.Create(new System_OperationRecord
                {
                    DataType = nameof(System_Role),
                    DataId = current.Id,
                    Explain = $"角色排序[名称 {current.Name}, 类型 {current.Type}]."
                });
            });

            if (!success)
                throw ex;

            return Success();
        }

        [AdministratorOnly]
        public AjaxResult Delete(List<string> ids)
        {
            var entityList = Repository.Select.Where(c => ids.Contains(c.Id)).ToList(c => new { c.Id, c.Name, c.Type });

            var orList = new List<System_OperationRecord>();

            foreach (var entity in entityList)
            {
                orList.Add(new System_OperationRecord
                {
                    DataType = nameof(System_Role),
                    DataId = entity.Id,
                    Explain = $"删除角色[名称 {entity.Name}, 类型 {entity.Type}]."
                });
            }

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                AuthoritiesBusiness.RevocationMenuForRole(ids, false);

                AuthoritiesBusiness.RevocationResourcesForRole(ids, false);

                if (Repository.Delete(o => ids.Contains(o.Id)) <= 0)
                    throw new ApplicationException("未删除任何数据");

                var orIds = OperationRecordBusiness.Create(orList);
            });

            if (!success)
                throw new ApplicationException("删除角色失败", ex);

            return Success();
        }

        #endregion

        #region 拓展功能



        #endregion

        #endregion
    }
}