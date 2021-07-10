using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Philip.Models;

namespace Philip.Data
{
    public class PhilipContext : DbContext
    {
        public PhilipContext (DbContextOptions<PhilipContext> options)
            : base(options)
        {
        }

        public DbSet<Philip.Models.Article> Article { get; set; }
        public DbSet<Philip.Models.AuditRecord> AuditRecords { get; set; }
    }
}
