using AutoMapper;
using MediatR;
using Movies.Application.DTOs;
using Movies.Application.Interfaces;
using Movies.Application.Mediators.Tickets.Commands;
using Movies.Application.Mediators.Tickets.Queries;

namespace Movies.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public TicketService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<TicketDto> CancelAsync(Guid id, CancellationToken cancel)
        {
            var cancelTicketCommand = new TicketCancelCommand(id);
            var ticket = await _mediator.Send(cancelTicketCommand, cancel);
            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task<TicketDto> ConfirmPaymentAsync(Guid id, CancellationToken cancel)
        {
            var confirmPaymentCommand = new TicketConfirmPaymentCommand(id);
            var ticket = await _mediator.Send(confirmPaymentCommand, cancel);
            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task<TicketDto> CreateAsync(TicketDto ticket, CancellationToken cancel)
        {
            var createTicketCommand = _mapper.Map<TicketCreateCommand>(ticket);
            var createdTicket = await _mediator.Send(createTicketCommand, cancel);
            return _mapper.Map<TicketDto>(createdTicket);
        }

        public async Task<TicketDto> GetEnrichedByIdAsync(Guid id, CancellationToken cancel)
        {
            var getTicketEnrichedByIdQuery = new GetTicketEnrichedByIdQuery(id);
            var ticket = await _mediator.Send(getTicketEnrichedByIdQuery, cancel);
            return _mapper.Map<TicketDto>(ticket);
        }
    }
}
