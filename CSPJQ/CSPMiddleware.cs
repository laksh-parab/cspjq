using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSPJQ
{
    public class CSPMiddleware
    {
        private readonly RequestDelegate _next;

        public CSPMiddleware(RequestDelegate next)
        {
            if (next == null)
            {
                throw new ArgumentNullException("next");
            }          

            _next = next;            
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.Response;
                      

            var headers = response.Headers;
            var nonceService = (INonceService)context.RequestServices.GetService(typeof(INonceService));

            var nonce = nonceService.GetNonce();
            var csp = $"default-src 'none'; script-src 'self' 'nonce-{nonce}' 'unsafe-eval'; "+
                "style-src 'self' 'unsafe-inline'; "+
                "img-src 'self' data:; font-src 'self'; object-src 'self'; "+
                "frame-src 'self'; connect-src 'self'; child-src 'self'; "+
                "report-uri /home/cspreport;";

            headers["Content-Security-Policy"] = csp;


            await _next(context);
        }
    }
}
