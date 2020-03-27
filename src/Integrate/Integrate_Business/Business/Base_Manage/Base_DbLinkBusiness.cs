using Integrate_Entity.Base_Manage;
using Library.Container;
using Library.Extention;
using Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace Integrate_Business.Base_Manage
{
    public class Base_DbLinkBusiness : BaseBusiness<Base_DbLink>, IBase_DbLinkBusiness, IDependency
    {
        #region �ⲿ�ӿ�

        public List<Base_DbLink> GetDataList(Pagination pagination)
        {
            return GetIQueryable().GetPagination(pagination).ToList();
        }

        /// <summary>
        /// ��ȡָ���ĵ�������
        /// </summary>
        /// <param name="id">����</param>
        /// <returns></returns>
        public Base_DbLink GetTheData(string id)
        {
            return GetEntity(id);
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="newData">����</param>
        public AjaxResult AddData(Base_DbLink newData)
        {
            Insert(newData);

            return Success();
        }

        /// <summary>
        /// ��������
        /// </summary>
        public AjaxResult UpdateData(Base_DbLink theData)
        {
            Update(theData);

            return Success();
        }

        public AjaxResult DeleteData(List<string> ids)
        {
            Delete(ids);

            return Success();
        }

        #endregion

        #region ˽�г�Ա

        #endregion

        #region ����ģ��

        #endregion
    }
}