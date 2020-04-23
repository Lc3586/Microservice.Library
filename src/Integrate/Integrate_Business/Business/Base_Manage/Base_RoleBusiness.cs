using AutoMapper;
using Coldairarrow.Util;
using Integrate_Entity.Base_Manage;
using Library.Container;
using Library.Extension;
using Library.Models;
using Library.LinqTool;
using System;
using System.Collections.Generic;
using System.Linq;
using static Integrate_Entity.Base_Manage.EnumType;

namespace Integrate_Business.Base_Manage
{
    public class Base_RoleBusiness : BaseBusiness<Base_Role>, IBase_RoleBusiness, IDependency
    {
        #region �ⲿ�ӿ�

        public List<Base_RoleDTO> GetDataList(Pagination pagination, string roleId = null, string roleName = null)
        {
            var where = Where.True<Base_Role>();
            if (!roleId.IsNullOrEmpty())
                where = where.And(x => x.Id == roleId);
            if (!roleName.IsNullOrEmpty())
                where = where.And(x => x.RoleName.Contains(roleName));

            var list = GetIQueryable()
                .Where(where)
                .GetPagination(pagination)
                .ToList()
                .Select(x => Mapper.Map<Base_RoleDTO>(x))
                .ToList();

            SetProperty(list);

            return list;

            void SetProperty(List<Base_RoleDTO> _list)
            {
                var allActionIds = Service.GetIQueryable<Base_Action>().Select(x => x.Id).ToList();

                var ids = _list.Select(x => x.Id).ToList();
                var roleActions = Service.GetIQueryable<Base_RoleAction>()
                    .Where(x => ids.Contains(x.RoleId))
                    .ToList();
                _list.ForEach(aData =>
                {
                    if (aData.RoleName == RoleTypeEnum.��������Ա.ToString())
                        aData.Actions = allActionIds;
                    else
                        aData.Actions = roleActions.Where(x => x.RoleId == aData.Id).Select(x => x.ActionId).ToList();
                });
            }
        }

        public Base_RoleDTO GetTheData(string id)
        {
            return GetDataList(new Pagination(), id).FirstOrDefault();
        }

        [DataAddLog(LogType.ϵͳ��ɫ����, "RoleName", "��ɫ")]
        [DataRepeatValidate(new string[] { "RoleName" }, new string[] { "��ɫ��" })]
        public AjaxResult AddData(Base_Role newData, List<string> actions)
        {
            var res = RunTransaction(() =>
            {
                Insert(newData);
                SetRoleAction(newData.Id, actions);
            });
            if (!res.Success)
                throw new Exception("ϵͳ�쳣,������", res.ex);

            return Success();
        }

        [DataEditLog(LogType.ϵͳ��ɫ����, "RoleName", "��ɫ")]
        [DataRepeatValidate(new string[] { "RoleName" }, new string[] { "��ɫ��" })]
        public AjaxResult UpdateData(Base_Role theData, List<string> actions)
        {
            var res = RunTransaction(() =>
            {
                Update(theData);
                SetRoleAction(theData.Id, actions);
            });
            if (!res.Success)
                throw new Exception("ϵͳ�쳣,������", res.ex);

            return Success();
        }

        [DataDeleteLog(LogType.ϵͳ��ɫ����, "RoleName", "��ɫ")]
        public AjaxResult DeleteData(List<string> ids)
        {
            var res = RunTransaction(() =>
            {
                Delete(ids);
                Service.Delete_Sql<Base_RoleAction>(x => ids.Contains(x.Id));
            });
            if (!res.Success)
                throw new Exception("ϵͳ�쳣,������", res.ex);

            return Success();
        }

        #endregion

        #region ˽�г�Ա

        private void SetRoleAction(string roleId, List<string> actions)
        {
            var roleActions = (actions ?? new List<string>())
                .Select(x => new Base_RoleAction
                {
                    Id = IdHelper.GetId(),
                    ActionId = x,
                    CreateTime = DateTime.Now,
                    RoleId = roleId
                }).ToList();
            Service.Delete_Sql<Base_RoleAction>(x => x.RoleId == roleId);
            Service.Insert(roleActions);
        }

        #endregion

        #region ����ģ��

        #endregion
    }
}