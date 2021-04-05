using System;
using System.Collections.Generic;
using System.Text;

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
        /// 
        /// </summary>
        public int Nb_Streams { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Nb_Programs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Format_Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Format_Long_Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Start_Time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Bit_Rate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Probe_Score { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public FormatTagsInfo Tags { get; set; }
    }
}
