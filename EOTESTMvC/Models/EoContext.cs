using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EOTESTMvC.Models
{
    public class EoContext:IdentityDbContext<User1>
    {
        public EoContext(DbContextOptions<EoContext> options) : base(options) { }
        public DbSet<Tokens> Tokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
