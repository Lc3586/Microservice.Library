using Microsoft.AspNetCore.Mvc;
using System;
using Library.Extention;
using Library.SelectOption;
using Library.Models;
using System.Collections.Generic;
using Integrate_Business.Base_Manage;
using Integrate_Entity.Base_Manage;

namespace Integrate_Api.Controllers.Base_Manage
{
    /// <summary>
    /// Ӧ����Կ
    /// </summary>
    /// <seealso cref="Integrate_Api.BaseApiController" />
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_LogController : BaseApiController
    {
        #region DI

        public Base_LogController(IBase_LogBusiness logBus)
        {
            _logBus = logBus;
        }

        IBase_LogBusiness _logBus { get; }

        #endregion

        #region ��ȡ

        [HttpPost]
        public ActionResult<AjaxResult<List<Base_Log>>> GetLogList(
            Pagination pagination,
            string logContent,
            string logType,
            string level,
            string opUserName,
            DateTime? startTime,
            DateTime? endTime)
        {
            pagination.SortField = "CreateTime";
            pagination.SortType = "desc";
            var list = _logBus.GetLogList(pagination, logContent, logType, level, opUserName, startTime, endTime);

            return JsonContent(pagination.BuildResult(list).ToJson());
        }

        [HttpPost]
        public ActionResult GetLogTypeList()
        {
            var list = typeof(LogType).ToOptionList();

            return Success(list);
        }

        [HttpPost]
        public ActionResult GetLoglevelList()
        {
            var list = typeof(LogLevel).ToOptionList();

            return Success(list);
        }

        #endregion

        #region �ύ

        #endregion
    }
}