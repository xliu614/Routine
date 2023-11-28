﻿using Microsoft.EntityFrameworkCore;
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
                .HasOne(navigationExpression: x => x.Company)
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
           modelBuilder.Entity<Employee>().HasData(
                 new Employee
                 {
                     Id = Guid.Parse("4b501cb3-d168-4cc0-b375-48fb33f318a4"),
                     CompanyId = Guid.Parse("5836d897-741b-4895-ae31-e6f3f1be1ffd"),
                     DateOfBirth = new DateTime(1976, 1, 2),
                     EmployeeNo = "MSFT231",
                     FirstName = "Nick",
                     LastName = "Carter",
                     Gender = Gender.Male
                 },
                 new Employee
                 {
                     Id = Guid.Parse("7eaa532c-1be5-472c-a738-94fd26e5fad6"),
                     CompanyId = Guid.Parse("5836d897-741b-4895-ae31-e6f3f1be1ffd"),
                     DateOfBirth = new DateTime(1981, 12, 5),
                     EmployeeNo = "MSFT245",
                     FirstName = "Vince",
                     LastName = "Carter",
                     Gender = Gender.Male
                 },
                 new Employee
                 {
                     Id = Guid.Parse("72457e73-ea34-4e02-b575-8d384e82a481"),
                     CompanyId = Guid.Parse("5836d897-741b-4895-ae31-e6f3f1be1ffd"),
                     DateOfBirth = new DateTime(1986, 11, 4),
                     EmployeeNo = "G003",
                     FirstName = "Mary",
                     LastName = "King",
                     Gender = Gender.Female
                 },
                 new Employee
                 {
                     Id = Guid.Parse("7644b71d-d74e-43e2-ac32-8cbadd7b1c3a"),
                     CompanyId = Guid.Parse("552bb206-bab4-4c7f-8353-b058a6dc62f4"),
                     DateOfBirth = new DateTime(1977, 4, 6),
                     EmployeeNo = "G097",
                     FirstName = "Kevin",
                     LastName = "Richardson",
                     Gender = Gender.Male
                 },
                 new Employee
                 {
                     Id = Guid.Parse("679dfd33-32e4-4393-b061-f7abb8956f53"),
                     CompanyId = Guid.Parse("552bb206-bab4-4c7f-8353-b058a6dc62f4"),
                     DateOfBirth = new DateTime(1967, 1, 24),
                     EmployeeNo = "A009",
                     FirstName = "Carr",
                     LastName = "Lee",
                     Gender = Gender.Male
                 },
                 new Employee
                 {
                     Id = Guid.Parse("1861341e-b42b-410c-ae21-cf11f36fc574"),
                     CompanyId = Guid.Parse("70608d93-5746-4b0b-b73d-88871033a660"),
                     DateOfBirth = new DateTime(1957, 3, 8),
                     EmployeeNo = "A404",
                     FirstName = "Not",
                     LastName = "Man",
                     Gender = Gender.Male
                 });
        }
    }
}

