using System.Collections.Generic;

namespace Model.Utils.Config
{
    /// <summary>
    /// CAS配置
    /// </summary>
    public class CASSetting
    {
        /// <summary>
        /// 协议版本号
        /// </summary>
        public string ProtocolVersion { get; set; }

        /// <summary>
        /// 跨域地址
        /// </summary>
        public List<string> CorsUrl { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// 启用单点注销
        /// </summary>
        public bool EnableSingleSignOut { get; set; }

        /// <summary>
        /// 获取TGT票据地址
        /// </summary>
        public string TGTUrl { get; set; }

        /// <summary>
        /// 生成ST票据地址
        /// </summary>
        public string STUrl { get; set; }

        /// <summary>
        /// 获取用户信息地址
        /// </summary>
        public string UserInfoUrl { get; set; }

        /// <summary>
        /// 删除TGT地址
        /// </summary>
        public string DeleteSTUrl { get; set; }
    }
}
