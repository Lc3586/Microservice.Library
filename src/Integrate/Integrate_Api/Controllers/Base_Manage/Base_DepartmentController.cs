using Microsoft.AspNetCore.Mvc;
using Library.Extention;
using Library.Models;
using System.Collections.Generic;
using Integrate_Business.Base_Manage;
using Integrate_Entity.Base_Manage;
using Integrate_Business.Util;

namespace Integrate_Api.Controllers.Base_Manage
{
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_DepartmentController : BaseApiController
    {
        #region DI

        public Base_DepartmentController(IBase_DepartmentBusiness departmentBus)
        {
            _departmentBus = departmentBus;
        }

        IBase_DepartmentBusiness _departmentBus { get; }

        #endregion

        #region ��ȡ

        [HttpPost]
        public ActionResult<AjaxResult<Base_Department>> GetTheData(string id)
        {
            var theData = _departmentBus.GetTheData(id) ?? new Base_Department();

            return Success(theData);
        }

        [HttpPost]
        public ActionResult<AjaxResult<List<Base_DepartmentTreeDTO>>> GetTreeDataList(string parentId)
        {
            var dataList = _departmentBus.GetTreeDataList(parentId);

            return Success(dataList);
        }

        #endregion

        #region �ύ

        [HttpPost]
        public ActionResult<AjaxResult> SaveData(Base_Department theData)
        {
            AjaxResult res;
            if (theData.Id.IsNullOrEmpty())
            {
                theData.InitEntity();

                res = _departmentBus.AddData(theData);
            }
            else
            {
                res = _departmentBus.UpdateData(theData);
            }

            return JsonContent(res.ToJson());
        }

        [HttpPost]
        public ActionResult<AjaxResult> DeleteData(string ids)
        {
            var res = _departmentBus.DeleteData(ids.ToList<string>());

            return JsonContent(res.ToJson());
        }

        #endregion
    }
}