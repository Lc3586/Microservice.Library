using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.File.Model
{
    /// <summary>
    /// 视频信息
    /// </summary>
    public class VideoInfo
    {
        /// <summary>
        /// 媒体流
        /// </summary>
        public List<StreamInfo> Streams { get; set; }

        /// <summary>
        /// 格式
        /// </summary>
        public FormatInfo Format { get; set; }

        /// <summary>
        /// 章节
        /// </summary>
        public List<ChapterInfo> Chapters { get; set; }

        /// <summary>
        /// 程序
        /// </summary>
        public List<ProgramInfo> Programs { get; set; }

        /// <summary>
        /// 程序版本
        /// </summary>
        public ProgramVersionInfo Program_Version { get; set; }

        /// <summary>
        /// 库版本
        /// </summary>
        public List<LibraryVersionInfo> Library_Versions { get; set; }
    }
}
