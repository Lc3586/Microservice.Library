using Library.SelectOption;
using Model.Common;
using Model.Public.MemberDTO;
using Model.System.Pagination;
using System.Collections.Generic;

namespace Business.Interface.System
{
    /// <summary>
    /// 会员业务接口类
    /// </summary>
    public interface IMemberBusiness
    {
        #region 基础功能

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="pagination">分页设置</param>
        /// <returns></returns>
        List<List> GetList(PaginationDTO pagination);

        /// <summary>
        /// 获取下拉框数据
        /// </summary>
        /// <param name="condition">关键词(多个用空格分隔)</param>
        /// <param name="pagination">分页设置</param>
        /// <returns></returns>
        List<SelectOption> DropdownList(string condition, PaginationDTO pagination);

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
        /// <param name="runTransaction">运行事务（默认运行）</param>
        /// <param name="withOP">写入操作人信息</param>
        /// <returns></returns>
        void Create(Create data, bool runTransaction = true, bool withOP = true);

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
        /// <param name="runTransaction">运行事务（默认运行）</param>
        /// <param name="withOP">写入操作人信息</param>
        /// <returns></returns>
        void Edit(Edit data, bool runTransaction = true, bool withOP = true);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns></returns>
        void Delete(List<string> ids);

        #endregion

        #region 拓展功能

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="openId"></param>
        void Login(string openId);

        /// <summary>
        /// 获取操作者详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OperatorDetail GetOperatorDetail(string id);

        #endregion
    }
}
