using Microservice.Library.Extension;
using Microservice.Library.ConsoleTool;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using T4CAGC.Models;
using T4CAGC.Template;
using Microservice.Library.DataAccess;

namespace T4CAGC.Handler
{
    public class Generate
    {
        /// <summary>
        /// 验证服务器配置
        /// </summary>
        public static IdentityConfig _IdentityConfig;

        public static string[] SupportLanguage = { "C#", "javascript" };

        public Generate()
        {

        }

        public Generate(IConfiguration Configuration)
        {
            _IdentityConfig = Configuration.GetSection("Identity").Get<IdentityConfig>();

            if (_IdentityConfig == null)
            {
                "Config Error!".ConsoleWrite(ConsoleColor.Red, "error");
                throw new Exception("Config Error!");
            }
        }

        public void GenerateEntityModel(GenerateConfig config)
        {
            if (config.Language.IsNullOrEmpty())
                throw new Exception("Language must be certainty");
            if (!config.DbType_Type.HasValue)
                throw new Exception("DbType is invalid");
            if (!config.Database.Any_Ex())
                throw new Exception("Specify at least one Database");
            if (!config.OutputPath.Any_Ex())
                config.OutputPath = new List<string>() { $"{AppContext.BaseDirectory}Output" };
            foreach (var database in config.Database)
            {
                var dbhelper = DbHelperFactory.GetDbHelper(config.DbType_Type.Value, $"{config.DbConnection};database={database};");

                var tables = dbhelper.GetDbTableInfo(database, config.Table, config.TableIgnore, true);

                foreach (var table in tables)
                {
                    Model_Entity temple = new Model_Entity(config, table);
                    string fileName = table.ClassName + ".cs";

                    config.OutputPath.ForEach(op =>
                    {
                        if (!Directory.Exists(op))
                            Directory.CreateDirectory(op);
                        File.WriteAllText($"{op}\\{fileName}", temple.TransformText(), Encoding.UTF8);
                    });
                }
            }
        }
    }
}
