using Library.Models;
using Model.System.MenuDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Interface.System
{
    /// <summary>
    /// 角色业务接口类
    /// </summary>
    public interface IMenuBusiness
    {
        #region 基础功能

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="pagination">分页设置</param>
        /// <returns></returns>
        List<List> GetList(Pagination pagination);

        /// <summary>
        /// 获取树状列表数据
        /// </summary>
        /// <param name="paramter">参数</param>
        /// <param name="deep">处于递归中</param>
        /// <returns></returns>
        List<TreeList> GetTreeList(TreeListParamter paramter, bool deep = false);

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
        /// 排序
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        void Sort(Sort data);

        /// <summary>
        /// 拖动排序
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        void DragSort(DragSort data);

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
