using MediatR;
using HRManagement.Application.Features.Auth.Queries.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // 👇 ESTAS DOS LÍNEAS TE FALTABAN, POR ESO EL ERROR
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        // 👇 ESTE CONSTRUCTOR TAMBIÉN ES OBLIGATORIO
        public AuthController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginQuery request)
        {
            // Ahora _mediator ya existe
            var user = await _mediator.Send(request);

            if (user == null)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

            // Generación del Token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
    }
}