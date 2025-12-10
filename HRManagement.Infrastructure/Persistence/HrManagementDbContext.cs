using HRManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Persistence
{
    public class HrManagementDbContext : DbContext
    {
        public HrManagementDbContext(DbContextOptions<HrManagementDbContext> options) : base(options)
        {
        }

        // Aquí declaramos la tabla "Positions"
        public DbSet<Position> Positions { get; set; }

        public DbSet<Employee> Employees { get; set; }
    }
}