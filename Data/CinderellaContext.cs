using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Cinderella.Models
{
    public class CinderellaContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
     {
        public CinderellaContext (DbContextOptions<CinderellaContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Cinderella.Models.Shoe> Shoe { get; set; }
        public DbSet<Cinderella.Models.AuditRecord> AuditRecords { get; set; }
        public DbSet<Cinderella.Models.Review> reviews { get; set; }
        public DbSet<Cinderella.Models.ReviewDesc> reviewDescs { get; set; }
    }
}
