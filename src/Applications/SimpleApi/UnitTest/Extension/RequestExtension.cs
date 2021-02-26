using Microservice.Library.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Extension
{
    /// <summary>
    /// 请求拓展类
    /// </summary>
    public static class RequestExtension
    {
        /// <summary>
        /// 将参数写入地址
        /// </summary>
        /// <param name="path">地址 /aaa/{id}</param>
        /// <param name="paramters">参数</param>
        public static string SetParamters2Path(this string path, Dictionary<string, object> paramters)
        {
            var result = path;

            paramters.ForEach(p =>
            {
                result = result.Replace($"{p.Key}", p.Value.ToString());
            });

            return result;
        }
    }
}
