using System;
using System.Collections.Generic;
using AdobeReg.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using AdobeReg.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // for app settings
using Microsoft.Extensions.DependencyInjection;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdobeReg.Controllers
{
    [Route("api/AdobeReg")]
    public class AdobeRegController : Controller
    {
        private AdobeRegContext _context;
        public IConfiguration Configuration { get; }
        private ISlapCrypto sCrypt { get; set; }
        private IAdobeGuid aGuid { get; set; }

        public AdobeRegController(AdobeRegContext context, IConfiguration configuration, ISlapCrypto crpto, IAdobeGuid adobeGuid)
        {
            _context = context;
            Configuration = configuration; // added to inject configuration
            sCrypt = crpto; // added to inject crypto
            aGuid = adobeGuid; // added to inject guid creation
        }

        // CREATE: api/values
        [HttpPost]
        public IActionResult Create([FromBody] string content)
        {
            if (content == null)
            {
                return BadRequest();
            }
            byte[] cipherText = Convert.FromBase64String(content);
            string plaintext;
            // AES encrypt the password
            {
              sCrypt.setConfig(this.Configuration);
              sCrypt.SetKey("AES");
              plaintext = sCrypt.Decrypt(content);
            }
                // Read XML and parse
            RegParser parser = new RegParser(plaintext, Configuration, sCrypt, aGuid);
                parser.Parse();
                // does order already exist ?
                _context.Auser.Add(parser.auser);
                _context.Sources.Add(parser.source);
                foreach (AOrder _order in parser.itemorders)
                {
                    _context.AOrder.Add(_order);
                }
                _context.SaveChanges();

            return Ok(true);
        }
        // GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/blog
        [HttpGet]
        public IActionResult Get()
        {
            var users = _context.AOrder.ToList();

            if (!users.Any())
            {
                return new NoContentResult();
            }

            return new ObjectResult(users);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return BadRequest();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]string value)
        {
            return BadRequest();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return BadRequest();
        }
    }
}
