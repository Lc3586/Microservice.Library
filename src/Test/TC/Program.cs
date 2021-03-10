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
            var bytes = Encoding.Convert(
                Encoding.GetEncoding("GB2312"),
                Encoding.UTF8,
                "çé¥­ç«".ToBytes(Encoding.GetEncoding("ISO-8859-1")));
            var chars = new char[Encoding.UTF8.GetCharCount(bytes, 0, bytes.Length)];
            Encoding.UTF8.GetChars(bytes, 0, bytes.Length, chars, 0);
            var result = new string(chars);


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
