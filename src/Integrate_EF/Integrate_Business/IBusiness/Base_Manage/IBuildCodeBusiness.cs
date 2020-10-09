using Integrate_Entity.Base_Manage;
using Library.Models;
using System.Collections.Generic;

namespace Integrate_Business.Base_Manage
{
    public interface IBuildCodeBusiness
    {
        List<Base_DbLink> GetAllDbLink();

        List<DbTableInfo> GetDbTableList(string linkId);

        void Build(string linkId, string areaName, List<string> tables, List<int> buildTypes);
    }
}
