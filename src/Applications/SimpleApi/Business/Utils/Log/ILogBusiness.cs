using Model.System.LogDTO;
using Model.Utils.Log.LogDTO;
using Model.Utils.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Utils.Log
{
    /// <summary>
    /// 日志业务接口类
    /// </summary>
    public interface ILogBusiness
    {
        /// <summary>
        /// 获取默认的日志组件类型
        /// </summary>
        /// <returns></returns>
        string GetDefaultType();

        /// <summary>
        /// 获取日志文件列表
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        List<FileInfo> GetFileList(DateTime start, DateTime end);

        /// <summary>
        /// 获取日志文件内容
        /// </summary>
        /// <param name="filename">文件名</param>
        Task GetFileContent(string filename);

        /// <summary>
        /// 获取ES数据列表
        /// </summary>
        /// <param name="pagination">排序、筛选以及数据量设置</param>
        /// <returns></returns>
        List<List> GetESList(PaginationDTO pagination);

        /// <summary>
        /// 获取ES数据详情
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        Detail GetESDetail(string id);

        /// <summary>
        /// 获取数据库数据列表
        /// </summary>
        /// <param name="pagination">分页设置</param>
        /// <returns></returns>
        List<List> GetDBList(PaginationDTO pagination);

        /// <summary>
        /// 获取数据库数据详情
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        Detail GetDBDetail(string id);
    }
}
