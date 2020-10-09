using System;
using Coldairarrow.Util;
using Integrate_Model.System;
using Library.Container;
using Library.Extension;

namespace Integrate_Business.Util
{
    public static partial class Extention
    {
        public static T InitEntity<T>(this T entity, UserDTO property = null)
        {
            var op = AutofacHelper.GetScopeService<IOperator>();

            if (entity.ContainsProperty("Id"))
                entity.SetPropertyValue("Id", IdHelper.GetLongId());
            if (entity.ContainsProperty("CreateTime"))
                entity.SetPropertyValue("CreateTime", DateTime.Now);
            if (entity.ContainsProperty("CreatorId"))
                entity.SetPropertyValue("CreatorId", property == null ? op?.UserId : property.Id);
            if (entity.ContainsProperty("CreatorName"))
                entity.SetPropertyValue("CreatorName", GetUserName(op, property));

            return entity;
        }

        public static T ModifyEntity<T>(this T entity, UserDTO property = null)
        {
            var op = AutofacHelper.GetScopeService<IOperator>();

            if (entity.ContainsProperty("Id"))
            {
                if (entity.ContainsProperty("ModifyTime"))
                    entity.SetPropertyValue("ModifyTime", DateTime.Now);
                if (entity.ContainsProperty("ModifyId"))
                    entity.SetPropertyValue("ModifyId", property == null ? op?.UserId : property.Id);
                if (entity.ContainsProperty("ModifyName"))
                    entity.SetPropertyValue("ModifyName", GetUserName(op, property));
            }
            return entity;
        }

        public static string GetUserName(IOperator op, UserDTO property = null)
        {
            string Name;
            if (property == null)
            {
                Name = op?.Property?.Name;
                if (string.IsNullOrEmpty(Name))
                    Name = op?.Property?.Account;
            }
            else
            {
                Name = property.Name;
                if (string.IsNullOrEmpty(Name))
                    Name = property.Account;
            }
            return Name;
        }
    }
}