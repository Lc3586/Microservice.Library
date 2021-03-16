using Microservice.Library.Extension;
using Microservice.Library.OfficeDocuments;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TC
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var packages = "Microservice.Library.Baidu,Microservice.Library.BloomFilter,Microservice.Library.Cache,Microservice.Library.Configuration,Microservice.Library.ConsistentHash,Microservice.Library.ConsoleTool,Microservice.Library.Container,Microservice.Library.DataMapping,Microservice.Library.Elasticsearch,Microservice.Library.Extension,Microservice.Library.File,Microservice.Library.FreeSql,Microservice.Library.Http,Microservice.Library.NLogger,Microservice.Library.OfficeDocuments,Microservice.Library.OpenApi,Microservice.Library.SelectOption,Microservice.Library.Snowflake,Microservice.Library.Soap,Microservice.Library.WeChat,Microservice.Library.Chinese,Microservice.Library.Collection,Microservice.Library.DataAccess,Microservice.Library.DataRepository,Microservice.Library.DataRepository_DM,Microservice.Library.SampleAuthentication,Microservice.Library.SuperSocket"
                  .Split(',').ToList();

            var versions = "0.0.0-gf908f70c8d-alpha,0.0.0-gf58a743371-alpha,0.0.0-gf3bfaf332c-alpha,0.0.0-gbd97dc7971-alpha,0.0.0-gb04441fe1c-alpha,0.0.0-g7fb5453427-alpha,0.0.0-g48584721c1-alpha,0.0.0-g266fcfe23f-alpha,0.0.0-g12c826cc74-alpha,0.0.0-g004c4ebb96-alpha,0.0.0.20284-alpha-g3c4fe539e3"
                .Split(',').ToList();

            var apiKey = "oy2ms2w5fg2ymdtbksgqnau4vl5hqlduvgu4i7bxwq2mu4";

            packages.ForEach(o =>
            {
                versions.ForEach(p =>
                {
                    var process = Process.Start(new ProcessStartInfo("nuget", $"delete {o} {p}  -Source nuget.org -ApiKey {apiKey} -NonInteractive")
                    {
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Hidden
                    });

                    process.WaitForExit();

                    if (process.ExitCode != 0)
                        Console.WriteLine($"删除包{o}失败,返回码[{process.ExitCode}].");

                    process.Close();
                    process.Dispose();
                });
            });


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
