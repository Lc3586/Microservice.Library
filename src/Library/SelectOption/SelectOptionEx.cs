using Library.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Library.SelectOption
{
    /// <summary>
    /// 选项扩展方法
    /// </summary>
    public static class SelectOptionEx
    {
        /// <summary>
        /// 是否非下拉框详细信息展示字段
        /// </summary>
        /// <param name="element">目标成员</param>
        /// <returns></returns>
        public static bool IsNoDropDownFiel(this MemberInfo element)
        {
            var DD = element.GetCustomAttribute(typeof(SelectOptionAttribute));
            return DD != null && ((SelectOptionAttribute)DD).NoDropDownFiel;
        }

        /// <summary>
        /// 获取成员的下拉框数据展示类型
        /// </summary>
        /// <param name="element">目标成员</param>
        /// <returns></returns>
        public static OptionDisplayType GetDropDownDataInfoType(this MemberInfo element)
        {
            var DD = element.GetCustomAttribute(typeof(SelectOptionAttribute));
            return DD == null ? OptionDisplayType.label : ((SelectOptionAttribute)DD).DisplayType;
        }

        /// <summary>
        /// 获取成员的下拉框数据拼接分隔符
        /// </summary>
        /// <param name="element">目标成员</param>
        /// <returns></returns>
        public static char GetDropDownDataSplit(this MemberInfo element)
        {
            var DD = element.GetCustomAttribute(typeof(SelectOptionAttribute));
            return DD == null ? ',' : ((SelectOptionAttribute)DD).Split;
        }

        /// <summary>
        /// 获取成员的下拉框数据排序值
        /// </summary>
        /// <param name="element">目标成员</param>
        /// <returns></returns>
        public static int GetDropDownDataOrder(this MemberInfo element)
        {
            var DD = element.GetCustomAttribute(typeof(SelectOptionAttribute));
            return DD == null ? 0 : ((SelectOptionAttribute)DD).Order;
        }

        /// <summary>
        /// 获取成员的下拉框数据分组名称
        /// </summary>
        /// <param name="element">目标成员</param>
        /// <returns></returns>
        public static string GetDropDownDataGroup(this MemberInfo element)
        {
            var DD = element.GetCustomAttribute(typeof(SelectOptionAttribute));
            return DD == null ? null : ((SelectOptionAttribute)DD).Group;
        }

        /// <summary>
        /// 获取成员的下拉框数据分组排序值
        /// </summary>
        /// <param name="element">目标成员</param>
        /// <returns></returns>
        public static int GetDropDownDataGroupOrder(this MemberInfo element)
        {
            var DD = element.GetCustomAttribute(typeof(SelectOptionAttribute));
            return DD == null ? 0 : ((SelectOptionAttribute)DD).GroupOrder;
        }

        /// <summary>
        /// 获取成员的下拉框数据文本值来源字段
        /// </summary>
        /// <param name="element">目标成员</param>
        /// <returns></returns>
        public static string GetDropDownDataTextField(this MemberInfo element)
        {
            var DD = element.GetCustomAttribute(typeof(SelectOptionAttribute));
            return DD == null ? null : ((SelectOptionAttribute)DD).TextField;
        }



        /// <summary>
        /// 将数据集合转为下拉框数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">数据集合</param>
        /// <param name="func"></param>
        /// <param name="fillOptions">填充选项</param>
        /// <param name="appointTypes">指定数据类型[默认text](key:数据名称,value:数据类型)</param>
        /// <returns></returns>
        public static List<SelectOption> ToDropDownList<T>(this List<T> list, Func<T, SelectOption> func, bool fillOptions = false, Dictionary<string, OptionDisplayType> appointTypes = null)
        {
            var selectList = list.Select(func).ToList();
            if (fillOptions)
                for (int i = 0; i < selectList.Count; i++)
                {
                    selectList.ForEach(o =>
                    {
                        o.options = list[i].ToDropDownDataInfoList(appointTypes);
                    });
                }
            return selectList;
        }

        /// <summary>
        /// 将数据集合转为下拉框数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOption"></typeparam>
        /// <param name="list">数据集合</param>
        /// <param name="func"></param>
        /// <param name="optionList">选项数据集合</param>
        /// <param name="appointTypes">指定数据类型[默认text](key:数据名称,value:数据类型)</param>
        /// <returns></returns>
        public static List<SelectOption> ToDropDownList<T, TOption>(this List<T> list, Func<T, SelectOption> func, List<TOption> optionList, Dictionary<string, OptionDisplayType> appointTypes = null)
        {
            var selectList = list.Select(func).ToList();
            for (int i = 0; i < selectList.Count; i++)
            {
                selectList.ForEach(o =>
                {
                    o.options = optionList[i].ToDropDownDataInfoList(appointTypes);
                });
            }
            return selectList;
        }

        /// <summary>
        /// 将数据集合转为下拉框数据集合
        /// </summary>
        /// <param name="list">数据集合</param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static List<SelectOption> ToDropDownList(this string[] list, Func<string, int, SelectOption> func = null)
        {
            var selectList = new List<SelectOption>();
            if (func == null)
                list.ForEach(o => selectList.Add(new SelectOption() { text = o, value = o, search = o }));
            else
                for (int i = 0; i < list.Length; i++)
                {
                    selectList.Add(func(list[i], i));
                }
            return selectList;
        }

        /// <summary>
        /// 将数据集合转为下拉框数据集合
        /// </summary>
        /// <typeparam name="TOption"></typeparam>
        /// <param name="list">数据集合</param>
        /// <param name="func"></param>
        /// <param name="optionList">选项数据集合</param>
        /// <param name="appointTypes">指定数据类型[默认text](key:数据名称,value:数据类型)</param>
        /// <returns></returns>
        public static List<SelectOption> ToDropDownList<TOption>(this string[] list, Func<string, int, SelectOption> func, List<TOption> optionList, Dictionary<string, OptionDisplayType> appointTypes = null)
        {
            var selectList = new List<SelectOption>();
            if (func == null)
                list.ForEach(o => selectList.Add(new SelectOption() { text = o, value = o, search = o }));
            else
                for (int i = 0; i < list.Length; i++)
                {
                    selectList.Add(func(list[i], i));
                }
            for (int i = 0; i < selectList.Count; i++)
            {
                selectList.ForEach(o =>
                {
                    o.options = optionList[i].ToDropDownDataInfoList(appointTypes);
                });
            }
            return selectList;
        }

        /// <summary>
        /// 将数据模型转换为下拉框数据详细信息集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">数据模型对象</param>
        /// <param name="appointTypes">指定数据类型[默认text](key:数据名称,value:数据类型)</param>
        /// <returns></returns>
        public static List<OptionInfo> ToDropDownDataInfoList<T>(this T model, Dictionary<string, OptionDisplayType> appointTypes = null)
        {
            List<OptionInfo> result = new List<OptionInfo>();
            var pis = model.GetType().GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                try
                {
                    if (pi.IsJsonIgnore() || pi.IsNoDropDownFiel()) continue;
                    OptionInfo OI = new OptionInfo() { field = pi.Name, display = pi.GetDisplayName(), displayType = appointTypes != null && appointTypes.Keys.Contains(pi.Name) ? appointTypes[pi.Name] : pi.GetDropDownDataInfoType(), value = pi.GetValue(model, null), order = pi.GetDropDownDataOrder(), group = pi.GetDropDownDataGroup(), groupOrder = pi.GetDropDownDataGroupOrder() };
                    if (OI.displayType == OptionDisplayType.carousel)
                        OI.value = OI.value.ToString().Split(pi.GetDropDownDataSplit());
                    string TextField = pi.GetDropDownDataTextField();
                    if (!string.IsNullOrWhiteSpace(TextField))
                        OI.text = (pis.First(o => o.Name == TextField)).GetValue(model, null);
                    result.Add(OI);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception)
                {
                    continue;
                }
#pragma warning restore CA1031 // Do not catch general exception types
            }
            return result.OrderBy(o => o.groupOrder).ThenBy(o => o.order).ToList();
        }

        /// <summary>
        /// 将枚举类型转为选项列表
        /// 注：value为值,text为显示内容
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static List<SelectOption> ToOptionList(this Type enumType)
        {
            var values = Enum.GetValues(enumType);
            List<SelectOption> list = new List<SelectOption>();
            foreach (var aValue in values)
            {
                list.Add(new SelectOption
                {
                    value = ((int)aValue).ToString(),
                    text = aValue.ToString()
                });
            }

            return list;
        }
    }
}
