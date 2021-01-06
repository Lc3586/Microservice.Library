using Library.Container;
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

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conStr">连接名或连接字符串</param>
        public BaseBusiness(string conStr)
        {
            _conString = conStr;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conStr">连接名或连接字符串</param>
        /// <param name="entityAssembly">实体类命名空间</param>
        public BaseBusiness(string conStr, string entityAssembly)
        {
            _conString = conStr;
            _entityAssembly = entityAssembly;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conStr">连接名或连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        public BaseBusiness(string conStr, DatabaseType dbType)
        {
            _conString = conStr;
            _dbType = dbType;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conStr">连接名或连接字符串</param>
        /// <param name="entityAssembly">实体类命名空间</param>
        /// <param name="dbType">数据库类型</param>
        public BaseBusiness(string conStr, string entityAssembly, DatabaseType dbType)
        {
            _conString = conStr;
            _entityAssembly = entityAssembly;
            _dbType = dbType;
        }

        #endregion

        #region 私有成员

        private string _conString { get; }
        private string _entityAssembly { get; }
        private DatabaseType? _dbType { get; }

        private object _serviceLock = new object();
        protected virtual string _valueField { get; } = "Id";
        protected virtual string _textField { get => throw new Exception("请在子类重写"); }

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

        /// <summary>
        /// 构建前端Select远程搜索数据
        /// </summary>
        /// <param name="selectedValueJson">已选择的项，JSON数组</param>
        /// <param name="q">查询关键字</param>
        /// <returns></returns>
        public List<SelectOption> GetOptionList(string selectedValueJson, string q)
        {
            return GetOptionList(selectedValueJson, q, _textField, _valueField, null);
        }

        /// <summary>
        /// 构建前端Select远程搜索数据
        /// </summary>
        /// <param name="selectedValueJson">已选择的项，JSON数组</param>
        /// <param name="q">查询关键字</param>
        /// <param name="textFiled">文本字段</param>
        /// <param name="valueField">值字段</param>
        /// <param name="source">指定数据源</param>
        /// <returns></returns>
        public List<SelectOption> GetOptionList(string selectedValueJson, string q, string textFiled, string valueField, IQueryable<T> source = null)
        {
            throw new NotImplementedException("需要改造成freesql");
            Pagination pagination = new Pagination
            {
                PageRows = 10
            };

            List<T> selectedList = new List<T>();
            string where = " 1=1 ";
            List<string> ids = selectedValueJson?.ToList<string>() ?? new List<string>();
            if (ids.Count > 0)
            {
                selectedList = GetNewQ().Where($"@0.Contains({valueField})", ids).ToList();

                where += $" && !@0.Contains({valueField})";
            }

            if (!q.IsNullOrEmpty())
            {
                where += $" && it.{textFiled}.Contains(@1)";
            }
            List<T> newQList = GetNewQ().Where(where, ids, q).GetPagination(pagination).ToList();

            var resList = selectedList.Concat(newQList).Select(x => new SelectOption
            {
                value = x.GetPropertyValue(valueField)?.ToString(),
                text = x.GetPropertyValue(textFiled)?.ToString()
            }).ToList();

            return resList;

            IQueryable<T> GetNewQ()
            {
                return source;
            }
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
