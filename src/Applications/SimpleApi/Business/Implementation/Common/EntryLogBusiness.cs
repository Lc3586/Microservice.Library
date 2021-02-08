using AutoMapper;
using Business.Interface.Common;
using Business.Utils;
using Business.Utils.Pagination;
using Entity.Common;
using FreeSql;
using Library.DataMapping.Gen;
using Library.FreeSql.Extention;
using Library.FreeSql.Gen;
using Library.OpenApi.Extention;
using Model.Common.EntryLogDTO;
using Model.System.Pagination;
using System.Collections.Generic;

namespace Business.Implementation.Common
{
    /// <summary>
    /// 档案操作记录业务类
    /// </summary>
    public class EntryLogBusiness : BaseBusiness, IEntryLogBusiness
    {
        #region DI

        public EntryLogBusiness(
            IFreeSqlProvider freeSqlProvider,
            IAutoMapperProvider autoMapperProvider)
        {
            Mapper = autoMapperProvider.GetMapper();
            Orm = freeSqlProvider.GetFreeSql();
            Repository = Orm.GetRepository<Common_EntryLog, long>();
        }

        #endregion

        #region 私有成员

        IMapper Mapper { get; set; }

        IFreeSql Orm { get; set; }

        IBaseRepository<Common_EntryLog, long> Repository { get; set; }

        #endregion

        #region 外部接口

        public List<List> GetList(PaginationDTO pagination)
        {
            var entityList = Orm.Select<Common_EntryLog>()
                                .GetPagination(pagination)
                                .ToList<Common_EntryLog, List>(typeof(List).GetNamesWithTagAndOther(true, "_List"));

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

        public string Create(Common_EntryLog data)
        {
            data.InitEntityWithoutOP();

            Repository.Insert(data);

            return data.Id;
        }

        #endregion
    }
}
