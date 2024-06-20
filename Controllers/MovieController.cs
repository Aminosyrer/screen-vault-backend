using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScreenVault.Models;
using ScreenVault.Services;
using System.Collections.Generic;

namespace ScreenVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieService _movieService;

        public MovieController(MovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public ActionResult<List<Movie>> Get() => _movieService.Get();

        [HttpGet("{id}", Name = "GetMovie")]
        public ActionResult<Movie> Get(int id)
        {
            var movie = _movieService.Get(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult<Movie> Create(Movie movie)
        {
            _ = _movieService.Create(movie);
            return CreatedAtRoute("GetMovie", new { id = movie.Id }, movie);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, Movie movieIn)
        {
            var movie = _movieService.Get(id);

            if (movie == null)
            {
                return NotFound();
            }

            _movieService.Update(id, movieIn);

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var movie = _movieService.Get(id);

            if (movie == null)
            {
                return NotFound();
            }

            _movieService.Remove(id);

            return NoContent();
        }
    }
}