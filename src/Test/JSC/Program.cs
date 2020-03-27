using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JSC
{
    /// <summary>
    /// JavaScript客户端身份验证服务使用示例程序
    /// <para>LCTR 2019-05-31</para>
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "JSC";
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
