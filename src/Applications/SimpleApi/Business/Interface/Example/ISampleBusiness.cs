﻿using Model.Example.DBDTO;
using Model.Utils.Pagination;
using System.Collections.Generic;

namespace Business.Interface.Example
{
    /// <summary>
    /// 示例业务接口类
    /// </summary>
    public interface ISampleBusiness
    {
        #region 基础功能

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="pagination">分页设置</param>
        /// <returns></returns>
        List<List> GetList(PaginationDTO pagination);

        /// <summary>
        /// 获取详情数据
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        Detail GetDetail(string id);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        void Create(Create data);

        /// <summary>
        /// 获取编辑数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Edit GetEdit(string id);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        void Edit(Edit data);

        /// <summary>
        /// 启用/禁用
        /// </summary>
        /// <param name="id">数据</param>
        /// <param name="enable">设置状态</param>
        /// <returns></returns>
        void Enable(string id, bool enable);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns></returns>
        void Delete(List<string> ids);

        #endregion

        #region 拓展功能



        #endregion
    }
}
