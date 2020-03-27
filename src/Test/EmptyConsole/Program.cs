using System;
using System.Collections.Generic;
using System.Reflection;

namespace EmptyConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

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