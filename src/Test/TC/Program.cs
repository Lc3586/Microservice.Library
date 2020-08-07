using TC.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Library.Extension;
using Library.ConsoleTool;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using McMaster.Extensions.CommandLineUtils;
using System.Reflection;

namespace TC
{
    /// <summary>
    /// 测试控制台
    /// <para>LCTR 2019-05-21</para>
    /// </summary>
    class Program
    {
        /// <summary>
        /// 程序状态
        /// </summary>
        public static ProgramState _ProgramState = ProgramState.Standby;

        /// <summary>
        /// 配置文件
        /// </summary>
        public static IConfiguration Configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json").Build();

        /// <summary>
        /// 命令行配置
        /// </summary>
        public static CommandConfig _CommandConfig;

        /// <summary>
        /// 主函数
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns></returns>
        static void Main(string[] args)
        {
            Console.Title = "TC";

            if (!Init())
                return;

            if (args.Length > 0 && args[0] == "manual")
                goto manual;

            var app = new CommandLineApplication(true);

            app.VersionOption("--version|-v", _CommandConfig.Version);
            app.HelpOption("-h|--help");
            app.Description = _CommandConfig.Description;

            app.OnExecute(() =>
            {
                app.ShowHelp();
                return 0;
            });

            _CommandConfig.Modulars.OrderBy(modular => modular.Sort).ForEach(modular => app.Command(modular.Name, command =>
            {
                command.HelpOption("-h|--help");
                command.Description = modular.Description;

                var Argument = command.Argument("method", "指定要执行的方法");

                List<CommandOption> Options = new List<CommandOption>();
                Dictionary<string, Arg> Options_arg = new Dictionary<string, Arg>();
                modular.Args.OrderBy(o => o.Sort).ForEach(arg =>
                {
                    var option = command.Option(arg.Name, arg.Description, arg.Type_enum);
                    Options.Add(option);
                    Options_arg.Add(option.LongName, arg);
                });
                command.OnExecute(() => Handler(command.Name, Argument.Value, Options.Select(o => new Arg_internal()
                {
                    Name = o.LongName,
                    Method = Options_arg[o.LongName].Method,
                    Value = o.OptionType == CommandOptionType.MultipleValue ?
                    o.Values?.Select(v => v.ConvertToAny(Convert.ChangeType(Options_arg[o.LongName].Default, Options_arg[o.LongName].DataType_Type), Options_arg[o.LongName].DataType_Type)).ToList() :
                    o.Value().ConvertToAny(Convert.ChangeType(Options_arg[o.LongName].Default, Options_arg[o.LongName].DataType_Type), Options_arg[o.LongName].DataType_Type)
                }).ToList()));
            }));

            app.Execute(args);
            return;

            manual:
            "Test Console Manual mode startup!".ConsoleWrite(ConsoleColor.White, "info");
            do
            {
                "输入指令\n".ConsoleWrite(ConsoleColor.White);
                GetParam(Console.ReadLine(), out string modular, out string method, out List<Arg_internal> _args);
                _ProgramState = Handler(modular, method, _args);

            } while (_ProgramState != ProgramState.Exit);
        }

        /// <summary>
        /// 初始化
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        private static bool Init()
        {
            try
            {
                _CommandConfig = Configuration.GetSection("Command").Get<CommandConfig>();
                return true;
            }
            catch (Exception ex)
            {
                "Test Console Init Error!".ConsoleWrite(ConsoleColor.Red, "error");
                ex.ConsoleWrite(ConsoleColor.Red, "error");
                return false;
            }
        }

        /// <summary>
        /// 获取命令参数
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="modular">输出：模块</param>
        /// <param name="method">输出：方法</param>
        /// <param name="args">输出：参数</param>
        private static void GetParam(string input, out string modular, out string method, out List<Arg_internal> args)
        {
            modular = input.Substring(0, input.IndexOfN2L(" "));
            input = input.Substring(modular.Length).TrimStart(' ');
            if (input == "-h" || input == "--help")
            {
                method = input;
                args = null;
                return;
            }
            method = input.IndexOf('-') == 0 ? string.Empty : input.Substring(0, input.IndexOfN2L(" "));
            var _args = input.Length > 0 ? (method == string.Empty ? input : input.Substring(method.Length)) : string.Empty;
            args = Regex.Matches(_args + " ", @"-(.*?)[\s\:\=](.*?)\s").Where(o => o.Groups != null && o.Groups.Count >= 2).Select(o => new Arg_internal() { Name = o.Groups[1].Value, Value = o.Groups[2].Value }).ToList();
        }

        /// <summary>
        /// 处理命令
        /// </summary>
        /// <param name="modular">模块</param>
        /// <param name="method">方法</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static ProgramState Handler(string modular, string method, List<Arg_internal> args)
        {
            try
            {

                switch (modular)
                {
                    case "exit":
                        "Exit!".ConsoleWrite(ConsoleColor.White, "info");
                        return ProgramState.Exit;
                    case "-h":
                    case "--help":
                        _CommandConfig.Description.ConsoleWrite(ConsoleColor.Cyan, "help");
                        goto end;
                }

                var Modular = _CommandConfig.Modulars.FirstOrDefault(o => o.Name == modular);
                if (Modular == null)
                    goto mismatch_modular;

                if (method == "-h" || method == "--help")
                {
                    Modular.Description.ConsoleWrite(ConsoleColor.Cyan, "help");
                    goto end;
                }

                var Method = Modular.Methods.FirstOrDefault(o => o.Name == method);
                if (Method == null)
                    goto mismatch_method;

                var assembly = Modular.Path.IsNullOrEmpty() ? Assembly.GetExecutingAssembly() : Assembly.LoadFile($"{AppContext.BaseDirectory}\\{Modular.Path}");

                var obj = Method.Static ? null : assembly.CreateInstance(Method.TypeName, true, BindingFlags.Default, null, Method.IConfig ? new object[] { Configuration } : null, null, null);

                var Params = args.Where(o => o.Method_match.Contains(method)).Select(o => o.Value).ToArray();

                if (Method.Async)
                    (assembly.GetType(Method.TypeName).GetMethod(Method.SpecifiedName).Invoke(obj, Params) as Task).Wait();
                else
                    assembly.GetType(Method.TypeName).GetMethod(Method.SpecifiedName).Invoke(obj, Params);
                goto end;

                mismatch_modular:
                "无效指令!\t输入 -h|--help 获取帮助信息".ConsoleWrite(ConsoleColor.Yellow, "warn");
                goto end;
                mismatch_method:
                $"无效指令!\t输入 {modular} -h|--help 获取帮助信息".ConsoleWrite(ConsoleColor.Yellow, "warn");

                end:
                return ProgramState.Standby;
            }
            catch (Exception ex)
            {
                "Test Console Handler Error!".ConsoleWrite(ConsoleColor.Red, "error");
                ex.ConsoleWrite(ConsoleColor.Red, "error");
                return ProgramState.Error;
            }
        }


    }
}
