using AutoMapper;
using Movies.Application.DTOs;

namespace Movies.Application.Mappings
{
    public class ExternalDtoToInternalMappingProfile : Profile
    {
        public ExternalDtoToInternalMappingProfile()
        {
            CreateMap<MovieResponse, ExternalMovieDto>().ReverseMap();
        }
    }
}
