﻿using System.IO;
using System.Text;

namespace Library.Extension
{
    public static partial class Extension
    {
        /// <summary>
        /// 将流Stream转为byte数组
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadToBytes(this Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);

            return bytes;
        }

        /// <summary>
        /// 将流读为字符串
        /// 注：使用默认编码
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns></returns>
        public static string ReadToString(this Stream stream)
        {
            string resStr = string.Empty;
            stream.Seek(0, SeekOrigin.Begin);
            resStr = new StreamReader(stream).ReadToEnd();
            stream.Seek(0, SeekOrigin.Begin);

            return resStr;
        }

        /// <summary>
        /// 将流读为字符串
        /// 注：使用指定编码
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="encoding">指定编码</param>
        /// <returns></returns>
        public static string ReadToString(this Stream stream, Encoding encoding)
        {
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            string resStr = string.Empty;
            resStr = new StreamReader(stream, encoding).ReadToEnd();

            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            return resStr;
        }
    }
}
