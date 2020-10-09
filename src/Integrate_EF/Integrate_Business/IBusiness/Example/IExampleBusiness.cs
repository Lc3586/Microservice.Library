using Integrate_Model.Example;
using Library.Models;
using System.Collections.Generic;

namespace Integrate_Business.Example
{
    /// <summary>
    /// 示例业务类接口
    /// </summary>
    public interface IExampleBusiness
    {
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="pagination">分页设置</param>
        /// <returns></returns>
        List<ExampleDTO.List> GetList(Pagination pagination);

        /// <summary>
        /// 获取编辑数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ExampleDTO.Edit GetEdit(string id);

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ExampleDTO.Detail GetDetail(string id);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        AjaxResult Create(ExampleDTO.Create data);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        AjaxResult Edit(ExampleDTO.Edit data);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns></returns>
        AjaxResult Delete(List<string> ids);
    }
}