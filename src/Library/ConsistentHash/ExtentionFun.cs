using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.ConsistentHash
{
    public static class ExtentionFun
    {
        /// <summary>
        /// 转为MurmurHash
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static uint ToMurmurHash(this string str)
        {
            return MurmurHash2.Hash(Encoding.UTF8.GetBytes(str));
        }
    }
}
