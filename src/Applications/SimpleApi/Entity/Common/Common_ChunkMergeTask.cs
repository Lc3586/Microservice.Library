using FreeSql.DataAnnotations;
using Microservice.Library.Json.Converters;
using Microservice.Library.OpenApi.Annotations;
using Newtonsoft.Json;
using System;

namespace Entity.Common
{
    /// <summary>
    /// 分片合并任务
    /// </summary>
    [Table]
    [OraclePrimaryKeyName("pk_SPAA_" + nameof(Common_ChunkMergeTask) + "_01")]
    [Index(nameof(Common_ChunkMergeTask) + "_idx_" + nameof(ServerKey), nameof(ServerKey) + " ASC")]
    [Index(nameof(Common_ChunkMergeTask) + "_idx_" + nameof(MD5), nameof(MD5) + " ASC")]
    [Index(nameof(Common_ChunkMergeTask) + "_idx_" + nameof(State), nameof(State) + " ASC")]
    public class Common_ChunkMergeTask
    {
        /// <summary>
        /// Id
        /// </summary>
        [OpenApiSubTag("List")]
        [Column(IsPrimary = true, StringLength = 36)]
        public string Id { get; set; }

        /// <summary>
        /// 服务器标识
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Column(StringLength = 36)]
        public string ServerKey { get; set; }

        /// <summary>
        /// MD5校验值
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Column(StringLength = 36)]
        public string MD5 { get; set; }

        /// <summary>
        /// 分片总数
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        public int TotalChunk { get; set; }

        /// <summary>
        /// 当前处理分片的索引
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        public int CurrentChunkIndex { get; set; }

        /// <summary>
        /// 是否压缩
        /// </summary>
        /// <remarks>
        /// <para>文件为图片时，压缩生成缩略图，原始图片将会保留</para>
        /// <para></para>
        /// </remarks>
        public bool Compress { get; set; }

        /// <summary>
        /// 保存原始文件
        /// </summary>
        public bool SaveOriginalFile { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Column(StringLength = 10)]
        public string State { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [OpenApiSubTag("_List")]
        [Column(StringLength = 50)]
        public string CreatorId { get; set; }

        /// <summary>
        /// 创建者名称
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Column(StringLength = 30)]
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [JsonConverter(typeof(DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime CreateTime { get; set; }
    }
}
