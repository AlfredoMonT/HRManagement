using MediatR;
using HRManagement.Domain.Entities;

namespace HRManagement.Application.Features.Auth.Queries.Login
{
    // Devuelve el objeto User si lo encuentra, o null si falla
    public class LoginQuery : IRequest<User?>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}