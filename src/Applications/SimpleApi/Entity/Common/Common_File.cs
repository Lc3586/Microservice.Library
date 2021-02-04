using FreeSql.DataAnnotations;
using Library.Json.Converters;
using Library.OpenApi.Annotations;
using Newtonsoft.Json;
using System;

namespace Entity.Common
{
    /// <summary>
    /// 文件信息
    /// </summary>
    [Table]
    [OraclePrimaryKeyName("pk_SPAA_" + nameof(Common_File) + "_01")]
    [Index(nameof(Common_File) + "_idx_" + nameof(Name), nameof(Name) + " ASC")]
    [Index(nameof(Common_File) + "_idx_" + nameof(FileType), nameof(FileType) + " ASC")]
    [Index(nameof(Common_File) + "_idx_" + nameof(MimeType), nameof(MimeType) + " ASC")]
    [Index(nameof(Common_File) + "_idx_" + nameof(Suffix), nameof(Suffix) + " ASC")]
    [Index(nameof(Common_File) + "_idx_" + nameof(ServerKey), nameof(ServerKey) + " ASC")]
    [Index(nameof(Common_File) + "_idx_" + nameof(MD5), nameof(MD5) + " ASC")]
    [Index(nameof(Common_File) + "_idx_" + nameof(CreatorId), nameof(CreatorId) + " ASC")]
    [Index(nameof(Common_File) + "_idx_" + nameof(CreateTime), nameof(CreateTime) + " DESC")]
    public class Common_File
    {
        /// <summary>
        /// Id
        /// </summary>
        [OpenApiSubTag("List")]
        [Column(IsPrimary = true, StringLength = 36)]
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [OpenApiSubTag("List", "Detail", "FileInfo")]
        [Column(StringLength = 256)]
        public string Name { get; set; }

        /// <summary>
        /// 完整名称
        /// </summary>
        [OpenApiSubTag("List", "Detail", "FileInfo")]
        [Column(StringLength = 300)]
        public string FullName { get; set; }

        /// <summary>
        /// 文件类型
        /// <para>电子文档</para>
        /// <para>电子表格</para>
        /// <para>图片</para>
        /// <para>音频</para>
        /// <para>视频</para>
        /// <para>压缩包</para>
        /// </summary>
        [OpenApiSubTag("List", "Detail", "FileInfo")]
        [Column(StringLength = 10)]
        public string FileType { get; set; }

        /// <summary>
        /// 内容类型
        /// </summary>
        [OpenApiSubTag("List", "Detail", "FileInfo")]
        [Column(StringLength = 100)]
        public string MimeType { get; set; }

        /// <summary>
        /// 文件后缀
        /// </summary>
        [OpenApiSubTag("List", "Detail", "FileInfo")]
        [Column(StringLength = 30)]
        public string Suffix { get; set; }

        /// <summary>
        /// MD5校验值
        /// </summary>
        [OpenApiSubTag("List", "Detail", "FileInfo")]
        [Column(StringLength = 36)]
        public string MD5 { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [OpenApiSubTag("List", "Detail", "FileInfo")]
        [Column(StringLength = 10)]
        public string State { get; set; }

        /// <summary>
        /// 服务器标识
        /// </summary>
        [OpenApiSubTag("List", "Detail", "FileInfo")]
        [Column(StringLength = 36)]
        public string ServerKey { get; set; }

        /// <summary>
        /// 存储类型
        /// <para>Path</para>
        /// <para>Uri</para>
        /// <para>Base64</para>
        /// <para>Base64Url</para>
        /// </summary>
        [OpenApiSubTag("List", "Detail", "FileInfo")]
        [Column(StringLength = 10)]
        public string StorageType { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        [OpenApiSubTag("_List")]
        [Column(StringLength = -1)]
        public string Path { get; set; }

        /// <summary>
        /// 预览图路径
        /// </summary>
        [OpenApiSubTag("_List")]
        [Column(StringLength = -1)]
        public string ThumbnailPath { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [OpenApiSubTag("_List")]
        [Column(StringLength = 50)]
        public string CreatorId { get; set; }

        /// <summary>
        /// 创建者名称
        /// </summary>
        [OpenApiSubTag("List", "Detail", "FileInfo")]
        [Column(StringLength = 30)]
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [OpenApiSubTag("List", "Detail", "FileInfo")]
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [JsonConverter(typeof(DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime CreateTime { get; set; }
    }
}
