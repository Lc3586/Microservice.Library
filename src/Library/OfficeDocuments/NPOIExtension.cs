﻿using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
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
                    var cell = row_title.CreateCell(i);
                    cell.SetCellType(CellType.String);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                }

            //生成数据行 
            for (int i = 0; i < rownum; i++)
            {
                var row = sheet.CreateRow(i + (firstRowIsTitle ? 1 : 0));
                for (int j = 0; j < colnum; j++)
                {
                    var cell = row.CreateCell(j);

                    var type = dt.Columns[j].DataType;
                    if (type == typeof(bool))
                        cell.SetCellValue((bool)dt.Rows[i][j]);
                    else if (type == typeof(byte)
                        || type == typeof(short)
                        || type == typeof(int))
                        cell.SetCellValue((int)dt.Rows[i][j]);
                    else if (type == typeof(TimeSpan)
                        || type == typeof(DateTime))
                        cell.SetCellValue((DateTime)dt.Rows[i][j]);
                    else
                        cell.SetCellValue(dt.Rows[i][j].ToString());
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
        /// <param name="offsetRow">位移行数</param>
        /// <param name="offsetCell">位移单元格数</param>
        /// <returns></returns>
        public static DataTable ReadSheet(this ISheet sheet, bool firstRowIsTitle = true, int offsetRow = 0, int offsetCell = 0)
        {
            var table = new DataTable();

            var cellCount = 0;
            for (int i = offsetRow; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                if (row == null)
                    continue;

                if (i == offsetRow)
                    cellCount = row.LastCellNum;

                if (i != offsetRow || !firstRowIsTitle)
                    table.Rows.Add(table.NewRow());

                for (int j = offsetCell; j < cellCount; j++)
                {
                    var cell = row.GetCell(j);
                    if (cell == null)
                        continue;

                    if (i == offsetRow && firstRowIsTitle)
                        table.Columns.Add(cell.StringCellValue);
                    else
                        table.Rows[table.Rows.Count - 1][j] = cell.ToString();
                }
            }

            return table;
        }

        /// <summary>
        /// 从excel文件导入数据
        /// </summary>
        /// <param name="fileNmae">文件</param>
        /// <param name="firstRowIsTitle">第一行是否为标题</param>
        /// <param name="offsetRow">位移行数</param>
        /// <param name="offsetCell">位移单元格数</param>
        /// <returns></returns>
        public static DataTable ReadExcel(this string fileNmae, bool firstRowIsTitle = true, int offsetRow = 0, int offsetCell = 0)
        {
            using (var fs = new FileStream(fileNmae, FileMode.Open, FileAccess.Read))
                return ReadExcel(fs, firstRowIsTitle, fileNmae.Substring(fileNmae.LastIndexOf('.')) == ".xlsx", offsetRow, offsetCell);
        }

        /// <summary>
        /// 从excel文件字节源导入
        /// </summary>
        /// <param name="fileBytes">文件字节源</param>
        /// <param name="firstRowIsTitle">第一行是否为标题</param>
        /// <param name="xslx">是否为xslx文件</param>
        /// <param name="offsetRow">位移行数</param>
        /// <param name="offsetCell">位移单元格数</param>
        /// <returns></returns>
        public static DataTable ReadExcel(this byte[] fileBytes, bool firstRowIsTitle = true, bool xslx = true, int offsetRow = 0, int offsetCell = 0)
        {
            using (var ms = new MemoryStream(fileBytes))
                return ReadExcel(ms, firstRowIsTitle, xslx, offsetRow, offsetCell);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="firstRowIsTitle">第一行是否为标题</param>
        /// <param name="xslx">是否为xslx文件</param>
        /// <param name="offsetRow">位移行数</param>
        /// <param name="offsetCell">位移单元格数</param>
        /// <returns></returns>
        public static DataTable ReadExcel(this Stream stream, bool firstRowIsTitle = true, bool xslx = true, int offsetRow = 0, int offsetCell = 0)
        {
            var workbook = xslx
                ? (IWorkbook)new XSSFWorkbook(stream)
                : new HSSFWorkbook(stream);

            var sheet = workbook.GetSheetAt(0);

            return ReadSheet(sheet, firstRowIsTitle, offsetRow, offsetCell);
        }

        #endregion
    }
}
