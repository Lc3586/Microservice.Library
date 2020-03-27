using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicDLL
{
    public class Test
    {

        public void A(string value)
        {
            Console.WriteLine("Test.A");
            Console.WriteLine(value);
        }

        public static void B(string value)
        {
            Console.WriteLine("Test.B");
            Console.WriteLine(value);
        }
    }
}
