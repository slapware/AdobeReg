using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace AdobeReg.Models
{
    public class Source
    {
        public int Id { get; set; }
        public string orderid { get; set; }
        public string type { get; set; }
        public string city { get; set; }
        public string address1 { get; set; }
        public string site_name1 { get; set; }
        public string site_name2 { get; set; }
        public string site_phone { get; set; }
        public string postalCode { get; set; }
        public string state { get; set; }
        public double totalamount { get; set; }
        public double subtotal { get; set; }
        public double taxtotal { get; set; }
        public AUser auser { get; set; }
    }
}
