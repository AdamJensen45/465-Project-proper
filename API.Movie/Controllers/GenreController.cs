using APP.MOV.Features.Genres;
using CORE.APP.Features;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Movie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GenreController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GenreController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/genre
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IQueryable<GenreQueryResponse> query = await _mediator.Send(new GenreQueryRequest());
            List<GenreQueryResponse> list = await query.ToListAsync();
            if (list.Count > 0)
                return Ok(list);
            return NoContent();
        }

        // GET: api/genre/1
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = await _mediator.Send(new GenreQueryRequest());
            var item = await query.SingleOrDefaultAsync(t => t.Id == id);
            if (item != null)
                return Ok(item);
            return NoContent();
        }

        // POST: api/genre
        [HttpPost]
        public async Task<IActionResult> Post(GenreCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await _mediator.Send(request);
                if (response.IsSuccessful)
                {
                    return Ok(response);
                }
                ModelState.AddModelError("GenrePost", response.Message);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/genre
        [HttpPut]
        public async Task<IActionResult> Put(GenreUpdateRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await _mediator.Send(request);
                if (response.IsSuccessful)
                {
                    return Ok(response);
                }
                ModelState.AddModelError("GenrePut", response.Message);
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/genre/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new GenreDeleteRequest() { Id = id });

            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}