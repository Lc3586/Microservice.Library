using Microservice.Library.OfficeDocuments;
using System;
using System.IO;

namespace TC
{
    class Program
    {
        static void Main(string[] args)
        {
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
