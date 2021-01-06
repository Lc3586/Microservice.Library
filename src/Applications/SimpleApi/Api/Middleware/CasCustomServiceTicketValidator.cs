using GSS.Authentication.CAS;
using GSS.Authentication.CAS.Security;
using GSS.Authentication.CAS.Validation;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Xml.Linq;

namespace Api.Middleware
{
    /// <summary>
    /// 自定义CAS票据验证器
    /// </summary>
    /// <remarks>如果不是标准的cas协议，就需要在此类中自定义解析逻辑</remarks>
    public class CasCustomServiceTicketValidator : CasServiceTicketValidator
    {
        protected static XNamespace Namespace = "http://www.yale.edu/tp/cas";
        protected static XNamespace NNamespace = "cas";
        protected static XName Attributes = Namespace + "attributes";
        protected static XName AuthenticationSuccess = Namespace + "authenticationSuccess";
        protected static XName AuthenticationFailure = Namespace + "authenticationFailure";
        protected static XName User = Namespace + "user";
        protected const string Code = "code";

        public CasCustomServiceTicketValidator(
            ICasOptions options,
            HttpClient? httpClient = null)
            : base("serviceValidate", options, httpClient)
        {
        }

        protected CasCustomServiceTicketValidator(
            string suffix,
            ICasOptions options,
            HttpClient? httpClient = null)
            : base(suffix, options, httpClient)
        {
        }

        protected override ICasPrincipal? BuildPrincipal(string responseBody)
        {
            var doc = XElement.Parse(responseBody);
            /* On ticket validation failure:
            <cas:serviceResponse xmlns:cas="http://www.yale.edu/tp/cas">
             <cas:authenticationFailure code="INVALID_TICKET">
                Ticket ST-1856339-aA5Yuvrxzpv8Tau1cYQ7 not recognized
              </cas:authenticationFailure>
            </cas:serviceResponse>
            */
            var failureElement = doc.Element(AuthenticationFailure);
            if (failureElement != null)
            {
                throw new AuthenticationException(failureElement.Value);
            }
            /* On ticket validation success:
            <cas:serviceResponse xmlns:cas="http://www.yale.edu/tp/cas">
                <cas:authenticationSuccess>
                <cas:user>username</cas:user>
                <cas:proxyGrantingTicket>PGTIOU-84678-8a9d...</cas:proxyGrantingTicket>
                </cas:authenticationSuccess>
            </cas:serviceResponse>
            */
            var principalName = doc.Element(AuthenticationSuccess).Element(User)?.Value ?? string.Empty;
            if (string.IsNullOrWhiteSpace(principalName)) return null;
            var assertion = new Assertion(principalName);

            var attributesNode = doc.Element(AuthenticationSuccess).Element(Attributes);
            if (attributesNode != null)
            {
                foreach (var element in attributesNode.Elements())
                {
                    assertion.Attributes.Add(element.Name.LocalName, element.Value);
                }
            }

            return new CasPrincipal(assertion, options.AuthenticationType);
        }
    }
}
