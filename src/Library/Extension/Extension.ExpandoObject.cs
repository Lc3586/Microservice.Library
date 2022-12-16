using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace Microservice.Library.Extension
{
    public static partial class Extension
    {
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="expandoObj">动态对象</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">属性值</param>
        public static void AddProperty(this ExpandoObject expandoObj, string propertyName, object value)
        {
            var obj = (IDictionary<string, object>)expandoObj;
            if (obj.ContainsKey(propertyName))
                throw new Exception("已存在该属性！");
            else
                obj.Add(propertyName, value);
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="expandoObj">动态对象</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">属性值</param>
        public static void SetProperty(this ExpandoObject expandoObj, string propertyName, object value)
        {
            var obj = (IDictionary<string, object>)expandoObj;
            if (!obj.ContainsKey(propertyName))
                obj.Add(propertyName, value);
            else
                obj[propertyName] = value;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="expandoObj">动态对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static object GetProperty(this ExpandoObject expandoObj, string propertyName)
        {
            var obj = (IDictionary<string, object>)expandoObj;
            if (!obj.ContainsKey(propertyName))
                throw new Exception("不存在该属性！");
            else
                return obj[propertyName];
        }

        /// <summary>
        /// 获取所有属性
        /// </summary>
        /// <param name="expandoObj">动态对象</param>
        /// <returns></returns>
        public static List<string> GetProperties(this ExpandoObject expandoObj)
        {
            var obj = (IDictionary<string, object>)expandoObj;
            return obj.Keys.CastToList<string>();
        }

        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="expandoObj">动态对象</param>
        /// <param name="propertyName">属性名</param>
        public static void RemoveProperty(this ExpandoObject expandoObj, string propertyName)
        {
            var obj = (IDictionary<string, object>)expandoObj;
            if (!obj.ContainsKey(propertyName))
                throw new Exception("不存在该属性！");
            else
                obj.Remove(propertyName);
        }

        /// <summary>
        /// 将动态属性对象ExpandoObject列表转为DataTable
        /// </summary>
        /// <param name="dataList">数据源</param>
        /// <returns></returns>
        public static DataTable ToDataTable(this IEnumerable<ExpandoObject> dataList)
        {
            DataTable dt = new DataTable();
            if (dataList.IsNullOrEmpty())
                return null;
            else if (dataList.Count() == 0)
                return dt;
            else
            {
                var aEntity = dataList.FirstOrDefault();
                var properties = aEntity.GetProperties();
                properties.ForEach(aProperty =>
                {
                    dt.Columns.Add(aProperty);
                });
                dataList.ForEach((aData, index) =>
                {
                    dt.Rows.Add(dt.NewRow());
                    properties.ForEach(aProperty =>
                    {
                        dt.Rows[index][aProperty] = aData.GetProperty(aProperty);
                    });
                });
            }

            return dt;
        }

        /// <summary>
        /// 将DataSet转为动态属性对象ExpandoObject列表(第一张表)
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static List<ExpandoObject> ToExpandoObjectList(this DataSet dataSet)
        {
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return null;

            return ToExpandoObjectList(dataSet.Tables[0]);
        }

        /// <summary>
        /// 将DataTable转为动态属性对象ExpandoObject列表
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<ExpandoObject> ToExpandoObjectList(this DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
                return null;

            var expandoObjectList = new List<ExpandoObject>();
            var rowsArray = new DataRow[dataTable.Rows.Count];
            dataTable.Rows.CopyTo(rowsArray, 0);

            foreach (var item in rowsArray)
                expandoObjectList.Add(ToExpandoObject(item, dataTable.Columns));

            return expandoObjectList;
        }

        /// <summary>
        /// 将DataSet转为动态属性对象ExpandoObject
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static ExpandoObject ToExpandoObject(this DataSet dataSet)
        {
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return null;

            return ToExpandoObject(dataSet.Tables[0]);
        }

        /// <summary>
        /// 将DataTable转为动态属性对象ExpandoObject
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static ExpandoObject ToExpandoObject(this DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
                return null;

            return ToExpandoObject(dataTable.Rows[0], dataTable.Columns);
        }

        /// <summary>
        /// 将DataRow转为动态属性对象ExpandoObject
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        private static ExpandoObject ToExpandoObject(DataRow row, DataColumnCollection columns = null)
        {
            var expandoObject = new ExpandoObject();
            var obj = (IDictionary<string, object>)expandoObject;

            DataColumn[] columnsArray;
            if (columns == null)
                columnsArray = row.GetColumnsInError();
            else
            {
                columnsArray = new DataColumn[columns.Count];
                columns.CopyTo(columnsArray, 0);
            }

            foreach (var item in columnsArray)
            {
                obj.Add(item.ColumnName, row[item]);
            }

            return expandoObject;
        }
    }
}
