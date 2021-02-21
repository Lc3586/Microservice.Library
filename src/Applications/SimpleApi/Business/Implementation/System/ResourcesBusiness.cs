using AutoMapper;
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
using Model.System.ResourcesDTO;
using Model.Utils.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Implementation.System
{
    /// <summary>
    /// 资源业务类
    /// </summary>
    public class ResourcesBusiness : BaseBusiness, IResourcesBusiness
    {
        #region DI

        public ResourcesBusiness(
            IFreeSqlProvider freeSqlProvider,
            IAutoMapperProvider autoMapperProvider,
            IOperationRecordBusiness operationRecordBusiness,
            IAuthoritiesBusiness authoritiesBusiness)
        {
            Orm = freeSqlProvider.GetFreeSql();
            Repository = Orm.GetRepository<System_Resources, string>();
            Mapper = autoMapperProvider.GetMapper();
            OperationRecordBusiness = operationRecordBusiness;
            AuthoritiesBusiness = authoritiesBusiness;
        }

        #endregion

        #region 私有成员

        IFreeSql Orm { get; set; }

        IBaseRepository<System_Resources, string> Repository { get; set; }

        IMapper Mapper { get; set; }

        IOperationRecordBusiness OperationRecordBusiness { get; set; }

        IAuthoritiesBusiness AuthoritiesBusiness { get; set; }

        #endregion

        #region 公共

        #region 基础功能

        public List<List> GetList(PaginationDTO pagination)
        {
            var entityList = Repository.Select
                                    .GetPagination(pagination)
                                    .ToList<System_Resources, List>(typeof(List).GetNamesWithTagAndOther(true, "_List"));

            var result = Mapper.Map<List<List>>(entityList);

            return result;
        }

        public List<SelectOption> DropdownList(string condition, PaginationDTO pagination)
        {
            var fields = new[] {
                nameof(System_Resources.Name),
                nameof(System_Resources.Code)
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

            var type = typeof(System_Resources);

            var select = SelectExtension.Select<System_Resources, SelectOption>(a => new SelectOption
            {
                text = a.Name,
                value = a.Id,
                search = $"{a.Name} {a.Code}",
                options = new List<OptionInfo>
                {
                    new OptionInfo
                    {
                        display = type.GetDescription(nameof(a.Name)),
                        value = a.Name
                    },
                    new OptionInfo
                    {
                        display =type.GetDescription(nameof(a.Code)),
                        value = a.Code
                    },
                    new OptionInfo
                    {
                        display = type.GetDescription(nameof(a.CreateTime)),
                        value = a.CreateTime
                    }
                }
            });

            var list = from a in Orm.Select<System_Resources>()
                            .Where(where_sql)
                            .GetPagination(pagination)
                            .ToList<System_Resources, List>(typeof(List).GetNamesWithTagAndOther(true, "_List"))
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
            var newData = Mapper.Map<System_Resources>(data).InitEntity();

            newData.Uri = newData.Uri?.ToLower();

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                Repository.Insert(newData);

                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(System_Resources),
                    DataId = newData.Id,
                    Explain = $"创建资源[编号 {newData.Code}, 类型 {newData.Type}]."
                });
            });

            if (!success)
                throw new ApplicationException("创建资源失败", ex);
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
            var editData = Mapper.Map<System_Resources>(data).ModifyEntity();

            editData.Uri = editData.Uri?.ToLower();

            var entity = Repository.GetAndCheckNull(editData.Id);

            var changed_ = string.Join(",",
                                       entity.GetPropertyValueChangeds<System_Resources, Edit>(editData)
                                            .Select(p => $"\r\n\t {p.Description}：{p.FormerValue} 更改为 {p.CurrentValue}"));

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(System_Resources),
                    DataId = entity.Id,
                    Explain = $"修改资源[编号 {entity.Code}, 类型 {entity.Type}].",
                    Remark = $"变更详情: \r\n\t{changed_}"
                });

                if (Repository.UpdateDiy
                      .SetSource(editData.ModifyEntity())
                      .UpdateColumns(typeof(Edit).GetNamesWithTagAndOther(false, "_Edit").ToArray())
                      .ExecuteAffrows() <= 0)
                    throw new ApplicationException("修改资源失败");
            });

            if (!success)
                throw ex;
        }

        [AdministratorOnly]
        public void Delete(List<string> ids)
        {
            var entityList = Repository.Select.Where(c => ids.Contains(c.Id)).ToList(c => new { c.Id, c.Name, c.Code, c.Type });

            var orList = new List<Common_OperationRecord>();

            foreach (var entity in entityList)
            {
                orList.Add(new Common_OperationRecord
                {
                    DataType = nameof(System_Resources),
                    DataId = entity.Id,
                    Explain = $"删除资源[编号 {entity.Code}, 类型 {entity.Type}]."
                });
            }

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                AuthoritiesBusiness.RevocationResourcesForAll(ids, false);

                if (Repository.Delete(o => ids.Contains(o.Id)) <= 0)
                    throw new ApplicationException("未删除任何数据");

                var orIds = OperationRecordBusiness.Create(orList);
            });

            if (!success)
                throw new ApplicationException("删除资源失败", ex);
        }

        #endregion

        #region 拓展功能

        [AdministratorOnly]
        public void Enable(string id, bool enable)
        {
            var entity = Repository.GetAndCheckNull(id);

            entity.Enable = enable;

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(System_Resources),
                    DataId = entity.Id,
                    Explain = $"{(enable ? "启用" : "禁用")}资源[编号 {entity.Code}, 类型 {entity.Type}]."
                });

                if (Repository.Update(entity) <= 0)
                    throw new ApplicationException($"{(enable ? "启用" : "禁用")}资源失败");
            });

            if (!success)
                throw ex;
        }

        #endregion

        #endregion
    }
}
