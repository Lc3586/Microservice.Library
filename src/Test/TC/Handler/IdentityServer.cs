using IdentityModel.Client;
using Library.Cache;
using Library.Extension;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TC.Models;

namespace TC.Handler
{
    public class IdentityServer
    {
        /// <summary>
        /// 验证服务器配置
        /// </summary>
        public static IdentityConfig _IdentityConfig;

        public IdentityServer()
        {

        }

        public IdentityServer(IConfiguration Configuration)
        {
            _IdentityConfig = Configuration.GetSection("Identity").Get<IdentityConfig>();

            if (_IdentityConfig == null)
            {
                "Config Error!".ConsoleWrite(ConsoleColor.Red, "error");
                throw new Exception("Config Error!");
            }
        }

        /// <summary>
        /// 获取验证服务器配置信息
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static async Task GetIdentityServerConfig(bool? ssl)
        {
            try
            {
                _IdentityConfig.useSSL = ssl ?? _IdentityConfig.useSSL;
                ("call " + _IdentityConfig.ConfigURI + " ... ").ConsoleWrite(ConsoleColor.White, "info");
                var apiClient = new HttpClient();
                var Respone = await apiClient.GetAsync(_IdentityConfig.ConfigURI);
                Respone.StatusCode.ConsoleWrite(!Respone.IsSuccessStatusCode ? ConsoleColor.Red : ConsoleColor.Green, null, false);

                var content = await Respone.Content.ReadAsStringAsync();
                content.ConsoleWrite(!Respone.IsSuccessStatusCode ? ConsoleColor.Red : ConsoleColor.White, "respone", true, 1);
            }
            catch (Exception ex)
            {
                "Get IdentityServer Config Error!".ConsoleWrite(ConsoleColor.Red, "error");
                ex.ConsoleWrite(ConsoleColor.Red, "error");
            }
        }

        /// <summary>
        /// 测试验证服务器
        /// </summary>
        /// <param name="grantType">授权模式</param>
        /// <param name="clientId">客户端ID</param>
        /// <param name="clientSecret">客户端秘钥</param>
        /// <param name="scope">权限范围</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="ssl">是否使用安全连接</param>
        /// <returns></returns>
        public static async Task TestIdentityServer(string grantType, string clientId, string clientSecret, string scope, string userName, string password, bool? ssl)
        {
            try
            {
                "Test IdentityServer Start!".ConsoleWrite(ConsoleColor.White, "info");
                switch (grantType)
                {
                    case IdentityModel.OidcConstants.GrantTypes.ClientCredentials:
                        await ClientCredentials(clientId, clientSecret, scope, ssl);
                        break;
                    case IdentityModel.OidcConstants.GrantTypes.Password:
                        await Password(clientId, clientSecret, scope, userName, password, ssl);
                        break;
                    case IdentityModel.OidcConstants.GrantTypes.AuthorizationCode:

                        break;
                    case IdentityModel.OidcConstants.GrantTypes.Implicit:

                        break;
                    default:
                        $"Unknown GrantType [{grantType}]!".ConsoleWrite(ConsoleColor.Red, "error");
                        break;
                }
            }
            catch (Exception ex)
            {
                "Test IdentityServer Error!".ConsoleWrite(ConsoleColor.Red, "error");
                ex.ConsoleWrite(ConsoleColor.Red, "error");
            }
            "Test IdentityServer End!".ConsoleWrite(ConsoleColor.White, "info", true, 1);
        }

        /// <summary>
        /// 使用客户端授权模式测试验证服务器
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="clientSecret">客户端秘钥</param>
        /// <param name="scope">权限范围</param>
        /// <param name="ssl">是否使用安全连接</param>
        /// <returns></returns>
        public static async Task ClientCredentials(string clientId, string clientSecret, string scope, bool? ssl)
        {
            "GrantType is client_credentials!".ConsoleWrite(ConsoleColor.White, "info");
            _IdentityConfig.useSSL = ssl ?? _IdentityConfig.useSSL;
            ("call " + _IdentityConfig.URI + " ... ").ConsoleWrite(ConsoleColor.White, "info");

            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(_IdentityConfig.URI);

            disco.StatusCode.ConsoleWrite(disco.IsError ? ConsoleColor.Red : ConsoleColor.Green, null, false);

            if (disco.IsError)
            {
                (" ... " + disco.Error).ConsoleWrite(ConsoleColor.Yellow, "warn"); ;
                return;
            }

            if (clientSecret.IsNullOrEmpty())
                clientSecret = Library.Extension.Extention.ReadPassword("请输入客户端密钥：");

            ("call " + disco.TokenEndpoint + " ... ").ConsoleWrite(ConsoleColor.White, "info");
            $" {{ ClientId = {clientId}, ClientSecret = {clientSecret.Replace('*', 0)}, Scope = {scope} }} ... ".ConsoleWrite(ConsoleColor.White, "params", false);

            var tokenRespone = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = clientId,
                ClientSecret = clientSecret,
                Scope = scope
            });

