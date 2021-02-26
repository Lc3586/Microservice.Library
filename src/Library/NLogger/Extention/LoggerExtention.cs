using Microservice.Library.Extension;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Microservice.Library.NLogger.Extention
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public static class LoggerExtention
    {
        /// <summary>
        /// 写入日志到本地TXT文件
        /// 注：日志文件名为"A_log.txt",目录为根目录
        /// </summary>
        /// <param name="log">日志内容</param>
        public static void WriteLog_LocalTxt(this string log)
        {
            Task.Run(() =>
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "A_log.txt");
                string logContent = $"{DateTime.Now.ToCstTime().ToString("yyyy-MM-dd HH:mm:ss")}:{log}\r\n";
                File.AppendAllText(filePath, logContent);
            });
        }
    }
}
