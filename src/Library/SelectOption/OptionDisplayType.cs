using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.SelectOption
{
    /// <summary>
    /// 选项展示类型
    /// </summary>
    public enum OptionDisplayType
    {
        /// <summary>
        /// 文本(无需换行)
        /// </summary>
        label,
        /// <summary>
        /// 文本(换行)
        /// </summary>
        text,
        /// <summary>
        /// 网页内容
        /// </summary>
        html,
        /// <summary>
        /// 链接
        /// </summary>
        link,
        /// <summary>
        /// 图片
        /// </summary>
        image,
        /// <summary>
        /// 轮播图片
        /// </summary>
        carousel,
        /// <summary>
        /// 隐藏域
        /// </summary>
        hidden,
        /// <summary>
        /// 文件
        /// </summary>
        file,
    }
}
