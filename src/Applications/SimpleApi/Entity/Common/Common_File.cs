﻿using FreeSql.DataAnnotations;
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
    [Index(nameof(Common_File) + "_idx_" + nameof(FullName), nameof(FullName) + " ASC")]
    [Index(nameof(Common_File) + "_idx_" + nameof(FileType), nameof(FileType) + " ASC")]
    [Index(nameof(Common_File) + "_idx_" + nameof(ContentType), nameof(ContentType) + " ASC")]
    [Index(nameof(Common_File) + "_idx_" + nameof(Suffix), nameof(Suffix) + " ASC")]
    [Index(nameof(Common_File) + "_idx_" + nameof(StorageType), nameof(StorageType) + " ASC")]
    [Index(nameof(Common_File) + "_idx_" + nameof(CreatorId), nameof(CreatorId) + " ASC")]
    [Index(nameof(Common_File) + "_idx_" + nameof(CreatorName), nameof(CreatorName) + " ASC")]
    [Index(nameof(Common_File) + "_idx_" + nameof(CreateTime), nameof(CreateTime) + " DESC")]
    public class Common_File
    {
        /// <summary>
        /// Id
        /// </summary>
        [Column(IsPrimary = true)]
        [OpenApiSubTag("_List")]
        public long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column(StringLength = 100)]
        [OpenApiSubTag("List", "Detail", "FileInfo")]
        public string Name { get; set; }

        /// <summary>
        /// 完整名称
        /// </summary>
        [Column(StringLength = 255)]
        [OpenApiSubTag("List", "Detail", "FileInfo")]
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
        [Column(StringLength = 30)]
        [OpenApiSubTag("List", "Detail", "FileInfo")]
        public string FileType { get; set; }

        /// <summary>
        /// 内容类型
        /// </summary>
        [Column(StringLength = 100)]
        [OpenApiSubTag("List", "Detail", "FileInfo")]
        public string ContentType { get; set; }

        /// <summary>
        /// 文件后缀
        /// </summary>
        [Column(StringLength = 30)]
        [OpenApiSubTag("List", "Detail", "FileInfo")]
        public string Suffix { get; set; }

        /// <summary>
        /// 所在服务器
        /// </summary>
        [Column(StringLength = 300)]
        [OpenApiSubTag("List", "Detail", "FileInfo")]
        public string Server { get; set; }

        /// <summary>
        /// 存储类型
        /// <para>Url</para>
        /// <para>Base64</para>
        /// <para>Base64Url</para>
        /// <para>Path</para>
        /// </summary>
        [Column(StringLength = 30)]
        [OpenApiSubTag("List", "Detail", "FileInfo")]
        public string StorageType { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        [Column(StringLength = -1)]
        [OpenApiSubTag("_List")]
        public string Path { get; set; }

        /// <summary>
        /// 预览图路径
        /// </summary>
        [Column(StringLength = -1)]
        [OpenApiSubTag("_List")]
        public string ThumbnailPath { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [Column(StringLength = 50)]
        [OpenApiSubTag("_List")]
        public string CreatorId { get; set; }

        /// <summary>
        /// 创建者名称
        /// </summary>
        [Column(StringLength = 30)]
        [OpenApiSubTag("List", "Detail", "FileInfo")]
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [JsonConverter(typeof(DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        [OpenApiSubTag("List", "Detail", "FileInfo")]
        public DateTime CreateTime { get; set; }
    }
}