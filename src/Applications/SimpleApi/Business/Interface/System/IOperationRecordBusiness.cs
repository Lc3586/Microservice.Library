using Library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Model.System.OperationRecordDTO;
using Library.Container;
using Entity.System;

namespace Business.Interface.System
{
    /// <summary>
    /// 档案操作记录业务接口类
    /// </summary>
    public interface IOperationRecordBusiness
    {
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="pagination">分页设置</param>
        /// <returns></returns>
        List<List> GetList(Pagination pagination);

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
        /// <param name="withOP">写入操作人信息</param>
        /// <returns>操作记录Id</returns>
        string Create(System_OperationRecord data, bool withOP = true);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="datas">数据集合</param>
        /// <returns></returns>
        List<string> Create(List<System_OperationRecord> datas);
    }
}
