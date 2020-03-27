using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Quickstart.UI
{
    public class InMemoryConfiguration
    {
        public static IConfiguration configuration { get; set; }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource("SSH", "System Service Host")//{  ApiSecrets = new [] { new Secret("123456".Sha256()) } }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "TC_00001",
                    ClientSecrets = new [] { new Secret("123456".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new [] { "SSH" }
                },
                new Client
                {
                    ClientId = "MC_test_00001",
                    ClientName = "Mvc Client For Test(Implicit)",
                    ClientSecrets = new [] { new Secret("123456".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Implicit,

                    //登录后的重定向地址
                    RedirectUris = {
                        "http://localhost:5000/signin-oidc",
                        "https://localhost:5001/signin-oidc"
                    },

                    //注销登录后的重定向地址
                    PostLogoutRedirectUris = {
                        "http://localhost:5000/signout-callback-oidc",
                        "https://localhost:5001/signout-callback-oidc"
                    },

                    AllowedScopes = new [] {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone
                    }
                },
                new Client
                {
                    ClientId = "MC_test_00002",
                    ClientName = "Mvc Client For Test(Hybrid)",
                    ClientSecrets = { new Secret("123456".Sha256())},
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    //登录后的重定向地址
                    RedirectUris = {
                        "http://localhost:5000/signin-oidc",
                        "https://localhost:5001/signin-oidc"
                    },

                    //注销登录后的重定向地址
                    PostLogoutRedirectUris = {
                        "http://localhost:5000/signout-callback-oidc",
                        "https://localhost:5001/signout-callback-oidc"
                    },


                    AllowedScopes = new [] {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "SSH"
                    },
                    AllowOfflineAccess = true
                },
                new Client
                {
                    ClientId = "JSC_test_00001",
                    ClientName = "JavaScript Client For Test",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    //登录后的重定向地址
                    RedirectUris = {
                        "http://localhost:5002/html/callback.html",
                        "https://localhost:5003/html/callback.html"
                    },

                    //注销登录后的重定向地址
                    PostLogoutRedirectUris = {
                        "http://localhost:5002/html/index.html",
                        "https://localhost:5003/html/index.html"
                    },
                    AllowedCorsOrigins = {
                        "http://localhost:5002",
                        "https://localhost:5003"
                    },

                    AllowedScopes = new [] {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "SSH"
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Email(),
                new IdentityResources.Phone()
            };
        }
    }
}
