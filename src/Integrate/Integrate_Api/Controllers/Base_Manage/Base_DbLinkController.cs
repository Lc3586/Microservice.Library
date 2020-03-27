using Integrate_Business.Base_Manage;
using Integrate_Business.Util;
using Integrate_Entity.Base_Manage;
using Library.Extention;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Integrate_Api.Controllers.Base_Manage
{
    /// <summary>
    /// Ӧ����Կ
    /// </summary>
    /// <seealso cref="Integrate_Api.BaseApiController" />
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_DbLinkController : BaseApiController
    {
        #region DI

        public Base_DbLinkController(IBase_DbLinkBusiness dbLinkBus)
        {
            _dbLinkBus = dbLinkBus;
        }

        IBase_DbLinkBusiness _dbLinkBus { get; }

        #endregion

        #region ��ȡ

        [HttpPost]
        public ActionResult<AjaxResult<List<Base_DbLink>>> GetDataList(Pagination pagination)
        {
            var dataList = _dbLinkBus.GetDataList(pagination);

            return Content(pagination.BuildResult(dataList).ToJson());
        }

        [HttpPost]
        public ActionResult<AjaxResult<Base_DbLink>> GetTheData(string id)
        {
            var theData = _dbLinkBus.GetTheData(id) ?? new Base_DbLink();

            return Success(theData);
        }

        #endregion

        #region �ύ

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="theData">���������</param>
        [HttpPost]
        public ActionResult<AjaxResult> SaveData(Base_DbLink theData)
        {
            AjaxResult res;
            if (theData.Id.IsNullOrEmpty())
            {
                theData.InitEntity();

                res = _dbLinkBus.AddData(theData);
            }
            else
            {
                res = _dbLinkBus.UpdateData(theData);
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
            var res = _dbLinkBus.DeleteData(ids.ToList<string>());

            return JsonContent(res.ToJson());
        }

        #endregion
    }
}