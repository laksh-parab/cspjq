using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSPJQ
{
    /// <summary>
    /// Tag helper for adding a nonce to
    /// inline scripts and styles.
    /// </summary>
    [HtmlTargetElement("script", Attributes = "asp-add-nonce")]
    [HtmlTargetElement("style", Attributes = "asp-add-nonce")]
    public class NonceTagHelper : TagHelper
    {
        private readonly INonceService _nonceService;
        [HtmlAttributeName("asp-add-nonce")]
        public bool AddNonce { get; set; }

        public NonceTagHelper(INonceService nonceService)
        {
            _nonceService = nonceService;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (AddNonce)
            {
                // The nonce service is created per request, so we
                // get the same nonce here as the CSP header
                output.Attributes.Add("nonce", _nonceService.GetNonce());
            }
        }
    }
}
