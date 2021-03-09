using Microservice.Library.OfficeDocuments;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace TC
{
    class Program
    {
        static void Main(string[] args)
        {
            var match = Regex.Match("123$Table[表]{说明}123", @$"[$]{"Table"}[[](.*?)[]]{{(.*?)}}");

            var dte = Console.ReadLine().ReadExcel();
            Console.WriteLine(dte.Columns.Count);

            using (var fr = new FileStream(Console.ReadLine(), FileMode.OpenOrCreate, FileAccess.ReadWrite))
                fr.Write(dte.DataTableToExcelBytes(true, false));


            var dt = Console.ReadLine().ReadCSV();
            Console.WriteLine(dt.Columns.Count);

            using (var fr = new FileStream(Console.ReadLine(), FileMode.OpenOrCreate, FileAccess.ReadWrite))
                fr.Write(dt.DataTableToCSVBytes());
        }
    }
}
