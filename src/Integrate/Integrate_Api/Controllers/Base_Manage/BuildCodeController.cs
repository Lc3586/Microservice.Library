using Integrate_Business.Base_Manage;
using Integrate_Entity.Base_Manage;
using Library.Extention;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Integrate_Api.Controllers.Base_Manage
{
    [Route("/Base_Manage/[controller]/[action]")]
    public class BuildCodeController : BaseApiController
    {
        #region DI

        public BuildCodeController(IBuildCodeBusiness buildCodeBus)
        {
            _buildCodeBus = buildCodeBus;
        }

        IBuildCodeBusiness _buildCodeBus { get; }

        #endregion

        [HttpPost]
        public ActionResult<AjaxResult<List<Base_DbLink>>> GetAllDbLink()
        {
            var list = _buildCodeBus.GetAllDbLink();

            return Success(list);
        }

        [HttpPost]
        public ActionResult<AjaxResult<List<DbTableInfo>>> GetDbTableList(string linkId)
        {
            var list = _buildCodeBus.GetDbTableList(linkId);

            return JsonContent(new Pagination().BuildResult(list).ToJson());
        }

        [HttpPost]
        public IActionResult Build(string linkId, string areaName, string tablesJson, string buildTypesJson)
        {
            _buildCodeBus.Build(linkId, areaName, tablesJson?.ToList<string>(), buildTypesJson?.ToList<int>());

            return Success();
        }
    }
}