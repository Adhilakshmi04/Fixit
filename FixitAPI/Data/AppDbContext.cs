using FixitAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FixitAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<EmployeeService> EmployeeServices { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Defining the many-to-many relationship between Employees and Services
            modelBuilder.Entity<EmployeeService>()
                .HasKey(es => new { es.EmployeeId, es.ServiceId });

            modelBuilder.Entity<EmployeeService>()
                .HasOne(es => es.Employee)
                .WithMany(e => e.EmployeeServices)
                .HasForeignKey(es => es.EmployeeId);

            modelBuilder.Entity<EmployeeService>()
                .HasOne(es => es.Service)
                .WithMany(s => s.EmployeeServices)
                .HasForeignKey(es => es.ServiceId);

            // Seeding Roles and Services data
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "Electrician" },
                new Role { Id = 2, RoleName = "Plumber" },
                new Role { Id = 3, RoleName = "Carpenter" },
                new Role { Id = 4, RoleName = "Maid" },
                new Role { Id = 5, RoleName = "Mason" }
            );

            modelBuilder.Entity<Service>().HasData(
                new Service { Id = 1, RoleId = 1, ServiceName = "AC" },
                new Service { Id = 2, RoleId = 1, ServiceName = "Washing Machine" },
                new Service { Id = 3, RoleId = 1, ServiceName = "Building Wiring" },
                new Service { Id = 4, RoleId = 1, ServiceName = "Mixer" },
                new Service { Id = 5, RoleId = 1, ServiceName = "Water Heater" },
                new Service { Id = 6, RoleId = 1, ServiceName = "Motor" },
                new Service { Id = 7, RoleId = 1, ServiceName = "TV" },
                new Service { Id = 8, RoleId = 1, ServiceName = "Grinder" },
                new Service { Id = 9, RoleId = 1, ServiceName = "Fridge" },
                new Service { Id = 10, RoleId = 2, ServiceName = "Building Water Connection" },
                new Service { Id = 11, RoleId = 3, ServiceName = "Window Installation & Repair" },
                new Service { Id = 12, RoleId = 3, ServiceName = "Furniture Repair" },
                new Service { Id = 13, RoleId = 3, ServiceName = "Stair Installation & Repair" },
                new Service { Id = 14, RoleId = 3, ServiceName = "Door Installation & Repair" },
                new Service { Id = 15, RoleId = 3, ServiceName = "Roofing Frame Installation & Repair" },
                new Service { Id = 16, RoleId = 4, ServiceName = "Laundry" },
                new Service { Id = 17, RoleId = 4, ServiceName = "House Cleaning" },
                new Service { Id = 18, RoleId = 4, ServiceName = "Cooking" },
                new Service { Id = 19, RoleId = 4, ServiceName = "Baby Sitter" },
                new Service { Id = 20, RoleId = 4, ServiceName = "Caring for Household Pets" },
                new Service { Id = 21, RoleId = 4, ServiceName = "Ironing" },
                new Service { Id = 22, RoleId = 5, ServiceName = "Building Construction Works" }
            );
        }
    }
}
