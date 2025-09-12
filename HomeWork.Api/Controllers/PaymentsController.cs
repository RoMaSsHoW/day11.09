using HomeWork.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HomeWork.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IRepository _repository;

        public PaymentsController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _repository.GetPaymentsAsync();
            return Ok(payments);
        }
    }
}
