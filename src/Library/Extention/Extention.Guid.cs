﻿using System;

namespace Library.Extention
{
    public static partial class Extention
    {
        /// <summary>
        /// 转为有序的GUID
        /// 注：长度为50字符
        /// </summary>
        /// <param name="guid">新的GUID</param>
        /// <returns></returns>
        public static string ToSequentialGuid(this Guid guid)
        {
            var timeStr = (DateTime.Now.ToCstTime().Ticks / 10000).ToString("x8");
            var newGuid = $"{timeStr.PadLeft(13, '0')}-{guid}";

            return newGuid;
        }

        /// <summary>
        /// 生成主键
        /// </summary>
        /// <returns></returns>
        public static string GenerateKey(this Guid guid)
        {
            return guid.ToSequentialGuid().ToUpper();
        }
    }
}
