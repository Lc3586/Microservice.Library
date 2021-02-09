using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Library.SampleAuthentication.Application
{
    /// <summary>
    /// 建议身份验证处理类
    /// </summary>
    public class SampleAuthenticationHandler<TOptions> : AuthenticationHandler<TOptions>
        where TOptions : SampleAuthenticationOptions, new()
    {
        public SampleAuthenticationHandler(IOptionsMonitor<TOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
