using Microservice.Library.Extension.Helper;
using Microservice.Library.File.Model;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace Microservice.Library.File
{
    /// <summary>
    /// 视频文件帮助类
    /// </summary>
    public static class VideoHelper
    {
        /// <summary>
        /// 视频截图
        /// </summary>
        /// <param name="videoFile">视频文件绝对路径</param>
        /// <param name="ffmpegFile">ffmpeg应用程序文件绝对路径</param>
        /// <param name="imageFile">截图文件存储路径</param>
        /// <param name="time">指定时间</param>
        /// <param name="quality">图片质量[2-31]</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        public static void Screenshot(this string videoFile, string ffmpegFile, string imageFile, TimeSpan time, int quality = 31, int? width = null, int? height = null)
        {
            if (!videoFile.Exists())
                throw new ApplicationException($"视频文件不存在[{videoFile}].");

            //string path = GetEXE();

            var ifDir = new FileInfo(imageFile).DirectoryName;
            if (!Directory.Exists(ifDir))
                Directory.CreateDirectory(ifDir);

            var arguments = $" -ss {time} -i \"{videoFile}\" -q:v {quality} -frames:v 1 -an -y -f mjpeg";

            if (width.HasValue && height.HasValue)
                arguments += $" -s {width.Value}x{height.Value} \"{imageFile}\"";
            else
                arguments += $" -s \"{imageFile}\"";

            var (output, error, exitCode) = ExecutableHelper.SimpleCall(ffmpegFile, arguments, null, null, Encoding.UTF8, Encoding.UTF8);

#if DEBUG
            Console.WriteLine(exitCode);
            Console.WriteLine(output);
            Console.WriteLine(error);
#endif

            if (exitCode != 0)
                throw new ApplicationException($"截图失败.{error}");
        }

        /// <summary>
        /// 获取视频信息
        /// </summary>
        /// <param name="videoFile">视频文件绝对路径</param>
        /// <param name="ffprobeFile">fffprobe应用程序文件绝对路径</param>
        /// <param name="format">获取有关输入多媒体流的容器格式的信息</param>
        /// <param name="streams">获取有关输入多媒体流中包含的每个媒体流的信息</param>
        /// <param name="chapters">获取有关以该格式存储的章节的信息</param>
        /// <param name="programs">获取有关程序及其输入多媒体流中包含的流的信息</param>
        /// <param name="version">获取与程序版本有关的信息、获取与库版本有关的信息、获取与程序和库版本有关的信息</param>
        /// <returns></returns>
        public static VideoInfo GetVideoInfo(this string videoFile, string ffprobeFile, bool format = true, bool streams = true, bool chapters = false, bool programs = false, bool version = false)
        {
            if (!videoFile.Exists())
                throw new ApplicationException($"视频文件不存在[{videoFile}].");

            //string path = GetEXE("ffprobe");

            var arguments = $" -i \"{videoFile}\" -print_format json -show_data";

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

            var (output, error, exitCode) = ExecutableHelper.SimpleCall(ffprobeFile, arguments, null, null, Encoding.UTF8, Encoding.UTF8);

#if DEBUG
            Console.WriteLine(exitCode);
            Console.WriteLine(output);
            Console.WriteLine(error);
#endif

            if (exitCode != 0)
                throw new ApplicationException($"获取视频信息失败. {error}");

            return JsonConvert.DeserializeObject<VideoInfo>(output);
        }
    }
}
