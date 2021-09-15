using System;

namespace Microservice.Library.File.Model
{
    /// <summary>
    /// 格式信息
    /// </summary>
    public class FormatInfo
    {
        /// <summary>
        /// 文件绝对路径
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// 输入视频的AVStream个数
        /// </summary>
        public int Nb_Streams { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Nb_Programs { get; set; }

        /// <summary>
        /// 格式名
        /// <para>半角逗号[,]分隔</para>
        /// </summary>
        public string Format_Name { get; set; }

        /// <summary>
        /// 格式名全称
        /// </summary>
        public string Format_Long_Name { get; set; }

        /// <summary>
        /// 首帧时间
        /// </summary>
        public double Start_Time { get; set; }

        /// <summary>
        /// 首帧时间
        /// </summary>
        public TimeSpan Start_Time_Convert { get { return TimeSpan.FromSeconds(Start_Time); } set { Start_Time = value.TotalSeconds; } }

        /// <summary>
        /// 时长(秒)
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public TimeSpan Duration_Convert { get { return TimeSpan.FromSeconds(Duration); } set { Duration = value.TotalSeconds; } }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 码率
        /// </summary>
        public long Bit_Rate { get; set; }

        /// <summary>
        /// 文件内容与文件拓展名匹配程度
        /// <para>100为最高分, 低于25分时文件拓展名可能被串改.</para>
        /// </summary>
        public int Probe_Score { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public FormatTagsInfo Tags { get; set; }
    }
}
