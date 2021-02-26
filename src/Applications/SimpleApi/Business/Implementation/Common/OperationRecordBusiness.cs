using AutoMapper;
using Business.Interface.Common;
using Business.Utils;
using Business.Utils.Pagination;
using Entity.Common;
using FreeSql;
using Microservice.Library.DataMapping.Gen;
using Microservice.Library.Extension;
using Microservice.Library.FreeSql.Extention;
using Microservice.Library.FreeSql.Gen;
using Microservice.Library.OpenApi.Extention;
using Model.Common.OperationRecordDTO;
using Model.Utils.Pagination;
using System.Collections.Generic;
using System.Linq;

namespace Business.Implementation.Common
{
    /// <summary>
    /// 操作记录业务类
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
            Repository = Orm.GetRepository<Common_OperationRecord, long>();
        }

        #endregion

        #region 私有成员

        IMapper Mapper { get; set; }

        IFreeSql Orm { get; set; }

        IBaseRepository<Common_OperationRecord, long> Repository { get; set; }

        #endregion

        #region 外部接口

        public List<List> GetList(PaginationDTO pagination)
        {
            var entityList = Orm.Select<Common_OperationRecord>()
                                .GetPagination(pagination)
                                .ToList<Common_OperationRecord, List>(typeof(List).GetNamesWithTagAndOther(true, "_List"));

            var result = Mapper.Map<List<List>>(entityList);

            return result;
        }

        public Detail GetDetail(string id)
        {
            var entity = Repository.Select
                                .Include(o => o.User)
                                .Where(o => o.Id == id)
                                .GetAndCheckNull();

            var result = Mapper.Map<Detail>(entity);

            if (entity.User != null)
                result._User = Mapper.Map<Model.System.UserDTO.Detail>(entity.User);

            return result;
        }

        public string Create(Common_OperationRecord data)
        {
            if (Operator.IsAuthenticated)
            {
                var operatorDetail = Operator.UserInfo;
                data.InitEntity(operatorDetail);
                data.Account = operatorDetail.Account;
                data.IsAdmin = Operator.IsAdmin;
            }
            else
                data.InitEntityWithoutOP();

            Repository.Insert(data);

            return data.Id;
        }

        public List<string> Create(List<Common_OperationRecord> datas)
        {
            if (Operator.IsAuthenticated)
            {
                var isAdmin = Operator.IsAdmin;
                var operatorDetail = Operator.UserInfo;
                datas.ForEach(o =>
                {
                    o.InitEntity(operatorDetail);

                    o.Account = operatorDetail.Account;
                    o.IsAdmin = isAdmin;
                });
            }
            else
                datas.ForEach(o => o.InitEntityWithoutOP());

            Repository.Insert(datas);

            return datas.Select(o => o.Id).ToList();
        }

        #endregion
    }
}
