using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Extention
{
    public static partial class Extention
    {
        /// <summary>
        /// 多选枚举转为对应文本,逗号隔开
        /// </summary>
        /// <param name="values">多个值</param>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static string ToMultipleText(this Type enumType, List<int> values)
        {
            if (values == null)
                return string.Empty;

            List<string> textList = new List<string>();

            var allValues = Enum.GetValues(enumType);
            foreach (var aValue in allValues)
            {
                if (values.Contains((int)aValue))
                    textList.Add(aValue.ToString());
            }

            return string.Join(",", textList);
        }

        /// <summary>
        /// 多选枚举转为对应文本,逗号隔开
        /// </summary>
        /// <param name="values">多个值逗号隔开</param>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static string ToMultipleText(this Type enumType, string values)
        {
            return enumType.ToMultipleText(values?.Split(',')?.Select(x => x.ToInt())?.ToList());
        }
    }
}
