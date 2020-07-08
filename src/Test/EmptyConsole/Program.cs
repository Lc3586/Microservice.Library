using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Library.ConsoleTool;

namespace EmptyConsole
{
    class Program
    {
        static void Main(string[] args)
        {
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

        }
    }
}

namespace E
{
    public class EA
    {

    }
}