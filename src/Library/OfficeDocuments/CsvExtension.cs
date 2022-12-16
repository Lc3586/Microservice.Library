using CsvHelper;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

namespace Microservice.Library.OfficeDocuments
{
    /// <summary>
    /// CSV文件拓展方法
    /// </summary>
    public static class CsvExtension
    {
        #region CSV

        /// <summary>
        /// 将DataTable输出为字节数组
        /// </summary>
        /// <param name="dt">表格数据</param>
        /// <param name="firstRowIsTitle">第一行是否为标题</param>
        /// <param name="encoding">编码</param>
        /// <returns>Byte数组</returns>
        public static byte[] DataTableToCSVBytes(this DataTable dt, bool firstRowIsTitle = true, Encoding encoding = null)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new StreamWriter(ms, encoding ?? Encoding.Default))
                using (var scv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    int colnum = dt.Columns.Count;//表格列数 
                    int rownum = dt.Rows.Count;//表格行数

                    if (firstRowIsTitle)
                    {
                        for (int i = 0; i < colnum; i++)
                            scv.WriteField(dt.Columns[i].ColumnName);

                        scv.NextRecord();
                    }

                    for (int i = 0; i < rownum; i++)
                    {
                        for (int j = 0; j < colnum; j++)
                            scv.WriteField(dt.Rows[i][j].ToString());

                        scv.NextRecord();
                    }
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// 读取工作簿
        /// </summary>
        /// <param name="csv">CSV文件读取器</param>
        /// <param name="firstRowIsTitle">第一行是否为标题</param>
        /// <param name="offsetRow">位移行数</param>
        /// <returns></returns>
        public static DataTable ReadCSV(this CsvReader csv, bool firstRowIsTitle = true, int offsetRow = 0)
        {
            using (var dr = new CsvDataReader(csv))
            {
                var table = new DataTable();
                if (firstRowIsTitle)
                {
                    for (int i = offsetRow; i < dr.FieldCount; i++)
                        table.Columns.Add(dr.GetName(i));
                }
                table.Load(dr);
                return table;
            }
        }

        /// <summary>
        /// 从CSV文件导入数据
        /// </summary>
        /// <param name="fileNmae">文件</param>
        /// <param name="firstRowIsTitle">第一行是否为标题</param>
        /// <param name="encoding">编码</param>
        /// <param name="offsetRow">位移行数</param>
        /// <returns></returns>
        public static DataTable ReadCSV(this string fileNmae, bool firstRowIsTitle = true, Encoding encoding = null, int offsetRow = 0)
        {
            using (var fs = new FileStream(fileNmae, FileMode.Open, FileAccess.Read))
                return ReadCSV(fs, firstRowIsTitle, encoding, offsetRow);
        }

        /// <summary>
        /// 从CSV文件字节源导入
        /// </summary>
        /// <param name="fileBytes">文件字节源</param>
        /// <param name="firstRowIsTitle">第一行是否为标题</param>
        /// <param name="encoding">编码</param>
        /// <param name="offsetRow">位移行数</param>
        /// <returns></returns>
        public static DataTable ReadCSV(this byte[] fileBytes, bool firstRowIsTitle = true, Encoding encoding = null, int offsetRow = 0)
        {
            using (var ms = new MemoryStream(fileBytes))
                return ReadCSV(ms, firstRowIsTitle, encoding, offsetRow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="firstRowIsTitle">第一行是否为标题</param>
        /// <param name="encoding">编码</param>
        /// <param name="offsetRow">位移行数</param>
        /// <returns></returns>
        public static DataTable ReadCSV(this Stream stream, bool firstRowIsTitle = true, Encoding encoding = null, int offsetRow = 0)
        {
            using (var sr = new StreamReader(stream, encoding ?? Encoding.Default))
            using (var cr = new CsvReader(sr, CultureInfo.InvariantCulture))
                return ReadCSV(cr, firstRowIsTitle, offsetRow);
        }

        #endregion
    }
}
