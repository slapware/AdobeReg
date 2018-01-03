using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//
using AdobeReg.Utility;
using AdobeReg.Models;
using Microsoft.Extensions.Configuration; // for app settings
using System.Xml;
using System.Xml.XPath;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdobeReg.Controllers
{
    [Route("api/signin")]
    public class SignInController : Controller
    {
        private AdobeRegContext _context;
        public IConfiguration Configuration { get; }
        private ISlapCrypto sCrypt { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AdobeReg.Controllers.SignInController"/> class.
        /// Added to set dependancy injection objects.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="configuration">Configuration.</param>
        /// <param name="crpto">Crpto.</param>
        /// <param name="adobeGuid">Adobe GUID.</param>
        public SignInController(AdobeRegContext context, IConfiguration configuration, ISlapCrypto crpto, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            Configuration = configuration; // added to inject configuration
            sCrypt = crpto; // added to inject crypto
            _httpContextAccessor = httpContextAccessor;        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/signin
        [HttpPost]
        public string Post([FromBody]string value)
        {
            if (value == null)
            {
                return "";
            }
            // Get contect to obtain ContentType
            var hcontext = _httpContextAccessor.HttpContext;
            string ctype = hcontext.Request.ContentType;
            string expexted = @"application/vnd.adobe.adept+xml";
            // Check request type is valid to continue.
            if(String.Compare(ctype, expexted, true) != 0)
                return "Invalid request type";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(value);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ns1", "http://ns.adobe.com/adept");
            string xPathString = "//ns1:user";
            XmlNode xmlNode = doc.DocumentElement.SelectSingleNode(xPathString, nsmgr);
            if (xmlNode == null)
            {
                return "";
            }
            string _user = (xmlNode.InnerText);
            xPathString = "//ns1:label";
            xmlNode = doc.DocumentElement.SelectSingleNode(xPathString, nsmgr);
            if (xmlNode == null)
            {
                return "";
            }
            var _encpass = (xmlNode.InnerText);
            sCrypt.setConfig(this.Configuration);
            sCrypt.SetKey("PSW");
            string upass = sCrypt.Encrypt(_encpass);

            AUser obj = _context.Auser.Where(c => c.user_id == _user && c.password == upass).SingleOrDefault();
            Response.Headers.Add("Content-Type", "application/vnd.adobe.adept+xml");
            string message = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            message += "<signInResponce xmlns=\"http://ns.adobe.com/adept\">";
            if(obj == null)
            {
                message += @"<error>Not Auth Error</error>";
                message += @"</signInResponce>";
                return message;
            }
            else
            {
                message += @"<user>"+ obj.uuid + "</user>";
                message += @"<label>" + obj.user_id + "</label>";
                message += @"</signInResponce>";
                return message;
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
