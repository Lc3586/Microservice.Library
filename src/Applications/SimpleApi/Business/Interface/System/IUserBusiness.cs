﻿using Library.Container;
using Library.Models;
using Library.SelectOption;
using Model.System.UserDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Interface.System
{
    /// <summary>
    /// 系统用户业务接口类
    /// </summary>
    public interface IUserBusiness
    {
        #region 基础功能

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="pagination">分页设置</param>
        /// <returns></returns>
        List<List> GetList(Pagination pagination);

        /// <summary>
        /// 获取下拉框数据
        /// </summary>
        /// <param name="condition">关键词(多个用空格分隔)</param>
        /// <param name="pagination">分页设置</param>
        /// <returns></returns>
        List<SelectOption> DropdownList(string condition, Pagination pagination);

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
        AjaxResult Create(Create data);

        /// <summary>
        /// 获取编辑数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AjaxResult<Edit> GetEdit(string id);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        AjaxResult Edit(Edit data);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns></returns>
        AjaxResult Delete(List<string> ids);

        #endregion

        #region 拓展功能



        #endregion
    }
}
