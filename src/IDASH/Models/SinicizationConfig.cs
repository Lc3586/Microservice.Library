using Microservice.Library.Extension;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDASH.Models
{
    /// <summary>
    /// 汉化配置
    /// </summary>
    public static class SinicizationConfig
    {
        /// <summary>
        /// 初始化
        /// 从配置写入数据
        /// </summary>
        /// <param name="Configuration">配置</param>
        public static bool Init(IConfiguration Configuration)
        {
            IdentityResources = new List<IdentityResourcesData>();
            try
            {
                var Sinicization = Configuration.GetSection("Sinicization");
                for (int i = 0; true; i++)
                {
                    if (Sinicization[$"IdentityResources:{i}:Name"].IsNullOrEmpty())
                        break;
                    IdentityResources.Add(Sinicization.GetSection($"IdentityResources:{i}").Get<IdentityResourcesData>());
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static List<IdentityResourcesData> IdentityResources { get; set; }
    }

    /// <summary>
    /// 身份验证服务资源汉化数据
    /// </summary>
    public class IdentityResourcesData
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
    }
}
