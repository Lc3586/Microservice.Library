using ImageProcessorCore;
using Microservice.Library.File.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Library.File
{
    /// <summary>
    /// 视频文件帮助类
    /// </summary>
    public static class VideoHelper
    {
        /// <summary>
        /// 获取程序
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetEXE(string name = "ffmpeg")
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return $"{name}.exe";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return $"/usr/bin/{name}";
            //else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            //    return name;
            else
                throw new ApplicationException($"当前操作系统不支持使用{name}.");
        }

        /// <summary>
        /// 视频截图
        /// </summary>
        /// <remarks>
        /// <para>在windows系统下运行时，需要将ffmpeg.exe文件放置于应用根目录下。</para>
        /// <para>在linux系统下运行时，需要安装ffmpeg，读取目录为(/usr/bin/ffmpeg)。</para>
        /// </remarks>
        /// <param name="videofile">视频文件绝对路径</param>
        /// <param name="imagefile">截图文件存储路径</param>
        /// <param name="time">指定时间</param>
        /// <param name="quality">图片质量[2-31]</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        public static async Task Screenshot(this string videofile, string imagefile, TimeSpan time, int quality = 31, int? width = null, int? height = null)
        {
            if (!videofile.Exists())
                throw new ApplicationException($"视频文件不存在[{videofile}].");

            string path = GetEXE();

            var arguments = $" -ss {time} -i {videofile} -q:v {quality} -frames:v 1 -an -y -f mjpeg {imagefile}";

            if (width.HasValue && height.HasValue)
                arguments += $" -s {width.Value}*{height.Value}";

            using (var process = new Process())
            {
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
#if DEBUG
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.StandardErrorEncoding = Encoding.UTF8;
#else
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.RedirectStandardError = false;
#endif
                process.StartInfo.FileName = path;
                process.StartInfo.Arguments = arguments;
                process.Start();
#if DEBUG
                var output = await process.StandardOutput.ReadToEndAsync();
                var error = await process.StandardError.ReadToEndAsync();
#endif
                process.WaitForExit();
#if DEBUG
                Console.Write(output);
                Console.Write(error);
#endif
            }
        }

        /// <summary>
        /// 获取视频信息
        /// </summary>
        /// <remarks>
        /// <para>在windows系统下运行时，需要将ffprobe.exe文件放置于应用根目录下。</para>
        /// <para>在linux系统下运行时，需要安装ffmpeg，读取目录为(/usr/bin/ffprobe)。</para>
        /// </remarks>
        /// <param name="videofile">视频文件绝对路径</param>
        /// <param name="format">获取有关输入多媒体流的容器格式的信息</param>
        /// <param name="streams">获取有关输入多媒体流中包含的每个媒体流的信息</param>
        /// <param name="chapters">获取有关以该格式存储的章节的信息</param>
        /// <param name="programs">获取有关程序及其输入多媒体流中包含的流的信息</param>
        /// <param name="version">获取与程序版本有关的信息、获取与库版本有关的信息、获取与程序和库版本有关的信息</param>
        /// <returns></returns>
        public static async Task<VideoInfo> GetVideoInfo(this string videofile, bool format = true, bool streams = true, bool chapters = false, bool programs = false, bool version = false)
        {
            if (!videofile.Exists())
                throw new ApplicationException($"视频文件不存在[{videofile}].");

            string path = GetEXE("ffprobe");

            var arguments = $" -i {videofile} -print_format json -show_data";

            if (format)
                arguments += " -show_format";
            if (streams)
                arguments += " -show_streams";
            if (chapters)
                arguments += " -show_chapters";
            if (programs)
                arguments += " -show_programs";
            if (version)
                arguments += " -show_versions";

            using (var process = new Process())
            {
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
#if DEBUG
                process.StartInfo.RedirectStandardError = true;
#else
                process.StartInfo.RedirectStandardError = false;
                process.StartInfo.StandardErrorEncoding = Encoding.UTF8;
#endif
                process.StartInfo.FileName = path;
                process.StartInfo.Arguments = arguments;
                process.Start();

                var output = await process.StandardOutput.ReadToEndAsync();
#if DEBUG
                var error = await process.StandardError.ReadToEndAsync();
#endif
                process.WaitForExit();
#if DEBUG
                Console.Write(output);
                Console.Write(error);
#endif
                return JsonConvert.DeserializeObject<VideoInfo>(output);
            }
        }
    }
}
