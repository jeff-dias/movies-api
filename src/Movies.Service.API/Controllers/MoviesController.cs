using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Movies.Application.Interfaces;

namespace Movies.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IExternalMoviesService _moviesService;

        public MoviesController(IExternalMoviesService moviesService)
        {
            _moviesService = moviesService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancel)
        {
            var movies = await _moviesService.GetAsync(cancel);
            return Ok(movies);
        }
    }
}
