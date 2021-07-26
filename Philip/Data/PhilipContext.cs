using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Philip.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Philip.Data
{
    public class PhilipContext : IdentityDbContext<ApplicationUser>
    {
        public PhilipContext (DbContextOptions<PhilipContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Philip.Models.Article> Article { get; set; }
        public DbSet<Philip.Models.AuditRecord> AuditRecords { get; set; }
        public DbSet<Philip.Models.Feedback> Feedback { get; set; }

    }
}
