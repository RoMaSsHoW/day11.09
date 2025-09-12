using HomeWork.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HomeWork.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly IRepository _repository;

        public CoursesController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _repository.GetCourseStatsAsync();
            return Ok(stats);
        }
    }
}
