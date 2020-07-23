using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Internal;
using FreeSql;
using Library.ConsoleTool;
using Library.FreeSql.Annotations;
using Library.FreeSql.Application;
using Library.FreeSql.Extention;
using Library.FreeSql.Gen;
using Oracle.ManagedDataAccess.Client;

namespace EmptyConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var orm = new FreeSqlGenerator(new FreeSqlGenOptions
            {
                FreeSqlGeneratorOptions = new FreeSqlGeneratorOptions
                {
                    ConnectionString = "user id=SAFETYEDU2;password=sedu#2019;data source=//122.112.150.191:1521/ORCL;Pooling=true;Min Pool Size=1;",
                    DatabaseType = DataType.Oracle,
                    LazyLoading = true,

                    MonitorCommandExecuting = (cmd) =>
                    {
                        Console.WriteLine(cmd.CommandText);
                    },
                    MonitorCommandExecuted = (cmd, log) =>
                    {
                        Console.WriteLine($"命令 {cmd},日志 {log}.");
                    },
                    HandleCommandLog = (content) =>
                    {
                        Console.WriteLine(content);
                    },
                }
            }).GetFreeSql();

            var response = new A();

            var rows = orm.Ado.ExecuteStoredProcedureWithModels(null, "PROC_WG2008",new B(), response);

            var member_F = typeof(A).GetMember("Field")?.FirstOrDefault(m => m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property);


            var member_P = typeof(A).GetMember("Property")?.FirstOrDefault(m => m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property);

            var value_F = member_F.GetMemberValue(typeof(A));
            var value_P = member_P.GetMemberValue(typeof(A));

            var model = new A { Property_ = "666" };

            var member_PP = typeof(A).GetMember("Property_")?.FirstOrDefault(m => m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property);

            var value_PP = member_PP.GetMemberValue(model);


            var mnc = System.Environment.UserDomainName;
            mnc = System.Environment.UserName;
            try
            {
                var process = Process.Start(new ProcessStartInfo("E:\\elasticsearch\\elasticsearch-7.2.0\\bin\\elasticsearch-service.bat", "remove")
                {
                    //Arguments = "remove",
                    //UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                });

                process.WaitForExit();

                process.Close();

                ConnectionOptions options = new ConnectionOptions();
                options.Password = "ADMINISTRATOR";
                options.Username = "9527?kwh*";
                //options.Impersonation = ImpersonationLevel.Identify;

                ManagementScope scope = new ManagementScope("\\\\192.168.1.113\\root\\cimv2", options);

                try
                {
                    scope.Connect();
                }
                catch (COMException ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }

                var svc = new ManagementObjectSearcher(
                       scope,
                       new ObjectQuery("Select Status,State from Win32_Service  where Name='elasticsearch-service-7_2_0-x64'"))
                           .Get()
                           .GetEnumerator();

                if (svc.MoveNext())
                {
                    var status = svc.Current["Status"].ToString();
                    var state = svc.Current["State"].ToString();
                    Console.WriteLine("service status {0}", status);
                    // if not running, StartService
                    if (!String.Equals(state, "running", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ((ManagementObject)svc.Current).InvokeMethod("StartService", new object[] { });
                    }
                }
                else
                {
                    Console.WriteLine("service not found");
                }

                var service = new ServiceController("elasticsearch-service-7_2_0-x64");
                try
                {
                    var s = service.Status;
                }
                catch (InvalidOperationException ex)
                {

                }
            }
            catch (Exception ex)
            {
                var t = ex.GetType();
                throw;
            }

            var tcs = new TaskCompletionSource<bool>();

            async void T()
            {
                await "Hello \nWorld!Hello \nWorld!Hello \nWorld!".ConsoleWriteAsync(-1, ConsoleColor.White, null, true, 2);
                await tcs.Task;
            }

            T();

            while (true)
            {
                Task.Delay(1000).GetAwaiter().GetResult();
                break;
            }

            tcs.SetResult(true);

            var input = Extension.ReadInput();
            var flag = Convert.ToBoolean("0");
            var ca = Encoding.ASCII.GetBytes(new char[] { '李' });
            ca = Encoding.ASCII.GetBytes(new char[] { 'l' });
            Console.Write("李l");
            Console.Write('\u0008');
            Console.Write(' ');
            Console.Write('\u0008');
            Console.Write('\u0008');
            Console.Write(' ');
            Console.Write('\u0008');

            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(80, 80);
            Console.SetWindowSize(40, 20);

            Console.SetBufferSize(800, 800);
            Console.SetWindowSize(200, 50);
            var w = Console.LargestWindowWidth;
            var h = Console.LargestWindowHeight;
            var bar = new ProgressBar(2);
            bar.RollBack(1, 0);
            bar.Successes(1m / 3, "步骤 1/3 : 正在创建客户端", 0);
            bar.Successes(1m / 10, 1);
            bar.Successes(2m / 10, 1);
            bar.Successes(10m / 10, 1);
            bar.Normal(0, "0 %", 1);
            bar.Successes(2m / 10, $"{(1m / 10 * 100):f2} %", 1);
            bar.RollBack(10, "10 %", 1);

            var dp = new DotPlot(8, 2, new List<DotPlot.DotColor>() {
                new DotPlot.DotColor { Title = "Black", Value = 0, Color = ConsoleColor.Black },
                new DotPlot.DotColor { Title = "White", Value = 1, Color = ConsoleColor.White },
                new DotPlot.DotColor { Title = "DarkGray", Value = 2, Color = ConsoleColor.DarkGray },
                new DotPlot.DotColor { Title = "Gray", Value = 3, Color = ConsoleColor.Gray },
                new DotPlot.DotColor { Title = "DarkGreen", Value = 4, Color = ConsoleColor.DarkGreen },
                new DotPlot.DotColor { Title = "Green", Value = 5, Color = ConsoleColor.Green },
                new DotPlot.DotColor { Title = "DarkYellow", Value = 6, Color = ConsoleColor.DarkYellow },
                new DotPlot.DotColor { Title = "Yellow", Value = 7, Color = ConsoleColor.Yellow },
                new DotPlot.DotColor { Title = "DarkBlue", Value = 8, Color = ConsoleColor.DarkBlue },
                new DotPlot.DotColor { Title = "Blue", Value = 9, Color = ConsoleColor.Blue },
                new DotPlot.DotColor { Title = "DarkCyan", Value = 10, Color = ConsoleColor.DarkCyan },
                new DotPlot.DotColor { Title = "Cyan", Value = 11, Color = ConsoleColor.Cyan },
                new DotPlot.DotColor { Title = "DarkMagenta", Value = 12, Color = ConsoleColor.DarkMagenta },
                new DotPlot.DotColor { Title = "Magenta", Value = 13, Color = ConsoleColor.Magenta },
                new DotPlot.DotColor { Title = "DarkRed", Value = 14, Color = ConsoleColor.DarkRed },
                new DotPlot.DotColor { Title = "Red", Value = 15, Color = ConsoleColor.Red }
            }, 1, "标题");
            dp.Show();

            dp.SetValue(0, 0, 0);
            dp.SetValue(0, 1, 1);
            dp.SetValue(1, 0, 2);
            dp.SetValue(1, 1, 3);
            dp.SetValue(2, 0, 4);
            dp.SetValue(2, 1, 5);
            dp.SetValue(3, 0, 6);
            dp.SetValue(3, 1, 7);
            dp.SetValue(4, 0, 8);
            dp.SetValue(4, 1, 9);
            dp.SetValue(5, 0, 10);
            dp.SetValue(5, 1, 11);
            dp.SetValue(6, 0, 12);
            dp.SetValue(6, 1, 13);
            dp.SetValue(7, 0, 14);
            dp.SetValue(7, 1, 15);

            dp.Update();

            dp.SetValue(10, 6, 2, true);

            var a = typeof(A).GetProperty("SA");
            var b = a.GetValue(null, null);
            var field = typeof(A).GetField("SA", BindingFlags.Static & BindingFlags.NonPublic);
            Console.WriteLine(field.GetValue(null));

            Console.WriteLine(typeof(E.EA[]).Assembly.GetType("E.EA").FullName);

            foreach (var prop in typeof(A).GetProperties())
            {
                Console.WriteLine($"Name:{prop.Name} \t" +
                    $"PropertyType:{prop.PropertyType} \t" +
                    $"Assembly:{prop.PropertyType.Assembly} \t" +
                    $"GenericTypeArguments:{string.Join<Type>(',', prop.PropertyType.GenericTypeArguments)} \t" +
                    $"IsGenericType:{prop.PropertyType.IsGenericType} \t" +
                    $"IsArray:{prop.PropertyType.IsArray} \t");
            }
        }

        public class A
        {
            /// <summary>
            /// 返回消息代码
            /// </summary>
            /// <remarks>0表示返回成功</remarks>
            [DbParameter(Name = "ReturnValue1", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Int32, Direction = ParameterDirection.Output)]
            public string ret_code { get; set; }

            /// <summary>
            /// 返回信息描述
            /// </summary>
            /// <remarks>允许传空值</remarks>
            [DbParameter(Name = "ReturnValue2", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Varchar2, Size = 50, Direction = ParameterDirection.Output)]
            public string ret_info { get; set; }

            public static string Field = "666";

            public static string Property { get; set; } = "666";

            public string Property_ { get; set; }

            public static List<object> SA { get { return new List<object>() { 6, 6, 6 }; } }

            //public object F0 { get; set; }
            //public int F1 { get; set; }
            //public string F2 { get; set; }
            //public DateTime F3 { get; set; }
            //public M3 F4 { get; set; }

            //public B X0 { get; set; }
            //public B[] X1 { get; set; }
            ////public EA[] X2 { get; set; }
            //public List<B> X3 { get; set; }
            //public IEnumerable<B> X4 { get; set; }

            //public object[] Y0 { get; set; }
            //public int[] Y1 { get; set; }
            //public string[] Y2 { get; set; }
            //public DateTime[] Y3 { get; set; }

            //public List<object> Z0 { get; set; }
            //public List<int> Z1 { get; set; }
            //public IEnumerable<int> Z2 { get; set; }
            //public List<string> Z3 { get; set; }
            //public List<DateTime> Z4 { get; set; }
            //public IEnumerable<DateTime> Z5 { get; set; }

            //public void M0()
            //{

            //}

            //public class M2
            //{

            //}

            //public enum M3
            //{

            //}

            //public interface M4
            //{

            //}
        }

        public class B
        {
            /// <summary>
            /// 医院代码
            /// </summary>
            [DbParameter(Name = "value_yybh", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input)]
            public string org_code { get; set; }

            /// <summary>
            /// 分院编号
            /// </summary>
            [DbParameter(Name = "value_fybh", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input)]
            public string fybh { get; set; }

            /// <summary>
            /// 挂号日期
            /// </summary>
            /// <remarks>yyyy-MM-dd</remarks>
            [DbParameter(Name = "value_ghrq", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Date, Direction = ParameterDirection.Input)]
            public DateTime ghrq { get; set; }

            /// <summary>
            /// 挂号科室代码
            /// </summary>
            /// <remarks>医院本地科室代码</remarks>
            [DbParameter(Name = "value_ksdm", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input)]
            public string ghksdm { get; set; }

            /// <summary>
            /// 医生编号
            /// </summary>
            /// <remarks>医院代码(8位)+医生本地代码</remarks>
            [DbParameter(Name = "value_ysgh", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input)]
            public string ghysbh2 { get; set; }

            /// <summary>
            /// 门诊时间
            /// </summary>
            /// <remarks>A=上午/P=下午/F=全天/N=夜间</remarks>
            [DbParameter(Name = "value_zblb", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input)]
            public string mzsj { get; set; }

            /// <summary>
            /// 预约号子
            /// </summary>
            [DbParameter(Name = "value_yyxh", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input)]
            public string num { get; set; }

            /// <summary>
            /// 保险号
            /// </summary>
            /// <remarks>医保号、农保号、通用就诊卡明码</remarks>
            [DbParameter(Name = "value_bxno", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input)]
            public string gfno { get; set; }

            /// <summary>
            /// 支付方式
            /// </summary>
            /// <remarks>1短信 2网银 3其他</remarks>
            [DbParameter(Name = "value_zffs", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input)]
            public string zffs { get; set; }

            /// <summary>
            /// 收款实体
            /// </summary>
            /// <remarks>这里是收款银行代码、短信运营商代码、或其他收款方代码</remarks>
            [DbParameter(Name = "value_zfyh", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Int32, Direction = ParameterDirection.Input)]
            public string zfyh { get; set; }

            /// <summary>
            /// 支付流水号
            /// </summary>
            [DbParameter(Name = "value_zflsh", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input)]
            public string zflsh { get; set; }

            /// <summary>
            /// 支付金额
            /// </summary>
            [DbParameter(Name = "value_zfje", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Int32, Direction = ParameterDirection.Input)]
            public decimal zfje { get; set; }

            /// <summary>
            /// 支付时间
            /// </summary>
            /// <remarks>yyyy-MM-dd HH:mm:ss</remarks>
            [DbParameter(Name = "value_zfsj", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Date, Direction = ParameterDirection.Input)]
            public DateTime zfsj { get; set; }

            /// <summary>
            /// 挂号来源
            /// </summary>
            /// <remarks>20预约挂号平台</remarks>
            [DbParameter(Name = "value_ghly", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input)]
            public string ghly { get; set; }

            /// <summary>
            /// 预约流水号
            /// </summary>
            /// <remarks>可唯一识别预约记录</remarks>
            [DbParameter(Name = "value_yylsh", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input)]
            public string yylsh { get; set; }

            /// <summary>
            /// 会员号
            /// </summary>
            /// <remarks>平台会员标识</remarks>
            [DbParameter(Name = "value_personid", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input)]
            public string personid { get; set; }

            /// <summary>
            /// 手机号
            /// </summary>
            [DbParameter(Name = "value_sjh", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input)]
            public string sjh { get; set; }

            /// <summary>
            /// 验证码
            /// </summary>
            [DbParameter(Name = "value_ghyzm", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input)]
            public string ghyzm { get; set; }

            /// <summary>
            /// 平台来源分点(新增)
            /// </summary>
            /// <remarks>数据源来自R301的平台来源分点代码PTLYFDDM</remarks>
            [DbParameter(Name = "value_ptlyfd", DataType = FreeSql.DataType.Oracle, DbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input)]
            public string ptlyfd { get; set; }
        }
    }
}

namespace E
{
    public class EA
    {

    }
}