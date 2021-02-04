using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Common
{
    /// <summary>
    /// 分片合并任务状态
    /// </summary>
    public static class ChunkMergeTaskState
    {
        public const string 等待上传 = "等待上传";

        public const string 等待合并 = "等待合并";

        public const string 正在合并 = "正在合并";

        public const string 等待压缩 = "等待压缩";

        public const string 等待清理 = "等待清理";

        public const string 已完成 = "已完成";
    }
}
