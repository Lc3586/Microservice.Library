using Library.Container;
using Microsoft.AspNetCore.Http;

namespace Library.WebApp
{
    public static class HttpContextCore
    {
        public static HttpContext Current { get => AutofacHelper.GetService<IHttpContextAccessor>().HttpContext; }
    }
}
