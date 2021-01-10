﻿using Library.Container;
using Library.Extension;
using Library.Models;
using Library.Log;
using Library.SelectOption;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using AutoMapper;
using Business.Interface.System;

namespace Business.Util
{
    /// <summary>
    /// 描述：业务处理基类
    /// 作者：Coldairarrow
    /// </summary>
    /// <typeparam name="T">泛型约束（数据库实体）</typeparam>
    public class BaseBusiness : IDependency
    {
        #region DI

        /// <summary>
        /// 日志组件
        /// </summary>
        public ILogger Logger { get => AutofacHelper.GetScopeService<ILogger>(); }

        /// <summary>
        /// 当前登录人
        /// </summary>
        public IOperator Operator { get => AutofacHelper.GetScopeService<IOperator>(); }

        #endregion

        #region 构造函数

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public BaseBusiness()
        {

        }

        #endregion

        #region 私有成员



        #endregion

        #region 外部属性


        #endregion

        #region 事物提交



        #endregion

        #region 增加数据



        #endregion

        #region 删除数据



        #endregion

        #region 更新数据



        #endregion

        #region 查询数据

        /// <summary>
        /// 获取分页后的数据
        /// </summary>
        /// <typeparam name="U">实体类型</typeparam>
        /// <param name="query">数据源IQueryable</param>
        /// <param name="pagination">分页参数</param>
        /// <returns></returns>
        public List<U> GetPagination<U>(IQueryable<U> query, Pagination pagination)
        {
            return query.GetPagination(pagination).ToList();
        }

        /// <summary>
        /// 获取分页后的数据
        /// </summary>
        /// <typeparam name="U">实体参数</typeparam>
        /// <param name="query">IQueryable数据源</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageRows">每页行数</param>
        /// <param name="orderColumn">排序列</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="count">总记录数</param>
        /// <param name="pages">总页数</param>
        /// <returns></returns>
        public List<U> GetPagination<U>(IQueryable<U> query, int pageIndex, int pageRows, string orderColumn, SortType orderType, ref int count, ref int pages)
        {
            Pagination pagination = new Pagination
            {
                PageIndex = pageIndex,
                PageRows = pageRows,
                SortType = orderType,
                SortField = orderColumn
            };
            pagination.RecordCount = count = query.Count();
            pages = (int)pagination.PageCount;

            return query.GetPagination(pagination).ToList();
        }

        #endregion

        #region 执行Sql语句



        #endregion

        #region 业务返回

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns></returns>
        public AjaxResult Success()
        {
            AjaxResult res = new AjaxResult
            {
                Success = true,
                Msg = "操作成功！",
            };

            return res;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
        public AjaxResult<U> Success<U>(U data)
        {
            AjaxResult<U> res = new AjaxResult<U>
            {
                Success = true,
                Msg = "操作成功",
                Data = data
            };

            return res;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="data">返回数据</param>
        /// <param name="msg">返回消息</param>
        /// <returns></returns>
        public AjaxResult<U> Success<U>(U data, string msg)
        {
            AjaxResult<U> res = new AjaxResult<U>
            {
                Success = true,
                Msg = msg,
                Data = data
            };

            return res;
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <returns></returns>
        public AjaxResult Error()
        {
            AjaxResult res = new AjaxResult
            {
                Success = false,
                Msg = "操作失败！",
            };

            return res;
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="msg">错误提示</param>
        /// <returns></returns>
        public AjaxResult Error(string msg)
        {
            AjaxResult res = new AjaxResult
            {
                Success = false,
                Msg = msg,
            };

            return res;
        }

        #endregion

        #region 其它操作

        #endregion

        #region Dispose

        /// <summary>
        /// 执行与释放或重置非托管资源关联的应用程序定义的任务。
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual void Dispose()
        {

        }

        #endregion
    }
}