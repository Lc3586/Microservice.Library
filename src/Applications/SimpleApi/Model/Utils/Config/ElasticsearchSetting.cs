using System;
using System.Collections.Generic;

namespace Model.Utils.Config
{
    /// <summary>
    /// ElasticSearch配置
    /// </summary>
    public class ElasticsearchSetting
    {
        /// <summary>
        /// 集群
        /// </summary>
        public List<Uri> Nodes { get; set; }

        /// <summary>
        /// 安全验证类型
        /// </summary>
        public ESSecurityType SecurityType { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 密钥标识
        /// </summary>
        public string KeyId { get; set; }

        /// <summary>
        /// 接口密钥
        /// </summary>
        public string ApiKey { get; set; }
    }
}
