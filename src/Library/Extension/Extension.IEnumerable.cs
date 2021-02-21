using Library.Extension.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Library.Extension
{
    public static partial class Extension
    {
        /// <summary>
        /// 复制序列中的数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="iEnumberable">原数据</param>
        /// <param name="startIndex">原数据开始复制的起始位置</param>
        /// <param name="length">需要复制的数据长度</param>
        /// <returns></returns>
        public static IEnumerable<T> Copy<T>(this IEnumerable<T> iEnumberable, int startIndex, int length)
        {
            var sourceArray = iEnumberable.ToArray();
            T[] newArray = new T[length];
            Array.Copy(sourceArray, startIndex, newArray, 0, length);

            return newArray;
        }

        /// <summary>
        /// 给IEnumerable拓展ForEach方法
        /// </summary>
        /// <typeparam name="T">模型类</typeparam>
        /// <param name="iEnumberable">数据源</param>
        /// <param name="func">方法</param>
        public static void ForEach<T>(this IEnumerable<T> iEnumberable, Action<T> func)
        {
            foreach (var item in iEnumberable)
            {
                func(item);
            }
        }

        /// <summary>
        /// 给IEnumerable拓展ForEach方法
        /// </summary>
        /// <typeparam name="T">模型类</typeparam>
        /// <param name="iEnumberable">数据源</param>
        /// <param name="func">方法</param>
        public static void ForEach<T>(this IEnumerable<T> iEnumberable, Action<T, int> func)
        {
            var array = iEnumberable.ToArray();
            for (int i = 0; i < array.Count(); i++)
            {
                func(array[i], i);
            }
        }

        /// <summary>
        /// 集合是否有存在元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iEnumberable"></param>
        /// <returns></returns>
        public static bool Any_Ex<T>(this IEnumerable<T> iEnumberable)
        {
            return iEnumberable != null && iEnumberable.Count() > 0;
        }

        /// <summary>
        /// 集合是否有存在元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iEnumberable"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool Any_Ex<T>(this IEnumerable<T> iEnumberable, Func<T, bool> predicate)
        {
            if (iEnumberable == null)
                return false;
            return iEnumberable.Count(predicate) > 0;
        }

        /// <summary>
        /// IEnumerable转换为List'T'
        /// </summary>
        /// <typeparam name="T">参数</typeparam>
        /// <param name="source">数据源</param>
        /// <returns></returns>
        public static List<T> CastToList<T>(this IEnumerable source)
        {
            return new List<T>(source.Cast<T>());
        }

        /// <summary>
        /// 将IEnumerable'T'转为对应的DataTable
        /// </summary>
        /// <typeparam name="T">数据模型</typeparam>
        /// <param name="iEnumberable">数据源</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> iEnumberable)
        {
            return iEnumberable.ToJson().ToDataTable();
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="iEnumberable">数据源</param>
        /// <param name="recordCount">总数据量(不分页)</param>
        /// <param name="pageIndex">指定页码</param>
        /// <param name="pageRows">每页数据量</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="sortType">排序类型(desc,asc)</param>
        /// <returns></returns>
        public static IEnumerable<T> GetPagination<T>(this IEnumerable<T> iEnumberable, out int recordCount, int? pageIndex = null, int? pageRows = null, string sortField = null, string sortType = "asc")
        {
            recordCount = iEnumberable.Count();
            var query = iEnumberable.AsQueryable();
            if (sortField != null)
                query = query.OrderBy($@"{sortField} {sortType ?? "asc"}");
            if (pageIndex != null)
                query = query.Skip((pageIndex.Value - 1) * pageRows.Value).Take(pageRows.Value);
            return query.ToList();
        }

        /// <summary>
        /// 下一个随机值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="source">值的集合</param>
        /// <returns></returns>
        public static T Next<T>(this IEnumerable<T> source)
        {
            return source.ToList()[new Random().Next(0, source.Count())];
        }

        /// <summary>
        /// 建造树结构
        /// </summary>
        /// <param name="allNodes">所有的节点</param>
        /// <returns></returns>
        public static List<T> BuildTree<T>(this List<T> allNodes) where T : TreeModel, new()
        {
            List<T> resData = new List<T>();
            var rootNodes = allNodes.Where(x => x.ParentId == "0" || string.IsNullOrEmpty(x.ParentId)).ToList();
            resData = rootNodes;
            resData.ForEach(aRootNode =>
            {
                if (allNodes.HaveChildren(aRootNode.Id))
                    aRootNode.Children = allNodes.GetChildren(aRootNode);
            });

            return resData;
        }
        /// <summary>
        /// 获取所有子节点
        /// 注：包括自己
        /// </summary>
        /// <typeparam name="T">节点类型</typeparam>
        /// <param name="allNodes">所有节点</param>
        /// <param name="parentNode">父节点</param>
        /// <param name="includeSelf">是否包括自己</param>
        /// <returns></returns>
        public static List<T> GetChildren<T>(this List<T> allNodes, T parentNode, bool includeSelf) where T : TreeModel
        {
            List<T> resList = new List<T>();
            if (includeSelf)
                resList.Add(parentNode);
            _getChildren(allNodes, parentNode, resList);

            return resList;

            void _getChildren(List<T> _allNodes, T _parentNode, List<T> _resNodes)
            {
                var children = _allNodes.Where(x => x.ParentId == _parentNode.Id).ToList();
                _resNodes.AddRange(children);
                children.ForEach(aChild =>
                {
                    _getChildren(_allNodes, aChild, _resNodes);
                });
            }
        }

        /// <summary>
        /// 获取所有子节点
        /// </summary>
        /// <typeparam name="T">树模型（TreeModel或继承它的模型）</typeparam>
        /// <param name="nodes">所有节点列表</param>
        /// <param name="parentNode">父节点Id</param>
        /// <returns></returns>
        public static List<object> GetChildren<T>(this List<T> nodes, T parentNode) where T : TreeModel, new()
        {
            Type type = typeof(T);
            var properties = type.GetProperties().ToList();
            List<object> resData = new List<object>();
            var children = nodes.Where(x => x.ParentId == parentNode.Id).ToList();
            children.ForEach(aChildren =>
            {
                T newNode = new T();
                resData.Add(newNode);

                //赋值属性
                properties.ForEach(aProperty =>
                {
                    var value = aProperty.GetValue(aChildren, null);
                    aProperty.SetValue(newNode, value);
                });
                //设置深度
                newNode.Level = parentNode.Level + 1;

                if (nodes.HaveChildren(aChildren.Id))
                    newNode.Children = nodes.GetChildren(newNode);
            });

            return resData;
        }

        /// <summary>
        /// 判断当前节点是否有子节点
        /// </summary>
        /// <typeparam name="T">树模型</typeparam>
        /// <param name="nodes">所有节点</param>
        /// <param name="nodeId">当前节点Id</param>
        /// <returns></returns>
        public static bool HaveChildren<T>(this List<T> nodes, string nodeId) where T : TreeModel, new()
        {
            return nodes.Exists(x => x.ParentId == nodeId);
        }
    }
}
