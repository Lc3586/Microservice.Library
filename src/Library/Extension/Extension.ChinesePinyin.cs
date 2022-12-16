using Microsoft.International.Converters.PinYinConverter;
using System.Collections.Generic;
using System.Linq;

namespace Microservice.Library.Extension
{
    /// <summary>
    /// 中文拼音帮助类
    /// </summary>
    /// <remarks>
    /// LCTR 2019-01-28
    /// </remarks>
    public static partial class Extension
    {
        /// <summary> 
        /// 汉字转化为拼音
        /// </summary> 
        /// <param name="str">汉字字符串</param> 
        /// <param name="None">非中文字符的填充字符[默认使用其本身进行填充]</param>
        /// <param name="RemovePS">是否去除音标</param>
        /// <param name="JustFirst">是否只获取首字母</param>
        /// <param name="Upper">是否转为大写</param>
        /// <param name="Lower">是否转为小写</param>
        /// <returns>全拼集合</returns> 
        public static List<string> GetPinyin(this string str, string None = null, bool RemovePS = false, bool JustFirst = false, bool Upper = false, bool Lower = false)
        {
            List<string> result = new List<string>();
            if (string.IsNullOrWhiteSpace(str))
                return result;
            List<KeyValuePair<char, List<string>>> Data = str.Select(v => new KeyValuePair<char, List<string>>(v, CheckStringChineseReg(v.ToString()) ? new ChineseChar(v).Pinyins.Where(o => o != null).Select(o => o.Substring(0, JustFirst ? 1 : (RemovePS ? o.Length - 1 : o.Length))).Distinct().ToList() : new List<string>())).ToList();
            for (int i = 0; i < Data.Max(o => o.Value.Count); i++)
            {
                string value = string.Join("", Data.Select(o => o.Value == null || o.Value.Count == 0 ? (None == null ? o.Key.ToString() : None) : (o.Value.Count - 1 >= i ? o.Value[i] : o.Value.FirstOrDefault())));
                if (Upper)
                    value = value.ToUpperInvariant();
                if (Lower)
                    value = value.ToLowerInvariant();
                result.Add(value);
            }
            return result;
        }

        /// <summary>
        /// 判断字符是否为汉字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool CheckStringChineseReg(this string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"[\u4e00-\u9fbb]+$");
        }
    }
}
