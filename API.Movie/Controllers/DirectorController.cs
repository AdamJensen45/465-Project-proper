using APP.MOV.Features.Directors;
using CORE.APP.Features;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Movie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DirectorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/director
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IQueryable<DirectorQueryResponse> query = await _mediator.Send(new DirectorQueryRequest());
            List<DirectorQueryResponse> list = await query.ToListAsync();
            if (list.Count > 0)
                return Ok(list);
            return NoContent();
        }

        // GET: api/director/1
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = await _mediator.Send(new DirectorQueryRequest());
            var item = await query.SingleOrDefaultAsync(t => t.Id == id);
            if (item != null)
                return Ok(item);
            return NoContent();
        }

        // POST: api/director
        [HttpPost]
        public async Task<IActionResult> Post(DirectorCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await _mediator.Send(request);
                if (response.IsSuccessful)
                {
                    return Ok(response);
                }
                ModelState.AddModelError("DirectorPost", response.Message);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/director
        [HttpPut]
        public async Task<IActionResult> Put(DirectorUpdateRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await _mediator.Send(request);
                if (response.IsSuccessful)
                {
                    return Ok(response);
                }
                ModelState.AddModelError("DirectorPut", response.Message);
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/director/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DirectorDeleteRequest() { Id = id });

            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}