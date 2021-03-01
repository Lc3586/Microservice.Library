﻿using AutoMapper;
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
using Model.Common.EntryLogDTO;
using Model.System;
using Model.Utils.Pagination;
using System;
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
            Repository = Orm.GetRepository<Common_EntryLog, string>();
        }

        #endregion

        #region 私有成员

        readonly IMapper Mapper;

        readonly IFreeSql Orm;

        readonly IBaseRepository<Common_EntryLog, string> Repository;

        #endregion

        #region 外部接口

        public List<List> GetList(PaginationDTO pagination)
        {
            var entityList = Orm.Select<Common_EntryLog>()
                                .Where(o => Operator.IsAdmin == true || o.CreatorId == Operator.AuthenticationInfo.Id)
                                .GetPagination(pagination)
                                .ToList<Common_EntryLog, List>(typeof(List).GetNamesWithTagAndOther(true, "_List"));

            var result = Mapper.Map<List<List>>(entityList);

            return result;
        }

        public Detail GetDetail(string id)
        {
            var entity = Repository.Where(o => o.Id == id)
                .Include(o => o.User)
                .Include(o => o.Member)
                .GetAndCheckNull();

            if (!Operator.IsAdmin && entity.CreatorId != Operator.AuthenticationInfo.Id)
                throw new ApplicationException("没有权限.");

            var result = Mapper.Map<Detail>(entity);

            if (entity.UserType == UserType.系统用户)
            {
                if (entity.User != null)
                    result._User = Mapper.Map<Model.System.UserDTO.Detail>(entity.User);
            }
            else if (entity.UserType == UserType.会员)
            {
                if (entity.Member != null)
                    result._Member = Mapper.Map<Model.Public.MemberDTO.Detail>(entity.Member);
            }

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
