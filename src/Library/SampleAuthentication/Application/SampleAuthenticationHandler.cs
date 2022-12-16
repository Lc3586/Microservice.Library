using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Microservice.Library.SampleAuthentication.Application
{
    /// <summary>
    /// 建议身份验证处理类
    /// </summary>
    public class SampleAuthenticationHandler<TOption> : CookieAuthenticationHandler
        where TOption : CookieAuthenticationOptions, new()
    {
        public SampleAuthenticationHandler(IOptionsMonitor<TOption> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
