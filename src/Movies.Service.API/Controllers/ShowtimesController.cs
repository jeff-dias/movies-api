using Microsoft.AspNetCore.Mvc;
using Movies.Application.DTOs;
using Movies.Application.Interfaces;

namespace Movies.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowtimesController : ControllerBase
    {
        private readonly IShowtimeService _showtimeService;

        public ShowtimesController(IShowtimeService showtimeService)
        {
            _showtimeService = showtimeService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancel)
        {
            var showtimes = await _showtimeService.GetAsync(cancel);
            return Ok(showtimes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEnrichedByIdAsync(Guid id, CancellationToken cancel)
        {
            var showtime = await _showtimeService.GetEnrichedByIdAsync(id, cancel);
            return Ok(showtime);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ShowtimeDto showtime, CancellationToken cancel)
        {
            var showtimeCreated = await _showtimeService.CreateAsync(showtime, cancel);
            return CreatedAtAction(nameof(Get), new { id = showtimeCreated.Id }, showtimeCreated);
        }
    }
}
