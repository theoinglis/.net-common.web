using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Common.Web.ExtensionMethods
{
    public static class ResponseExtensions
    {
        public static void SetCookie(this HttpResponseMessage response, string key, string value)
        {
            response.Headers.AddCookies(
                new List<CookieHeaderValue>
                {
                    new CookieHeaderValue(key, value) {Path = "/"},
                });
        }

        public static void ClearCookie(this HttpResponseMessage response, string key)
        {
            response.Headers.AddCookies(
                new List<CookieHeaderValue>
                {
                    new CookieHeaderValue(key, string.Empty)
                    {
                        Path = "/", 
                        Expires = DateTime.UtcNow.AddDays(-1)
                    },
                });
        }
    }
}
