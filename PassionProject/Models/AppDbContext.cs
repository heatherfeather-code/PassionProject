using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;
using PassionProject.Models;


namespace PassionProject
{
	public class AppDbContext : DbContext
	{
		public DbSet <Projects> Projects { get; set; }
		public DbSet <YarnItems> YarnItems { get; set; }

		public DbSet <ProjectYarnBridge> ProjectYarn { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}

		protected override void OnModelCreating (ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}
	}


	
}
