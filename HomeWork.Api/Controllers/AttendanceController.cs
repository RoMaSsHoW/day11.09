using HomeWork.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HomeWork.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IRepository _repository;

        public AttendanceController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("missing")]
        public async Task<IActionResult> GetMissing([FromQuery] DateTime date)
        {
            var res = await _repository.GetMissingAttendanceAsync(date);
            return Ok(res);
        }
    }
}
