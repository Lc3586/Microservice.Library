using Library.Models;
using Model.System.RoleDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Interface.System
{
    /// <summary>
    /// 角色业务接口类
    /// </summary>
    public interface IRoleBusiness
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
        /// 启用/禁用
        /// </summary>
        /// <param name="id">数据</param>
        /// <param name="enable">设置状态</param>
        /// <returns></returns>
        AjaxResult Enable(string id, bool enable);

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        AjaxResult Sort(Sort data);

        /// <summary>
        /// 拖动排序
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        AjaxResult DragSort(DragSort data);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns></returns>
        AjaxResult Delete(List<string> ids);

        #endregion

        #region 拓展功能

        /// <summary>
        /// 是否为管理员
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        bool IsAdmin(string id);

        /// <summary>
        /// 获取授权数据
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="includeMenu">包括授权菜单</param>
        /// <param name="includeResources">包括授权资源</param>
        /// <returns></returns>
        Base GetBase(string id, bool includeMenu, bool includeResources);

        /// <summary>
        /// 获取授权的菜单
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        List<Model.System.MenuDTO.Base> GetMenuAuthoritiesBase(string id);

        /// <summary>
        /// 获取授权的资源
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        List<Model.System.ResourcesDTO.Base> GetResourcesAuthoritiesBase(string id);

        #endregion
    }
}
