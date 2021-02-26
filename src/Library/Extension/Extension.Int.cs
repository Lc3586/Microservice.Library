using System;
using System.Collections;

namespace Microservice.Library.Extension
{
    public static partial class Extension
    {
        /// <summary>
        /// int转Ascll字符
        /// </summary>
        /// <param name="ascllCode"></param>
        /// <returns></returns>
        public static string ToAscllStr(this int ascllCode)
        {
            if (ascllCode >= 0 && ascllCode <= 255)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)ascllCode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            else
            {
                throw new Exception("ASCII Code is not valid.");
            }
        }

        /// <summary>
        /// jsGetTime转为DateTime
        /// </summary>
        /// <param name="jsGetTime">js中Date.getTime()</param>
        /// <returns></returns>
        [Obsolete("建议使用Extention.Long中的拓展方法")]
        public static DateTime ToDateTime_From_JsGetTime(this long jsGetTime)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(jsGetTime + "0000");  //说明下，时间格式为13位后面补加4个"0"，如果时间格式为10位则后面补加7个"0",至于为什么我也不太清楚，也是仿照人家写的代码转换的
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow); //得到转换后的时间

            return dtResult;
        }

        /// <summary>
        /// 二维数组排序
        /// </summary>
        /// <param name="data">二维数据</param>
        /// <param name="sortIndex">用作排序依据的维度(可能的值：0,1)</param>
        /// <param name="asc">true:升序,false:降序</param>
        /// <returns></returns>
        public static int[,] OrderBy(this int[,] data, int sortIndex = 0, bool asc = true)
        {
            var result = data;
            if (data == null)
                goto end; ;
            if (data.Length == 0)
                goto end;

            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(0); j++)
                {
                    var compare = Comparer.Default.Compare(data[i, sortIndex], data[j, sortIndex]);
                    if (compare == 0)
                        continue;
                    if ((asc && compare > 0) || (!asc && compare < 0))
                    {
                        var temp = data[i, sortIndex];
                        data[i, sortIndex] = data[j, sortIndex];
                        data[j, sortIndex] = temp;
                    }
                }
            }

            end:
            return result;
        }

        /// <summary>
        /// int转bool
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool ToBool(this int value)
        {
            if (value == 0)
                return false;
            if (value == 1)
                return true;
            return Convert.ToBoolean(value);
        }
    }
}
