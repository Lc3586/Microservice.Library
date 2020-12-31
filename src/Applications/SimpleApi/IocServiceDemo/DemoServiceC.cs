using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IocServiceDemo
{
    public class DemoServiceC : IDemoService
    {
        /// <summary>
        /// 使用依赖注入的实现类一定要有公共的无参构造函数
        /// </summary>
        public DemoServiceC()
        {

        }

        public string Change(string value)
        {
            var random = new Random();
            return $"C : {string.Join("", value.Select(c => new[] { true, false }[random.Next(0, 1)] ? char.ToUpper(c) : char.ToLower(c)))}";
        }
    }
}
