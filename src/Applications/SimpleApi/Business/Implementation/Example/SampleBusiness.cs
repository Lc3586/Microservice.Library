using AutoMapper;
using Business.Filter;
using Business.Interface.Common;
using Business.Interface.Example;
using Business.Utils;
using Business.Utils.Pagination;
using Entity.Common;
using Entity.Example;
using FreeSql;
using Microservice.Library.DataMapping.Gen;
using Microservice.Library.Extension;
using Microservice.Library.FreeSql.Extention;
using Microservice.Library.FreeSql.Gen;
using Microservice.Library.OpenApi.Extention;
using Model.Example.DBDTO;
using Model.Utils.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Implementation.Example
{
    /// <summary>
    /// 操作记录业务类
    /// </summary>
    public class SampleBusiness : BaseBusiness, ISampleBusiness
    {
        #region DI

        public SampleBusiness(
            IFreeSqlProvider freeSqlProvider,
            IAutoMapperProvider autoMapperProvider,
            IOperationRecordBusiness operationRecordBusiness)
        {
            Mapper = autoMapperProvider.GetMapper();
            Orm = freeSqlProvider.GetFreeSql();
            Repository = Orm.GetRepository<Sample_DB, string>();
            OperationRecordBusiness = operationRecordBusiness;
        }

        #endregion

        #region 私有成员

        readonly IMapper Mapper;

        readonly IFreeSql Orm;

        readonly IBaseRepository<Sample_DB, string> Repository;

        readonly IOperationRecordBusiness OperationRecordBusiness;

        #endregion

        #region 外部接口

        #region 基础功能

        public List<List> GetList(PaginationDTO pagination)
        {
            var entityList = Repository.Select
                                    .GetPagination(pagination)
                                    .ToList<Sample_DB, List>(typeof(List).GetNamesWithTagAndOther(true, "_List"));

            var result = Mapper.Map<List<List>>(entityList);

            return result;
        }

        public Detail GetDetail(string id)
        {
            var entity = Repository.GetAndCheckNull(id);

            var result = Mapper.Map<Detail>(entity);

            return result;
        }

        public void Create(Create data)
        {
            var newData = Mapper.Map<Sample_DB>(data).InitEntity();

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                Repository.Insert(newData);

                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(Sample_DB),
                    DataId = newData.Id,
                    Explain = $"创建示例数据[名称 {newData.Name}]."
                });
            });

            if (!success)
                throw new ApplicationException("创建示例数据失败", ex);
        }

        public Edit GetEdit(string id)
        {
            var entity = Repository.GetAndCheckNull(id);

            var result = Mapper.Map<Edit>(entity);

            return result;
        }

        public void Edit(Edit data)
        {
            var editData = Mapper.Map<Sample_DB>(data).ModifyEntity();

            var entity = Repository.GetAndCheckNull(editData.Id);

            var changed_ = string.Join(",",
                                       entity.GetPropertyValueChangeds<Sample_DB, Edit>(editData)
                                            .Select(p => $"\r\n\t {p.Description}：{p.FormerValue} 更改为 {p.CurrentValue}"));

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(Sample_DB),
                    DataId = entity.Id,
                    Explain = $"修改示例数据[名称 {entity.Name}].",
                    Remark = $"变更详情: \r\n\t{changed_}"
                });

                if (Repository.UpdateDiy
                      .SetSource(editData.ModifyEntity())
                      .UpdateColumns(typeof(Edit).GetNamesWithTagAndOther(false, "_Edit").ToArray())
                      .ExecuteAffrows() <= 0)
                    throw new ApplicationException("修改示例数据失败");
            });

            if (!success)
                throw ex;
        }

        [AdministratorOnly]
        public void Delete(List<string> ids)
        {
            var entityList = Repository.Select.Where(c => ids.Contains(c.Id)).ToList(c => new { c.Id, c.Name });

            var orList = new List<Common_OperationRecord>();

            foreach (var entity in entityList)
            {
                orList.Add(new Common_OperationRecord
                {
                    DataType = nameof(Sample_DB),
                    DataId = entity.Id,
                    Explain = $"删除示例数据[名称 {entity.Name}]."
                });
            }

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                if (Repository.Delete(o => ids.Contains(o.Id)) <= 0)
                    throw new ApplicationException("未删除任何数据");

                var orIds = OperationRecordBusiness.Create(orList);
            });

            if (!success)
                throw new ApplicationException("删除示例数据失败", ex);
        }

        #endregion

        #region 拓展功能

        public void Enable(string id, bool enable)
        {
            var entity = Repository.GetAndCheckNull(id);

            entity.Enable = enable;

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(Sample_DB),
                    DataId = entity.Id,
                    Explain = $"{(enable ? "启用" : "禁用")}示例数据[名称 {entity.Name}]."
                });

                if (Repository.Update(entity) <= 0)
                    throw new ApplicationException($"{(enable ? "启用" : "禁用")}示例数据失败");
            });

            if (!success)
                throw ex;
        }

        #endregion

        #endregion
    }
}
