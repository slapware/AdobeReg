using System;
using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AdobeReg.Models
{
    public class AOrder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string orderid { get; set; }
        public string orderdate { get; set; }
        public string title { get; set; }
        public string currency { get; set; }
        public string submissionDate { get; set; }
        [Required]
        public double unitprice { get; set; }
        [Required]
        public string custemail { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string paymethod { get; set; }
        public string lineitem { get; set; }
        public string productid { get; set; }
        [Required]
        public string isbn { get; set; }
        public string quantity { get; set; }
        public string testorder { get; set; }
        public string postalcode { get; set; }
        public string store { get; set; }
        public string billpostalCode {get; set; }
        public string storeid { get; set; }
        public string url { get; set; }

        public Source source { get; set; }
        public AUser auser { get; set; }
    }
}
