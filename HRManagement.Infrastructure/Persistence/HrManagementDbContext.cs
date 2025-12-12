using HRManagement.Application.Interfaces; // Ahora esto funcionará gracias al Paso 1
using HRManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Persistence
{
    // 👇 ¡IMPORTANTE! Agrega ", IHrManagementDbContext" aquí
    public class HrManagementDbContext : DbContext, IHrManagementDbContext
    {
        public HrManagementDbContext(DbContextOptions<HrManagementDbContext> options) : base(options)
        {
        }

        public DbSet<Position> Positions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}