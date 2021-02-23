using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Utils.Config
{
    /// <summary>
    /// ElasticSearch安全验证类型
    /// </summary>
    public enum ESSecurityType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 经典身份验证
        /// </summary>
        Basic,
        /// <summary>
        /// 接口密钥
        /// </summary>
        ApiKey
    }
}
