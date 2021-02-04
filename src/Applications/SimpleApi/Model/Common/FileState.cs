using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Common
{
    /// <summary>
    /// 文件状态
    /// </summary>
    public static class FileState
    {
        public const string 等待上传 = "等待上传";

        public const string 等待处理 = "等待处理";

        public const string 可用 = "可用";

        public const string 不可用 = "不可用";
    }
}
