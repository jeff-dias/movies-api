using AutoMapper;
using MediatR;
using Movies.Application.DTOs;
using Movies.Application.Interfaces;
using Movies.Application.Mediators.Auditoriums.Queries;

namespace Movies.Application.Services
{
    public class AuditoriumService : IAuditoriumService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuditoriumService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<ICollection<AuditoriumDto>> GetAsync(CancellationToken cancel)
        {
            var result = await _mediator.Send(new GetAuditoriumsQuery(), cancel);
            return _mapper.Map<ICollection<AuditoriumDto>>(result);
        }

        public async Task<AuditoriumDto> GetByIdAsync(Guid id, CancellationToken cancel)
        {
            var auditoriumById = new GetAuditoriumByIdQuery(id);

            var result = await _mediator.Send(auditoriumById, cancel);
            return _mapper.Map<AuditoriumDto>(result);
        }
    }
}
