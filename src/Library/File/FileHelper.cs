using Microservice.Library.Extension;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Microservice.Library.File
{
    /// <summary>
    /// 文件操作帮助类
    /// </summary>
    public static class FileHelper
    {
        #region 读操作

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path">文件目录</param>
        /// <returns></returns>
        public static bool Exists(this string path)
        {
            return System.IO.File.Exists(path);
        }

        /// <summary>
        /// 获取当前程序根目录
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDir()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        #endregion

        #region 写操作

        /// <summary>
        /// 输出字符串到文件
        /// 注：使用系统默认编码;若文件不存在则创建新的,若存在则覆盖
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="path">文件路径</param>
        public static void WriteTxt(this string content, string path)
        {
            WriteTxt(content, path, null, null);
        }

        /// <summary>
        /// 输出字符串到文件
        /// 注：使用自定义编码;若文件不存在则创建新的,若存在则覆盖
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">编码</param>
        public static void WriteTxt(this string content, string path, Encoding encoding)
        {
            WriteTxt(content, path, encoding, null);
        }

        /// <summary>
        /// 输出字符串到文件
        /// 注：使用自定义模式,使用默认编码
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="path">文件路径</param>
        /// <param name="fileModel">输出方法</param>
        public static void WriteTxt(this string content, string path, FileMode fileModel)
        {
            WriteTxt(content, path, null, fileModel);
        }

        /// <summary>
        /// 输出字符串到文件
        /// 注：使用自定义编码以及写入模式
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="fileModel">写入模式</param>
        public static void WriteTxt(this string content, string path, Encoding encoding, FileMode fileModel)
        {
            WriteTxt(content, path, encoding, (FileMode?)fileModel);
        }

        /// <summary>
        /// 输出字符串到文件
        /// 注：使用自定义编码以及写入模式
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="fileModel">写入模式</param>
        private static void WriteTxt(this string content, string path, Encoding encoding, FileMode? fileModel)
        {
            CheckDirectory(path);

            if (encoding == null)
                encoding = Encoding.Default;
            if (fileModel == null)
                fileModel = FileMode.Create;

            using (FileStream fileStream = new FileStream(path, fileModel.Value))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, encoding))
                {
                    streamWriter.Write(content);
                    streamWriter.Flush();
                }
            }
        }

        /// <summary>
        /// 检验目录，若目录已存在则不变
        /// </summary>
        /// <param name="path">目录位置</param>
        public static void CheckDirectory(this string path)
        {
            if (path.Contains("\\"))
                Directory.CreateDirectory(GetPathDirectory(path));
        }

        /// <summary>
        /// 输出日志到指定文件
        /// </summary>
        /// <param name="msg">日志消息</param>
        /// <param name="path">日志文件位置（默认为 log.txt）</param>
        public static void WriteLog(this string msg, string path = @"log.txt")
        {
            string content = $"{DateTime.Now.ToCstTime():yyyy-MM-dd HH:mm:ss}:{msg}";

            WriteTxt(content, $"{GetCurrentDir()}{path}");
        }

        /// <summary>
        /// 获取文件位置中的目录位置（不包括文件名）
        /// </summary>
        /// <param name="path">文件位置</param>
        /// <returns></returns>
        public static string GetPathDirectory(this string path)
        {
            if (!path.Contains("\\"))
                return GetCurrentDir();
            string pattern = @"^(.*\\).*?$";
            Match match = Regex.Match(path, pattern);

            return match.Groups[1].ToString();
        }

        /// <summary>
        /// 获取文件字节数
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <returns></returns>
        public static long GetFileBytes(this string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                return fs.Length;
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <param name="unit">单位</param>
        /// <param name="precision">精度</param>
        /// <returns></returns>
        public static string GetFileSize(this string path, int unit = 1024, int precision = 2)
        {
            var length = GetFileBytes(path);
            return GetFileSize(length, unit, precision);
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="length">字节数</param>
        /// <param name="unit">单位</param>
        /// <param name="precision">精度</param>
        /// <returns></returns>
        public static string GetFileSize(this long length, int unit = 1024, int precision = 2)
        {
            if (length <= 0)
                return "0 KB";

            var formats = new string[] { "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

            for (int i = 0; i < formats.Length; i++)
            {
                var value = length / Math.Pow(unit, i + 1);

                if (value < unit)
                    return $"{Math.Round(value, precision)} {formats[i]}";
            }

            return $"{Math.Round(length / Math.Pow(unit, formats.Length), precision)} {formats[formats.Length - 1]}";
        }


        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="source">源目录</param>
        /// <param name="destination">目标目录</param>
        /// <param name="overwrite">是否覆盖同名文件</param>
        /// <param name="remove">移除已复制了的源文件</param>
        public static void CopyTo(this string source, string destination, bool overwrite = true, bool remove = false)
        {
            var sourceDir = new DirectoryInfo(source);
            if (!sourceDir.Exists)
                return;

            sourceDir.GetFiles().ForEach(file =>
            {
                var dir = file.DirectoryName.Replace(source, destination);

                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var dfile = file.FullName.Replace(source, destination);

                if (!overwrite && System.IO.File.Exists(dfile))
                    return;

                System.IO.File.Copy(file.FullName, dfile, true);

                if (remove)
                    System.IO.File.Delete(file.FullName);
            });

            sourceDir.GetDirectories().ForEach(dir =>
            {
                dir.FullName.CopyTo(dir.FullName.Replace(source, destination), overwrite, remove);
            });

            if (remove && !sourceDir.GetFiles().Any())
                sourceDir.Delete();
        }

        #endregion
    }
}
