using APP.MOV.Features.Movies;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Movie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MovieController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovieController(IMediator mediator)
        {
            _mediator = mediator;
        }
        //Get api/movie
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Get()
        {
            IQueryable<MovieQueryResponse> query = await _mediator.Send(new MovieQueryRequest());
            List<MovieQueryResponse> list = await query.ToListAsync();
            if (list.Count > 0)
                return Ok(list);
            return NoContent();
        }
        //Get api/movie/1
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
           var query = await _mediator.Send(new MovieQueryRequest());
           var item = await query.SingleOrDefaultAsync(t => t.Id == id);
            if (item != null)
                return Ok(item);
            return NoContent();

        }
        //Post api/movie
        [HttpPost]
        public async Task<IActionResult> Post(MovieCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await _mediator.Send(request);
                if (response.IsSuccessful)
                {
                    return Ok(response);
                }
                ModelState.AddModelError("MoviePost", response.Message);
            }
            return BadRequest(ModelState);

        }
        //Put api/movie
        [HttpPut]
        public async Task<IActionResult> Put(MovieUpdateRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await _mediator.Send(request);
                if (response.IsSuccessful)
                {
                    return Ok(response);
                }
                ModelState.AddModelError("MoviePut", response.Message);
            }
            return BadRequest(ModelState);
        }
        //Delete api/movie/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new MovieDeleteRequest() { Id = id });

            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
