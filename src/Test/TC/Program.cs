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
            //var bytes = Encoding.Convert(
            //    Encoding.GetEncoding("GB2312"),
            //    Encoding.UTF8,
            //    "çé¥­ç«".ToBytes(Encoding.GetEncoding("ISO-8859-1")));
            //var chars = new char[Encoding.UTF8.GetCharCount(bytes, 0, bytes.Length)];
            //Encoding.UTF8.GetChars(bytes, 0, bytes.Length, chars, 0);
            //var result = new string(chars);

            var string_iso_8859_1 = "{\"openid\":\"oK_PG52ahmmoW9Z35AWcXp7l9cTY\",\"nickname\":\"çé¥­ç«\",\"sex\":1,\"language\":\"zh_CN\",\"city\":\"å®æ³¢\",\"province\":\"æµæ±\",\"country\":\"ä¸­å½\",\"headimgurl\":\"https://thirdwx.qlogo.cn/mmopen/vi_32/WJD4sghFshpgic9icleY3ric4uyaaV66QGSYkf2OSsXe8KlAlBfDTwzETJWwaeCu1zD9nX8sfVEnQrfhEapmModvw/132\",\"privilege\":[]}";
            Console.WriteLine(string_iso_8859_1);
            var bytes_utf_8 = string_iso_8859_1.ToBytes(Encoding.GetEncoding("ISO-8859-1"));
            var char_utf_8 = new char[Encoding.UTF8.GetCharCount(bytes_utf_8, 0, bytes_utf_8.Length)];
            Encoding.UTF8.GetChars(bytes_utf_8, 0, bytes_utf_8.Length, char_utf_8, 0);
            var string_utf_8 = new string(char_utf_8).ToJObject();
            Console.WriteLine(string_utf_8);
            //var bytes_gb2312 = Encoding.Convert(
            //        Encoding.GetEncoding("GB2312"),
            //        Encoding.UTF8,
            //        string_utf8.ToBytes(Encoding.UTF8));
            //var chars = new char[Encoding.UTF8.GetCharCount(bytes_gb2312, 0, bytes_gb2312.Length)];
            //Encoding.UTF8.GetChars(bytes, 0, bytes.Length, chars, 0);
            //var result = new string(chars).ToJObject();

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
