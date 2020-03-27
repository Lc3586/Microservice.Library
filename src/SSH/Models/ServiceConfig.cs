using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSH.Models
{
    public class ServiceConfig
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 秘钥
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// 跨域配置信息
        /// </summary>
        public List<Cors> Cors { get; set; }
    }

    /// <summary>
    /// 跨域配置
    /// </summary>
    public class Cors
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 域名
        /// </summary>
        public string[] Origins { get; set; }

        /// <summary>
        /// 允许任何头
        /// </summary>
        public bool? AnyHeader { get; set; } = false;

        /// <summary>
        /// 允许任何方法
        /// </summary>
        public bool? AnyMethod { get; set; } = false;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? Enable { get; set; } = false;
    }
}
