using AutoMapper;
using Business.Interface.System;
using Entity.System;
using FreeSql;
using Library.DataMapping.Gen;
using Library.Extension;
using Library.FreeSql.Extention;
using Library.FreeSql.Gen;
using Library.Models;
using Library.OpenApi.Annotations;
using Library.OpenApi.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Model.System.OperationRecordDTO;
using Business.Util;

namespace Business.Implementation.System
{
    /// <summary>
    /// 档案操作记录业务类
    /// </summary>
    public class OperationRecordBusiness : BaseBusiness, IOperationRecordBusiness
    {
        #region DI

        public OperationRecordBusiness(
            IFreeSqlProvider freeSqlProvider,
            IAutoMapperProvider autoMapperProvider)
        {
            Mapper = autoMapperProvider.GetMapper();
            Orm = freeSqlProvider.GetFreeSql();
            Repository = Orm.GetRepository<System_OperationRecord, long>();
        }

        #endregion

        #region 私有成员

        IMapper Mapper { get; set; }

        IFreeSql Orm { get; set; }

        IBaseRepository<System_OperationRecord, long> Repository { get; set; }

        #endregion

        #region 外部接口

        public List<List> GetList(Pagination pagination)
        {
            var entityList = Orm.Select<System_OperationRecord>()
                                .ToList<System_OperationRecord, List>(pagination, typeof(List).GetNamesWithTagAndOther(true, "_List"));

            var result = Mapper.Map<List<List>>(entityList);

            return result;
        }

        public AjaxResult<Detail> GetDetail(string id)
        {
            var entity = Repository.Select
                                .Include(o => o.User)
                                .Where(o => o.Id == id)
                                .GetAndCheckNull();

            var result = Mapper.Map<Detail>(entity);

            if (entity.User != null)
                result._User = Mapper.Map<Model.System.UserDTO.Detail>(entity.User);

            return Success(result);
        }

        public string Create(System_OperationRecord data, bool withOP = true)
        {
            if (withOP)
            {
                var currentUser = Operator.Property;
                data.InitEntity(currentUser);

                data.Account = currentUser.Account;
                data.UserType = currentUser.Type;
                data.IsAdmin = Operator.IsAdmin;
            }
            else
                data.InitEntityWithoutOP();

            Repository.Insert(data);

            return data.Id;
        }

        public List<string> Create(List<System_OperationRecord> datas)
        {
            datas.ForEach(o => o.InitEntity());

            Repository.Insert(datas);

            return datas.Select(o => o.Id).ToList();
        }

        #endregion
    }
}
