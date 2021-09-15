using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.File.Model
{
    /// <summary>
    /// 媒体流信息
    /// </summary>
    public class StreamInfo
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 编码器名
        /// </summary>
        public string Codec_Name { get; set; }

        /// <summary>
        /// 编码器名全称
        /// </summary>
        public string Codec_Long_Name { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Profile { get; set; }

        /// <summary>
        /// 编码器类型
        /// </summary>
        public string Codec_Type { get; set; }

        /// <summary>
        /// 编码器每帧时长
        /// </summary>
        public string Codec_Time_Base { get; set; }

        /// <summary>
        /// 编码器标签名
        /// </summary>
        public string Codec_Tag_String { get; set; }

        /// <summary>
        /// 编码器标签
        /// </summary>
        public string Codec_Tag { get; set; }

        /// <summary>
        /// 采样点格式
        /// </summary>
        public string Sample_Fmt { get; set; }

        /// <summary>
        /// 采样率
        /// </summary>
        public long Sample_Rate { get; set; }

        /// <summary>
        /// 音频通道数
        /// </summary>
        public int Channels { get; set; }

        /// <summary>
        /// 音频通道布局
        /// </summary>
        public string Channel_Layout { get; set; }

        /// <summary>
        /// 采样点bit数
        /// </summary>
        public int Bits_Per_Sample { get; set; }

        /// <summary>
        /// 帧宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 帧高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 视频帧宽度
        /// </summary>
        public int Coded_Width { get; set; }

        /// <summary>
        /// 视频帧高度
        /// </summary>
        public int Coded_Height { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Closed_Captions { get; set; }

        /// <summary>
        /// 记录帧缓存大小
        /// <para>视频的延迟帧数</para>
        /// </summary>
        public int Has_B_Frames { get; set; }

        /// <summary>
        /// 像素格式
        /// </summary>
        public string Pix_Fmt { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 色度样品的位置
        /// </summary>
        public string Chroma_Location { get; set; }

        /// <summary>
        /// 参考帧数量
        /// </summary>
        public int Refs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Is_Avc { get; set; }

        /// <summary>
        /// 表示用几个字节表示NALU的长度
        /// </summary>
        public long Nal_Length_Size { get; set; }

        /// <summary>
        /// 真实基础帧率
        /// </summary>
        public string R_Frame_Rate { get; set; }

        /// <summary>
        /// 平均帧率
        /// </summary>
        public string Avg_Frame_Rate { get; set; }

        /// <summary>
        /// 每帧时长
        /// </summary>
        public string Time_Base { get; set; }

        /// <summary>
        /// 流开始时间
        /// <para>基于<see cref="Time_Base"/></para>
        /// </summary>
        public int Start_Pts { get; set; }

        /// <summary>
        /// 首帧时间
        /// </summary>
        public double Start_Time { get; set; }

        /// <summary>
        /// 首帧时间
        /// </summary>
        public TimeSpan Start_Time_Convert { get { return TimeSpan.FromSeconds(Start_Time); } set { Start_Time = value.TotalSeconds; } }

        /// <summary>
        /// 流时长
        /// <para>基于<see cref="Time_Base"/></para>
        /// </summary>
        public int Duration_Ts { get; set; }

        /// <summary>
        /// 时长(秒)
        /// <para>转换（duration_ts * time_base）之后的时长，单位秒</para>
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public TimeSpan Duration_Convert { get { return TimeSpan.FromSeconds(Duration); } set { Duration = value.TotalSeconds; } }

        /// <summary>
        /// 码率
        /// </summary>
        public long Bit_Rate { get; set; }

        /// <summary>
        /// 最大码率
        /// </summary>
        public long Max_Bit_Rate { get; set; }

        /// <summary>
        /// 原生采样的比特数/位深
        /// </summary>
        public int Bits_Per_Raw_Sample { get; set; }

        /// <summary>
        /// 视频流帧数
        /// </summary>
        public long Nb_Frames { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Extradata { get; set; }

        /// <summary>
        /// 配置
        /// </summary>
        public DispositionInfo Disposition { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public StreamTagsInfo Tags { get; set; }
    }
}
