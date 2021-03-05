using Microservice.Library.OfficeDocuments;
using System;
using System.IO;

namespace TC
{
    class Program
    {
        static void Main(string[] args)
        {
            var dt = Console.ReadLine().ReadCSV();
            Console.WriteLine(dt.Columns);

            using (var fr = new FileStream(Console.ReadLine(), FileMode.OpenOrCreate, FileAccess.ReadWrite))
                fr.Write(dt.DataTableToCSVBytes());
        }
    }
}
