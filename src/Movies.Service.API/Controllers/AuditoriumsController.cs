using Microsoft.AspNetCore.Mvc;
using Movies.Application.Interfaces;

namespace Movies.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriumsController : ControllerBase
    {
        private readonly IAuditoriumService _auditoriumService;

        public AuditoriumsController(IAuditoriumService auditoriumService)
        {
            _auditoriumService = auditoriumService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAsync(CancellationToken cancel)
        {
            var result = await _auditoriumService.GetAsync(cancel);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancel)
        {
            var result = await _auditoriumService.GetByIdAsync(id, cancel);
            return Ok(result);
        }
    }
}
