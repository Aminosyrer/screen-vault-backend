using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models;
using MovieDatabase.Services;
using System.Collections.Generic;

namespace MovieDatabase.Controllers
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

        [HttpGet("{id:length(24)}", Name = "GetMovie")]
        public ActionResult<Movie> Get(string id)
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
            _movieService.Create(movie);
            return CreatedAtRoute("GetMovie", new { id = movie.Id.ToString() }, movie);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Movie movieIn)
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
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var movie = _movieService.Get(id);

            if (movie == null)
            {
                return NotFound();
            }

            _movieService.Remove(movie.Id);

            return NoContent();
        }
    }
}
