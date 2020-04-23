using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Library.Extension;
using Library.Models;
using Integrate_Business.Base_Manage;
using Integrate_Entity.Base_Manage;
using Integrate_Business.Util;

namespace Integrate_Api.Controllers.Base_Manage
{
    /// <summary>
    /// ϵͳȨ��
    /// </summary>
    /// <seealso cref="BaseApiController" />
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_ActionController : BaseApiController
    {
        #region DI

        public Base_ActionController(IBase_ActionBusiness actionBus)
        {
            _actionBus = actionBus;
        }

        IBase_ActionBusiness _actionBus { get; }

        #endregion

        #region ��ȡ

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="id">id����</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<AjaxResult<Base_Action>> GetTheData(string id)
        {
            var theData = _actionBus.GetTheData(id) ?? new Base_Action();

            return Success(theData);
        }

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <param name="parentId">����Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<AjaxResult<List<Base_Action>>> GetPermissionList(string parentId)
        {
            var dataList = _actionBus.GetDataList(new Pagination(), null, parentId, new List<int> { 2 });

            return Success(dataList);
        }

        [HttpPost]
        public ActionResult<AjaxResult<List<Base_Action>>> GetAllActionList()
        {
            var dataList = _actionBus.GetDataList(new Pagination(), null, null, new List<int> { 0, 1, 2 });

            return Success(dataList);
        }

        /// <summary>
        /// ��ȡ�˵����б�
        /// </summary>
        /// <param name="keyword">�ؼ���</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<AjaxResult<List<Base_ActionDTO>>> GetMenuTreeList(string keyword)
        {
            var dataList = _actionBus.GetTreeDataList(keyword, new List<int> { 0, 1 }, true);

            return Success(dataList);
        }

        /// <summary>
        /// ��ȡȫ�İ������б�
        /// </summary>
        /// <param name="keyword">�ؼ���</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<AjaxResult<List<Base_ActionDTO>>> GetActionTreeList(string keyword)
        {
            var dataList = _actionBus.GetTreeDataList(keyword, null, false);

            return Success(dataList);
        }

        #endregion

        #region �ύ

        [HttpPost]
        public ActionResult<AjaxResult> SaveData(Base_Action theData, string permissionListJson)
        {
            AjaxResult res;
            var permissionList = permissionListJson?.ToList<Base_Action>();
            if (theData.Id.IsNullOrEmpty())
            {
                theData.InitEntity();

                res = _actionBus.AddData(theData, permissionList);
            }
            else
            {
                res = _actionBus.UpdateData(theData, permissionList);
            }

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="ids">id����,JSON����</param>
        [HttpPost]
        public ActionResult<AjaxResult> DeleteData(string ids)
        {
            var res = _actionBus.DeleteData(ids.ToList<string>());

            return JsonContent(res.ToJson());
        }

        ///// <summary>
        ///// ����Ȩ��
        ///// </summary>
        ///// <returns></returns>
        ///// <param name="parentId">����Id</param>
        ///// <param name="permissionListJson">Ȩ���б�JSON����</param>
        //[HttpPost]
        //public ActionResult<AjaxResult> SavePermission(string parentId, string permissionListJson)
        //{
        //    var res = _actionBus.SavePermission(parentId, permissionListJson?.ToList<Base_Action>());

        //    return JsonContent(res.ToJson());
        //}

        #endregion
    }
}