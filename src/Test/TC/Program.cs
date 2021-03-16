using Microservice.Library.Extension;
using Microservice.Library.OfficeDocuments;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace TC
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var string_iso_8859_1 = "";
            Console.WriteLine(string_iso_8859_1);
            var bytes_utf_8 = string_iso_8859_1.ToBytes(Encoding.GetEncoding("ISO-8859-1"));
            var char_utf_8 = new char[Encoding.UTF8.GetCharCount(bytes_utf_8, 0, bytes_utf_8.Length)];
            Encoding.UTF8.GetChars(bytes_utf_8, 0, bytes_utf_8.Length, char_utf_8, 0);
            var string_utf_8 = new string(char_utf_8).ToJObject();
            Console.WriteLine(string_utf_8);

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
