using Microservice.Library.OpenApi.Annotations;
using Newtonsoft.Json;
using System;

namespace Model.Utils.Log.LogDTO
{
    /// <summary>
    /// 文件信息
    /// </summary>
    public class FileInfo
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文件后缀
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// 字节数
        /// </summary>
        public string Bytes { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [JsonConverter(typeof(Microservice.Library.OpenApi.JsonExtension.DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 上次写入时间
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [JsonConverter(typeof(Microservice.Library.OpenApi.JsonExtension.DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime LastWriteTime { get; set; }
    }
}
