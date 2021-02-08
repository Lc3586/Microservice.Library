using Entity.Common;
using Model.Common.EntryLogDTO;
using Model.System.Pagination;
using System.Collections.Generic;

namespace Business.Interface.Common
{
    /// <summary>
    /// 登录日志业务接口类
    /// </summary>
    public interface IEntryLogBusiness
    {
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="pagination">分页设置</param>
        /// <returns></returns>
        List<List> GetList(PaginationDTO pagination);

        /// <summary>
        /// 获取详情数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Detail GetDetail(string id);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>登录日志Id</returns>
        string Create(Common_EntryLog data);
    }
}
