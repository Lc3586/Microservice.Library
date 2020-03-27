using Microsoft.AspNetCore.Mvc;
using System;
using Library.Extention;
using Library.Models;
using System.Collections.Generic;
using Integrate_Business.Base_Manage;
using Integrate_Entity.Base_Manage;
using Integrate_Business.Util;

namespace Integrate_Api.Controllers.Base_Manage
{
    /// <summary>
    /// Ӧ����Կ
    /// </summary>
    /// <seealso cref="Integrate_Api.BaseApiController" />
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_RoleController : BaseApiController
    {
        #region DI

        public Base_RoleController(IBase_RoleBusiness roleBus)
        {
            _roleBus = roleBus;
        }

        IBase_RoleBusiness _roleBus { get; }

        #endregion

        #region ��ȡ

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="roleName">��ɫ��</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<AjaxResult<List<Base_RoleDTO>>> GetDataList(Pagination pagination, string roleName)
        {
            var dataList = _roleBus.GetDataList(pagination, null, roleName);

            return Content(pagination.BuildResult(dataList).ToJson());
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="id">id����</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<AjaxResult<Base_RoleDTO>> GetTheData(string id)
        {
            var theData = _roleBus.GetTheData(id) ?? new Base_RoleDTO();

            return Success(theData);
        }

        #endregion

        #region �ύ

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="theData">���������</param>
        /// <param name="actionsJson">Ȩ��ֵJSON</param>
        [HttpPost]
        public ActionResult<AjaxResult> SaveData(Base_Role theData, string actionsJson)
        {
            AjaxResult res;
            var actionList = actionsJson?.ToList<string>();
            if (theData.Id.IsNullOrEmpty())
            {
                theData.InitEntity();

                res = _roleBus.AddData(theData, actionList);
            }
            else
            {
                res = _roleBus.UpdateData(theData, actionList);
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
            var res = _roleBus.DeleteData(ids.ToList<string>());

            return JsonContent(res.ToJson());
        }

        #endregion
    }
}