            (tokenRespone.IsError ? "FAIL" : "OK").ConsoleWrite(tokenRespone.IsError ? ConsoleColor.Red : ConsoleColor.Green, null, false);

            if (tokenRespone.IsError)
            {
                tokenRespone.Error.ConsoleWrite(ConsoleColor.Yellow, "warn");
                return;
            }

            tokenRespone.Json.ConsoleWrite(ConsoleColor.White, "json", true, 1);

            ("call " + _IdentityConfig.TestTokenURI + " ... ").ConsoleWrite(ConsoleColor.White, "info");

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenRespone.AccessToken);

            var tokenTestRespone = await apiClient.GetAsync(_IdentityConfig.TestTokenURI);

            tokenTestRespone.StatusCode.ConsoleWrite(!tokenTestRespone.IsSuccessStatusCode ? ConsoleColor.Red : ConsoleColor.Green, null, false);

            var content = await tokenTestRespone.Content.ReadAsStringAsync();
            content.ConsoleWrite(!tokenTestRespone.IsSuccessStatusCode ? ConsoleColor.Red : ConsoleColor.White, "respone", true, 1);
        }

        /// <summary>
        /// 使用资源所有者密码授权模式测试验证服务器
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="clientSecret">客户端秘钥</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="scope">权限范围</param>
        /// <param name="ssl">是否使用安全连接</param>
        /// <returns></returns>
        public static async Task Password(string clientId, string clientSecret, string scope, string userName, string password, bool? ssl)
        {
            "GrantType is password!".ConsoleWrite(ConsoleColor.White, "info");
            _IdentityConfig.useSSL = ssl ?? _IdentityConfig.useSSL;
            ("call " + _IdentityConfig.URI + " ... ").ConsoleWrite(ConsoleColor.White, "info");

            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(_IdentityConfig.URI);

            disco.StatusCode.ConsoleWrite(disco.IsError ? ConsoleColor.Red : ConsoleColor.Green, null, false);

            if (disco.IsError)
            {
                (" ... " + disco.Error).ConsoleWrite(ConsoleColor.Yellow, "warn"); ;
                return;
            }

            if (clientSecret.IsNullOrEmpty())
                clientSecret = Library.Extension.Extention.ReadPassword("请输入客户端密钥：");

            if (password.IsNullOrEmpty())
                password = Library.Extension.Extention.ReadPassword("请输入用户密钥：");

            ("call " + disco.TokenEndpoint + " ... ").ConsoleWrite(ConsoleColor.White, "info");
            $" {{ ClientId = {clientId}, ClientSecret = {clientSecret.Replace('*', 0)}, Scope = {scope}, UserName = {userName}, Password = {password.Replace('*', 0)} }} ... ".ConsoleWrite(ConsoleColor.White, "params", false);

            var tokenRespone = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = clientId,
                ClientSecret = clientSecret,
                Scope = scope,

                UserName = userName,
                Password = password
            });

            (tokenRespone.IsError ? "FAIL" : "OK").ConsoleWrite(tokenRespone.IsError ? ConsoleColor.Red : ConsoleColor.Green, null, false);

            if (tokenRespone.IsError)
            {
                tokenRespone.Error.ConsoleWrite(ConsoleColor.Yellow, "warn");
                return;
            }

            tokenRespone.Json.ConsoleWrite(ConsoleColor.White, "json", true, 1);

            ("call " + _IdentityConfig.TestTokenURI + " ... ").ConsoleWrite(ConsoleColor.White, "info");

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenRespone.AccessToken);

            var tokenTestRespone = await apiClient.GetAsync(_IdentityConfig.TestTokenURI);

            tokenTestRespone.StatusCode.ConsoleWrite(!tokenTestRespone.IsSuccessStatusCode ? ConsoleColor.Red : ConsoleColor.Green, null, false);

            var content = await tokenTestRespone.Content.ReadAsStringAsync();
            content.ConsoleWrite(!tokenTestRespone.IsSuccessStatusCode ? ConsoleColor.Red : ConsoleColor.White, "respone", true, 1);
        }

        /// <summary>
        /// 使用授权码授权模式测试验证服务器
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="clientSecret">客户端秘钥</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="scope">权限范围</param>
        /// <param name="ssl">是否使用安全连接</param>
        /// <returns></returns>
        public static async Task AuthorizationCode(string clientId, string clientSecret, string scope, string userName, string password, bool? ssl)
        {

        }

        /// <summary>
        /// 使用隐式授权模式测试验证服务器
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="clientSecret">客户端秘钥</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="scope">权限范围</param>
        /// <param name="ssl">是否使用安全连接</param>
        /// <returns></returns>
        public static async Task Implicit(string clientId, string clientSecret, string scope, string userName, string password, bool? ssl)
        {

        }
    }
}
