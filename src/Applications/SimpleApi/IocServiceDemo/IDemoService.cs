using System;
using System.Collections.Generic;
using System.Text;

namespace IocServiceDemo
{
    /// <summary>
    /// 服务接口类
    /// </summary>
    public interface IDemoService
    {
        /// <summary>
        /// 更改
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        string Change(string value);
    }
}
