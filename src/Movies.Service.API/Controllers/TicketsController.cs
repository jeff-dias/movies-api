using Microsoft.AspNetCore.Mvc;
using Movies.Application.DTOs;
using Movies.Application.Interfaces;

namespace Movies.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEnrichedByIdAsync(Guid id, CancellationToken cancel)
        {
            var ticket = await _ticketService.GetEnrichedByIdAsync(id, cancel);
            return Ok(ticket);
        }

        [HttpPost]
        public async Task<IActionResult> Post(TicketDto ticket, CancellationToken cancel)
        {
            var ticketCreated = await _ticketService.CreateAsync(ticket, cancel);
            return CreatedAtAction(nameof(GetEnrichedByIdAsync), new { id = ticketCreated.Id }, ticketCreated);
        }

        [HttpPut("{id}/confirm")]
        public async Task<IActionResult> ConfirmPayment(Guid id, CancellationToken cancel)
        {
            var ticket = await _ticketService.ConfirmPaymentAsync(id, cancel);
            return Ok(ticket);
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id, CancellationToken cancel)
        {
            var ticket = await _ticketService.CancelAsync(id, cancel);
            return Ok(ticket);
        }
    }
}
