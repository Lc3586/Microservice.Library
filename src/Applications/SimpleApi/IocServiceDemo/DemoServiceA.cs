using System;
using System.Collections.Generic;
using System.Text;

namespace IocServiceDemo
{
    /// <summary>
    /// 服务实现类
    /// </summary>
    public class DemoServiceA : IDemoService
    {
        /// <summary>
        /// 使用依赖注入的实现类一定要有公共的无参构造函数
        /// </summary>
        public DemoServiceA()
        {

        }

        public string Change(string value)
        {
            return $"A : {value?.ToUpper()}";
        }
    }
}
