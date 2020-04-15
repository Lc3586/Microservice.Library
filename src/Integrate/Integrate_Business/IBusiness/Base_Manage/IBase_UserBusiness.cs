using Integrate_Entity.Base_Manage;
using Library.Models;
using Library.SelectOption;
using System.Collections.Generic;

namespace Integrate_Business.Base_Manage
{
    public interface IBase_UserBusiness
    {
        List<Base_UserDTO> GetDataList(Pagination pagination, bool all, string userId = null, string keyword = null);
        List<SelectOption> GetOptionList(string selectedValueJson, string q);
        UserDTO GetTheData(string id);
        AjaxResult AddData(Base_User newData, List<string> roleIds);
        AjaxResult UpdateData(Base_User theData, List<string> roleIds);
        AjaxResult DeleteData(List<string> ids);
    }
}