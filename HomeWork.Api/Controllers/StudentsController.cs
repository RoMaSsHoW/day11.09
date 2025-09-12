using HomeWork.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HomeWork.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IRepository _repository;

        public StudentsController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _repository.GetStudentsWithGroupsAsync();
            return Ok(students);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? name, 
            [FromQuery] string? course, 
            [FromQuery] decimal? minPayment)
        {
            var res = await _repository.SearchStudentsAsync(name, course, minPayment);
            return Ok(res);
        }
    }
}
