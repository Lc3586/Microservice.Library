using System;
using System.Collections.Generic;
using System.Text;

namespace IocServiceDemo
{
    /// <summary>
    /// 服务配置
    /// </summary>
    public class DemoServiceOptions
    {
        public DemoServiceOptions()
        {
            Threshold = 1;
            DisableType = new List<Type>();
        }

        /// <summary>
        /// 服务切换阈值
        /// </summary>
        public int Threshold { get; set; }

        /// <summary>
        /// 禁用的类型
        /// </summary>
        public List<Type> DisableType { get; set; }
    }
}
