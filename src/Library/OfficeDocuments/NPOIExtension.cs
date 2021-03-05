using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.IO;

namespace Microservice.Library.OfficeDocuments
{
    /// <summary>
    /// NPOI拓展方法
    /// </summary>
    public static class NPOIExtension
    {
        #region Excel

        /// <summary>
        /// 将DataTable输出为字节数组
        /// </summary>
        /// <param name="dt">表格数据</param>
        /// <param name="firstRowIsTitle">第一行是否为标题</param>
        /// <param name="xslx">
        /// <para>true .xlsx</para>
        /// <para>false .xls</para>
        /// <para>默认 true</para>
        /// </param>
        /// <returns>Byte数组</returns>
        public static byte[] DataTableToExcelBytes(this DataTable dt, bool firstRowIsTitle = true, bool xslx = true)
        {
            var workbook = xslx ? (IWorkbook)new XSSFWorkbook() : new HSSFWorkbook();
            var sheet = workbook.CreateSheet();

            int colnum = dt.Columns.Count;//表格列数 
            int rownum = dt.Rows.Count;//表格行数

            //生成行 列名行 
            var row_title = sheet.CreateRow(0);
            if (firstRowIsTitle)
                for (int i = 0; i < colnum; i++)
                {
                    row_title.CreateCell(i)
                        .SetCellValue(dt.Columns[i].ColumnName);
                }

            //生成数据行 
            for (int i = 0; i < rownum; i++)
            {
                var row = sheet.CreateRow(i += (firstRowIsTitle ? 1 : 0));
                for (int j = 0; j < colnum; j++)
                {
                    row.CreateCell(j)
                        .SetCellValue(dt.Rows[i][j].ToString());
                }
            }

            //自适应列宽
            for (int i = 0; i < colnum; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            //将DataTable写入内存流
            var ms = new MemoryStream();
            workbook.Write(ms);
            return ms.ToArray();
        }

        /// <summary>
        /// 读取工作簿
        /// </summary>
        /// <param name="sheet">工作簿</param>
        /// <param name="firstRowIsTitle">第一行是否为标题</param>
        /// <returns></returns>
        public static DataTable ReadSheet(this ISheet sheet, bool firstRowIsTitle = true)
        {
            var table = new DataTable();

            for (int i = 0; i < sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                for (int j = 0; j < row.Cells.Count; j++)
                {
                    if (i == 0 && firstRowIsTitle)
                        table.Columns.Add(row.Cells[j].StringCellValue);
                    else
                    {
                        table.Rows.Add(table.NewRow());
                        table.Rows[i -= (firstRowIsTitle ? 1 : 0)][j] = row.Cells[j].StringCellValue;
                    }
                }
            }

            return table;
        }

        /// <summary>
        /// 从excel文件导入数据
        /// </summary>
        /// <param name="fileNmae">文件</param>
        /// <param name="firstRowIsTitle">第一行是否为标题</param>
        /// <returns></returns>
        public static DataTable ReadExcel(this string fileNmae, bool firstRowIsTitle = true)
        {
            using (var fs = new FileStream(fileNmae, FileMode.Open, FileAccess.Read))
                return ReadExcel(fs, firstRowIsTitle, fileNmae.Substring(fileNmae.LastIndexOf('.')) == "xslx");
        }

        /// <summary>
        /// 从excel文件字节源导入
        /// </summary>
        /// <param name="fileBytes">文件字节源</param>
        /// <param name="firstRowIsTitle">第一行是否为标题</param>
        /// <param name="xslx">是否为xslx文件</param>
        /// <returns></returns>
        public static DataTable ReadExcel(this byte[] fileBytes, bool firstRowIsTitle = true, bool xslx = true)
        {
            using (var ms = new MemoryStream(fileBytes))
                return ReadExcel(ms, firstRowIsTitle, xslx);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="firstRowIsTitle">第一行是否为标题</param>
        /// <param name="xslx">是否为xslx文件</param>
        /// <returns></returns>
        public static DataTable ReadExcel(this Stream stream, bool firstRowIsTitle = true, bool xslx = true)
        {
            var workbook = xslx
                ? (IWorkbook)new XSSFWorkbook(stream)
                : new HSSFWorkbook(stream);

            var sheet = workbook.GetSheetAt(0);

            return ReadSheet(sheet, firstRowIsTitle);
        }

        #endregion
    }
}
