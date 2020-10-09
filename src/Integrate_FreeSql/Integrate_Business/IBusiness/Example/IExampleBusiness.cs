using Integrate_Model.Example;
using Library.Models;
using System.Collections.Generic;

namespace Integrate_Business.Example
{
    /// <summary>
    /// ʾ��ҵ����ӿ�
    /// </summary>
    public interface IExampleBusiness
    {
        /// <summary>
        /// ��ȡ�б�����
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <returns></returns>
        List<ExampleDTO.List> GetList(Pagination pagination);

        /// <summary>
        /// ��ȡ�༭����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ExampleDTO.Edit GetEdit(string id);

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ExampleDTO.Detail GetDetail(string id);

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="data">����</param>
        /// <returns></returns>
        AjaxResult Create(ExampleDTO.Create data);

        /// <summary>
        /// �༭
        /// </summary>
        /// <param name="data">����</param>
        /// <returns></returns>
        AjaxResult Edit(ExampleDTO.Edit data);

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="ids">Id����</param>
        /// <returns></returns>
        AjaxResult Delete(List<string> ids);
    }
}