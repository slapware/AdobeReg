using System;
using Microsoft.EntityFrameworkCore;

namespace AdobeReg.Models
{
    public class AdobeRegContext : DbContext
    {
        public AdobeRegContext(DbContextOptions<AdobeRegContext> options)
            : base(options)
        {
        }
        public DbSet<AOrder> AOrder { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<AUser> Auser { get; set; }
    }
}
