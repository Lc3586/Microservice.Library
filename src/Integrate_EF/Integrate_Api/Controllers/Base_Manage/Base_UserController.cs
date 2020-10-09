using Microsoft.AspNetCore.Mvc;
using Library.Extension;
using Library.Models;
using System.Collections.Generic;
using Integrate_Business.Base_Manage;
using Integrate_Entity.Base_Manage;
using Integrate_Business.Util;
using Library.SelectOption;

namespace Integrate_Api.Controllers.Base_Manage
{
    /// <seealso cref="Integrate_Api.BaseApiController" />
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_UserController : BaseApiController
    {
        #region DI

        public Base_UserController(IBase_UserBusiness userBus)
        {
            _userBus = userBus;
        }

        IBase_UserBusiness _userBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public ActionResult<AjaxResult<List<Base_UserDTO>>> GetDataList(Pagination pagination, string keyword)
        {
            var dataList = _userBus.GetDataList(pagination, false, null, keyword);

            return Content(pagination.BuildResult(dataList).ToJson());
        }

        [HttpPost]
        public ActionResult<AjaxResult<Base_UserDTO>> GetTheData(string id)
        {
            var theData = _userBus.GetTheData(id) ?? new Base_UserDTO();

            return Success(theData);
        }

        [HttpPost]
        public ActionResult<AjaxResult<List<SelectOption>>> GetOptionList(string selectedValueJson, string q)
        {
            var list = _userBus.GetOptionList(selectedValueJson, q);

            return Success(list);
        }

        #endregion

        #region 提交

        [HttpPost]
        public ActionResult<AjaxResult> SaveData(Base_User theData, string newPwd, string roleIdsJson)
        {
            AjaxResult res;
            if (!newPwd.IsNullOrEmpty())
                theData.Password = newPwd.ToMD5String();
            var roleIds = roleIdsJson?.ToList<string>() ?? new List<string>();
            if (theData.Id.IsNullOrEmpty())
            {
                theData.InitEntity();

                res = _userBus.AddData(theData, roleIds);
            }
            else
            {
                res = _userBus.UpdateData(theData, roleIds);
            }

            return JsonContent(res.ToJson());
        }

        [HttpPost]
        public ActionResult<AjaxResult> DeleteData(string ids)
        {
            var res = _userBus.DeleteData(ids.ToList<string>());

            return JsonContent(res.ToJson());
        }

        #endregion
    }
}