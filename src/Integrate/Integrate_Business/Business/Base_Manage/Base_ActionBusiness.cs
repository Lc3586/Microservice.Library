using Integrate_Entity.Base_Manage;
using Library.Container;
using Library.Models;
using Library.LinqTool;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Library.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using static Integrate_Entity.Base_Manage.EnumType;
using Coldairarrow.Util;
using Library.Tree;

namespace Integrate_Business.Base_Manage
{
    public class Base_ActionBusiness : BaseBusiness<Base_Action>, IBase_ActionBusiness, IDependency
    {
        #region �ⲿ�ӿ�

        public List<Base_Action> GetDataList(Pagination pagination, string keyword = null, string parentId = null, List<int> types = null, IQueryable<Base_Action> q = null)
        {
            q = q ?? GetIQueryable();
            var where = Where.True<Base_Action>();
            if (!keyword.IsNullOrEmpty())
            {
                where = where.And(x => EF.Functions.Like(x.Name, $"%{keyword}%"));
            }
            if (!parentId.IsNullOrEmpty())
                where = where.And(x => x.ParentId == parentId);
            if (types?.Count > 0)
                where = where.And(x => types.Contains(x.Type));

            return q.Where(where).GetPagination(pagination).ToList();
        }

        public List<Base_ActionDTO> GetTreeDataList(string keyword, List<int> types, bool selectable, IQueryable<Base_Action> q = null, bool checkEmptyChildren = false)
        {
            var where = Where.True<Base_Action>();
            if (!types.IsNullOrEmpty())
                where = where.And(x => types.Contains(x.Type));
            var qList = (q ?? GetIQueryable()).Where(where).OrderBy(x => x.Sort).ToList();

            var treeList = qList.Select(x => new Base_ActionDTO
            {
                Id = x.Id,
                NeedAction = x.NeedAction,
                Text = x.Name,
                ParentId = x.ParentId,
                Type = x.Type,
                Url = x.Url,
                Value = x.Id,
                Icon = x.Icon,
                Sort = x.Sort,
                selectable = selectable
            }).ToList();

            //�˵��ڵ���,���ӽڵ�Ϊ�����Ƴ����ڵ�
            if (checkEmptyChildren)
                treeList = treeList.Where(x => x.Type != 0 || TreeHelper.GetChildren(treeList, x, false).Count > 0).ToList();

            SetProperty(treeList);

            return TreeHelper.BuildTree(treeList);

            void SetProperty(List<Base_ActionDTO> _list)
            {
                var ids = _list.Select(x => x.Id).ToList();
                var allPermissions = GetIQueryable()
                    .Where(x => ids.Contains(x.ParentId) && x.Type == 2)
                    .ToList();

                _list.ForEach(aData =>
                {
                    var permissionValues = allPermissions
                        .Where(x => x.ParentId == aData.Id)
                        .Select(x => $"{x.Name}({x.Value})")
                        .ToList();

                    aData.PermissionValues = permissionValues;
                });
            }
        }

        /// <summary>
        /// ��ȡָ���ĵ�������
        /// </summary>
        /// <param name="id">����</param>
        /// <returns></returns>
        public Base_Action GetTheData(string id)
        {
            return GetEntity(id);
        }

        public AjaxResult AddData(Base_Action newData, List<Base_Action> permissionList)
        {
            var res = RunTransaction(() =>
            {
                Insert(newData);
                SavePermission(newData.Id, permissionList);
            });
            if (res.Success)
                return Success();
            else
                throw res.ex;
        }

        public AjaxResult UpdateData(Base_Action theData, List<Base_Action> permissionList)
        {
            var res = RunTransaction(() =>
            {
                Update(theData);
                SavePermission(theData.Id, permissionList);
            });
            if (res.Success)
                return Success();
            else
                throw res.ex;
        }

        public AjaxResult DeleteData(List<string> ids)
        {
            Delete(ids);

            return Success();
        }

        public void SavePermission(string parentId, List<Base_Action> permissionList)
        {
            permissionList.ForEach(aData =>
            {
                aData.Id = IdHelper.GetId();
                aData.CreateTime = DateTime.Now;
                aData.CreatorId = null;
                aData.CreatorRealName = null;
                aData.ParentId = parentId;
                aData.NeedAction = true;
            });
            //ɾ��ԭ��
            Delete_Sql(x => x.ParentId == parentId && x.Type == 2);
            //����
            Insert(permissionList);

            //Ȩ��ֵ����Ψһ
            var repeatValues = GetIQueryable()
                .GroupBy(x => x.Value)
                .Where(x => !string.IsNullOrEmpty(x.Key) && x.Count() > 1)
                .Select(x => x.Key)
                .ToList();
            if (repeatValues.Count > 0)
                throw new Exception($"����Ȩ��ֵ�ظ�:{string.Join(",", repeatValues)}");
        }

        #endregion

        #region ˽�г�Ա

        #endregion

        #region ����ģ��

        #endregion
    }

    public class Base_ActionDTO : TreeModel
    {
        /// <summary>
        /// ����,�˵�=0,ҳ��=1,Ȩ��=2
        /// </summary>
        public Int32 Type { get; set; }

        /// <summary>
        /// �˵���ַ
        /// </summary>
        public String Url { get; set; }

        public string path { get => Url; }

        /// <summary>
        /// �Ƿ���ҪȨ��(��ҳ����Ч)
        /// </summary>
        public bool NeedAction { get; set; }

        public string TypeText { get => ((ActionTypeEnum)Type).ToString(); }
        public string NeedActionText { get => NeedAction ? "��" : "��"; }
        public object children { get => Children; }

        public string title { get => Text; }
        public string value { get => Id; }
        public string key { get => Id; }
        public bool selectable { get; set; }

        /// <summary>
        /// ͼ��
        /// </summary>
        [JsonIgnore]
        public string Icon { get; set; }

        public string icon { get => Icon; }

        /// <summary>
        /// ����
        /// </summary>
        public int Sort { get; set; }

        public List<string> PermissionValues { get; set; }
    }
}