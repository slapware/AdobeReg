using System;
using Microsoft.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace AdobeReg.Formatter
{
    /// <summary>
    /// Adobe output formatter.
    /// </summary>
    public class AdobeOutputFormatter : StringOutputFormatter
    {
        public AdobeOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/vnd.adobe.adept+xml"));

            SupportedEncodings.Add(Encoding.UTF8);
        }
    }
}
