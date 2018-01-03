using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdobeReg.Models;
using Microsoft.AspNetCore.Mvc;
using AdobeReg.Utility;
using Microsoft.Extensions.Configuration; // for app settings

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdobeReg.Controllers
{
    [Route("api/test")]
    public class AdobeTestController : Controller
    {
        private AdobeRegContext _context;
        public IConfiguration Configuration { get; }
        private ISlapCrypto sCrypt { get; set; }
        private IAdobeGuid aGuid { get; set; }

        public AdobeTestController(AdobeRegContext context, IConfiguration configuration, ISlapCrypto crpto, IAdobeGuid adobeGuid)
        {
            _context = context;
            Configuration = configuration; // added to inject configuration
            sCrypt = crpto; // added to inject crypto
            aGuid = adobeGuid; // added to inject adobe Guid
        }

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

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {

            if (value == null)
            {
                return BadRequest();
            }

            // Read XML and parse
            RegParser parser = new RegParser(value, this.Configuration, sCrypt, aGuid);
            parser.Parse();
            _context.Auser.Add(parser.auser);
            _context.Sources.Add(parser.source);
            foreach (AOrder _order in parser.itemorders)
            {
                _context.AOrder.Add(_order);
            }
            _context.SaveChanges();
            return Ok(true);
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
