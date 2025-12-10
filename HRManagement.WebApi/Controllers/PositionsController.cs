using HRManagement.Domain.Entities;
using HRManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly HrManagementDbContext _context;

        public PositionsController(HrManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetPositions()
        {
            return Ok(_context.Positions.ToList());
        }

        [HttpPost]
        public IActionResult CreatePosition(Position position)
        {
            _context.Positions.Add(position);
            _context.SaveChanges();
            return Ok(position);
        }
    }
}