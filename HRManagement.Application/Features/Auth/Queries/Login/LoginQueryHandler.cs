using MediatR;
using Microsoft.EntityFrameworkCore;
using HRManagement.Domain.Entities;
using HRManagement.Application.Interfaces; 

namespace HRManagement.Application.Features.Auth.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, User?>
    {
        private readonly IHrManagementDbContext _context; // Usamos la Interfaz

        public LoginQueryHandler(IHrManagementDbContext context)
        {
            _context = context;
        }

        public async Task<User?> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            // Busca usando la interfaz
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password, cancellationToken);

            return user;
        }
    }
}