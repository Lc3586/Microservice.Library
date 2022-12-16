using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.File.Model
{
    /// <summary>
    /// 库版本信息
    /// </summary>
    public class LibraryVersionInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Major { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Minor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Micro { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Ident { get; set; }
    }
}
