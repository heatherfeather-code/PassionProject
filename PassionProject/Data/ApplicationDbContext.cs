using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PassionProject.Models;

namespace PassionProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //Create Projects table from the model
        public DbSet<Projects> Projects { get; set; }

        public DbSet<ProjectYarnBridge> ProjectYarn { get; set; }

        public DbSet<YarnItems>  Yarn { get; set; }
    }
}
