using Integrate_Entity.Base_Manage;
using Library.Models;
using System.Collections.Generic;

namespace Integrate_Business.Base_Manage
{
    public interface IBase_DbLinkBusiness
    {
        List<Base_DbLink> GetDataList(Pagination pagination);
        Base_DbLink GetTheData(string id);
        AjaxResult AddData(Base_DbLink newData);
        AjaxResult UpdateData(Base_DbLink theData);
        AjaxResult DeleteData(List<string> ids);
    }
}