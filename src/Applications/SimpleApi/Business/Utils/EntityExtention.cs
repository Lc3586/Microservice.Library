using System;
using Business.Interface.System;
using Library.Container;
using Library.Extension;
using Model.System.UserDTO;

namespace Business.Utils
{
    /// <summary>
    /// 实体类拓展方法
    /// </summary>
    public static partial class EntityExtention
    {
        /// <summary>
        /// 初始化实体，不处理当前登录人
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T InitEntityWithoutOP<T>(this T entity)
        {
            if (entity.ContainsProperty("Id"))
                entity.SetPropertyValue("Id", IdHelper.NextIdUpper());
            if (entity.ContainsProperty("CreateTime"))
                entity.SetPropertyValue("CreateTime", DateTime.Now);

            return entity;
        }

        /// <summary>
        /// 初始化实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static T InitEntity<T>(this T entity, Authorities property = null)
        {
            var op = AutofacHelper.GetScopeService<IOperator>();

            if (entity.ContainsProperty("Id"))
                entity.SetPropertyValue("Id", IdHelper.NextIdUpper());
            if (entity.ContainsProperty("CreateTime"))
                entity.SetPropertyValue("CreateTime", DateTime.Now);
            if (entity.ContainsProperty("CreatorId"))
                entity.SetPropertyValue("CreatorId", property == null ? op?.UserId : property.Id);
            if (entity.ContainsProperty("CreatorName"))
                entity.SetPropertyValue("CreatorName", GetUserName(op, property));

            return entity;
        }

        /// <summary>
        /// 修改实体，不处理当前登录人
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T ModifyEntityWithoutOP<T>(this T entity)
        {
            if (entity.ContainsProperty("Id"))
            {
                if (entity.ContainsProperty("ModifyTime"))
                    entity.SetPropertyValue("ModifyTime", DateTime.Now);
            }
            return entity;
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static T ModifyEntity<T>(this T entity, Authorities property = null)
        {
            var op = AutofacHelper.GetScopeService<IOperator>();

            if (entity.ContainsProperty("Id"))
            {
                if (entity.ContainsProperty("ModifyTime"))
                    entity.SetPropertyValue("ModifyTime", DateTime.Now);
                if (entity.ContainsProperty("ModifiedById"))
                    entity.SetPropertyValue("ModifiedById", property == null ? op?.UserId : property.Id);
                if (entity.ContainsProperty("ModifiedByName"))
                    entity.SetPropertyValue("ModifiedByName", GetUserName(op, property));
            }
            return entity;
        }

        /// <summary>
        /// 获取当前登录用户的用户名
        /// </summary>
        /// <param name="op"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetUserName(IOperator op = null, Authorities property = null)
        {
            op = op ?? AutofacHelper.GetScopeService<IOperator>();

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
