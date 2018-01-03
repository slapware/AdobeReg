using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AdobeReg.Models
{
    public class AUser
    {
        public int Id { get; set; }
        [Key]
        public string user_id { get; set; }
        public string password { get; set; }
        public string uuid { get; set; }
    }
}
