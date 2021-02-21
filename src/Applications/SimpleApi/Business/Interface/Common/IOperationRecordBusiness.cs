using Entity.Common;
using Model.Common.OperationRecordDTO;
using Model.Utils.Pagination;
using System.Collections.Generic;

namespace Business.Interface.Common
{
    /// <summary>
    /// 操作记录业务接口类
    /// </summary>
    public interface IOperationRecordBusiness
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
        /// <returns>操作记录Id</returns>
        string Create(Common_OperationRecord data);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="datas">数据集合</param>
        /// <returns></returns>
        List<string> Create(List<Common_OperationRecord> datas);
    }
}
