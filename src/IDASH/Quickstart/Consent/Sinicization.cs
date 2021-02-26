using IDASH.Models;
using IdentityServer4.Models;
using Microservice.Library.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDASH.Quickstart.Consent
{
    /// <summary>
    /// 汉化数据
    /// </summary>
    public static class Sinicization
    {

        /// <summary>
        /// 汉化身份资源
        /// </summary>
        /// <param name="resources"></param>
        /// <param name="sinicizationConfig">汉化配置</param>
        /// <returns></returns>
        public static Resources Resources(this Resources resources)
        {
            try
            {
                foreach (var IdentityResource in resources.IdentityResources)
                {
                    var Sinicization = SinicizationConfig.IdentityResources.FirstOrDefault(o => o.Name == IdentityResource.Name);
                    if (Sinicization != null)
                    {
                        if (!Sinicization.DisplayName.IsNullOrEmpty())
                            IdentityResource.DisplayName = Sinicization.DisplayName;
                        if (!Sinicization.Description.IsNullOrEmpty())
                            IdentityResource.Description = Sinicization.Description;
                    }
                }
            }
            catch (Exception)
            {

            }
            return resources;
        }
    }
}
