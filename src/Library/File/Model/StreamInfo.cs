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
        /// 编码名称
        /// </summary>
        public string Codec_Name { get; set; }

        /// <summary>
        /// 编码全称
        /// </summary>
        public string Codec_Long_Name { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Profile { get; set; }

        /// <summary>
        /// 编码类型
        /// </summary>
        public string Codec_Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Codec_Time_Base { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Codec_Tag_String { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Codec_Tag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Sample_Fmt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Sample_Rate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Channels { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Channel_Layout { get; set; }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public int Coded_Width { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Coded_Height { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Closed_Captions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Has_B_Frames { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Pix_Fmt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Chroma_Location { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Refs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Is_Avc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Nal_Length_Size { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string R_Frame_Rate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Avg_Frame_Rate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Time_Base { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Start_Pts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Start_Time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Duration_Ts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Bit_Rate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Max_Bit_Rate { get; set; }

        /// <summary>
        /// 原生采样的比特数/位深
        /// </summary>
        public string Bits_Per_Raw_Sample { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Nb_Frames { get; set; }

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
