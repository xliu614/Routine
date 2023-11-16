using Microsoft.EntityFrameworkCore;
using Routine.Api.Entities;

namespace Routine.Api.Data
{
    public class RoutineDbContext:DbContext
    {
        public RoutineDbContext(DbContextOptions<RoutineDbContext> options): base(options)
        {
            
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Company>().Property(x => x.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Company>().Property(x => x.Introduction).HasMaxLength(500);

            modelBuilder.Entity<Employee>().Property(x => x.EmployeeNo).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Employee>().Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Employee>().Property(x => x.LastName).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Employee>()
                .HasOne(navigationExpression: x=>x.Company)
                .WithMany(navigationExpression: x => x.Employees)
                .HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = Guid.Parse("5836d897-741b-4895-ae31-e6f3f1be1ffd"),
                    Name = "Google",
                    Introduction = "Never get in!"
                },
                new Company
                {
                    Id = Guid.Parse("552bb206-bab4-4c7f-8353-b058a6dc62f4"),
                    Name = "Microsoft",
                    Introduction = "Evil source"
                },
                new Company
                {
                    Id = Guid.Parse("70608d93-5746-4b0b-b73d-88871033a660"),
                    Name = "Alipapa",
                    Introduction = "Fubao China"
                }

           ); 
        }
    }
}
