using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace MC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string Message = "")
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = Message });
        }

        [Authorize]
        public IActionResult Secure()
        {
            ViewData["Message"] = "安全页面.";

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Secure_Hybrid()
        {
            ViewData["Message"] = "安全页面(混合模式).";
            //访问令牌
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            //刷新令牌
            //var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(IdentityConfig._IdentityConfig.TockenAuthScheme, accessToken);
            ViewBag.Json = JArray.Parse(await client.GetStringAsync("https://localhost:9003/api/values")).ToString();
            return View();
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}
