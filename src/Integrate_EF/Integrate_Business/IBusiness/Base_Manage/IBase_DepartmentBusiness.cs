using Integrate_Entity.Base_Manage;
using Library.Models;
using System.Collections.Generic;

namespace Integrate_Business.Base_Manage
{
    public interface IBase_DepartmentBusiness
    {
        List<Base_DepartmentTreeDTO> GetTreeDataList(string parentId = null);
        Base_Department GetTheData(string id);
        List<string> GetChildrenIds(string departmentId);
        AjaxResult AddData(Base_Department newData);
        AjaxResult UpdateData(Base_Department theData);
        AjaxResult DeleteData(List<string> ids);
    }
}