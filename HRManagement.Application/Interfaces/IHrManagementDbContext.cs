using HRManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HRManagement.Application.Interfaces
{
    public interface IHrManagementDbContext
    {
        DbSet<User> Users { get; set; }
        // Aquí puedes agregar otros DbSets si los necesitas en el futuro

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}