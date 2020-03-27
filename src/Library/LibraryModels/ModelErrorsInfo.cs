using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Models
{
    /// <summary>
    /// 模型验证错误信息
    /// </summary>
    public class ModelErrorsInfo
    {
        /// <summary>
        /// 全名
        /// </summary>
        public string FullKey { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// 错误提示
        /// </summary>
        public string ErrorInfo { get { return string.Join(",", Errors); } }
    }
}
