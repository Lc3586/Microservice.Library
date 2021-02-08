﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Utils
{
    [NonController]
    [Authorize]
    [Route("/[controller]/[action]")]
    public class TestController : BaseApiController
    {
        [HttpGet]
        public IActionResult Test()
        {
            return HtmlContent("");
        }

        /// <summary>
        /// 压力测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PressTest()
        {
            //var bus = AutofacHelper.GetScopeService<IBase_UserBusiness>();
            //var db = DbFactory.GetRepository();
            //Base_UnitTest data = new Base_UnitTest
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    UserId = Guid.NewGuid().ToString(),
            //    Age = 10,
            //    UserName = Guid.NewGuid().ToString()
            //};
            //db.Insert(data);
            //db.Update(data);
            //db.GetIQueryable<Base_UnitTest>().FirstOrDefault();
            //db.Delete(data);

            return Success("");
        }
    }
}