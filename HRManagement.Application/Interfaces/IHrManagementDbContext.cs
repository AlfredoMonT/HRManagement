using HRManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Application.Interfaces
{
    public interface IHrManagementDbContext
    {
        DbSet<User> Users { get; set; }


        DbSet<Employee> Employees { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}