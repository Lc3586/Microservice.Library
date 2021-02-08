using Business.Interface.System;
using Library.Container;
using Library.Extension;
using Model.Common;
using Model.System.UserDTO;
using System;

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
        /// <param name="operatorDetail"></param>
        /// <returns></returns>
        public static T InitEntity<T>(this T entity, OperatorDetail operatorDetail = null)
        {
            var op = AutofacHelper.GetScopeService<IOperator>();

            if (entity.ContainsProperty("Id"))
                entity.SetPropertyValue("Id", IdHelper.NextIdUpper());
            if (entity.ContainsProperty("CreateTime"))
                entity.SetPropertyValue("CreateTime", DateTime.Now);
            if (entity.ContainsProperty("CreatorId"))
                entity.SetPropertyValue("CreatorId", operatorDetail == null ? op?.Id : operatorDetail.Id);
            if (entity.ContainsProperty("CreatorName"))
                entity.SetPropertyValue("CreatorName", GetUserName(op, operatorDetail));

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
        /// <param name="operatorDetail"></param>
        /// <returns></returns>
        public static T ModifyEntity<T>(this T entity, OperatorDetail operatorDetail = null)
        {
            var op = AutofacHelper.GetScopeService<IOperator>();

            if (entity.ContainsProperty("Id"))
            {
                if (entity.ContainsProperty("ModifyTime"))
                    entity.SetPropertyValue("ModifyTime", DateTime.Now);
                if (entity.ContainsProperty("ModifiedById"))
                    entity.SetPropertyValue("ModifiedById", operatorDetail == null ? op?.Id : operatorDetail.Id);
                if (entity.ContainsProperty("ModifiedByName"))
                    entity.SetPropertyValue("ModifiedByName", GetUserName(op, operatorDetail));
            }
            return entity;
        }

        /// <summary>
        /// 获取当前登录用户的用户名
        /// </summary>
        /// <param name="op"></param>
        /// <param name="operatorDetail"></param>
        /// <returns></returns>
        public static string GetUserName(IOperator op = null, OperatorDetail operatorDetail = null)
        {
            op = op ?? AutofacHelper.GetScopeService<IOperator>();

            string Name;
            if (operatorDetail == null)
            {
                Name = op?.Detail?.Name;
                if (string.IsNullOrEmpty(Name))
                    Name = op?.Detail?.Account;
            }
            else
            {
                Name = operatorDetail.Name;
                if (string.IsNullOrEmpty(Name))
                    Name = operatorDetail.Account;
            }
            return Name;
        }
    }
}
