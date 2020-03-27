using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicDLL
{
    public class Test_Constructor
    {

        public Test_Constructor()
        {

        }

        public Test_Constructor(IConfiguration Configuration)
        {

        }

        public void A(string value)
        {
            Console.WriteLine("Test_Constructor.A");
            Console.WriteLine(value);
        }

        public static void B(string value)
        {
            Console.WriteLine("Test_Constructor.B");
            Console.WriteLine(value);
        }
    }
}
