using CodebridgeTestApp.Dtos;
using CodebridgeTestApp.Enum;
using CodebridgeTestApp.Parameters;
using CodebridgeTestApp.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodebridgeTestApp.Controllers
{
    [ApiController]
    public class DogController : ControllerBase
    {
        private readonly IDogService _dogService;

        public DogController(IDogService dogService)
        {
            _dogService = dogService;
        }

        [HttpGet("dogs")]
        public async Task<ActionResult<IEnumerable<DogGetDto>>> GetDogsAsync(
            [FromQuery] PageParameter? pageParameter = null,
            [FromQuery] string attribute = null,
            [FromQuery] SortDirection order = SortDirection.Asc)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (pageParameter is not null && !pageParameter.IsValid())
                return BadRequest("Invalid pagination parameter");

            if (attribute != null && attribute.Count() <= 0) return BadRequest("Invalid attribute parameter");

            var dogs = await _dogService.GetDogsAsync(pageParameter, attribute, order);

            return Ok(dogs);
        }

        [HttpPost("dog")]
        public async Task<ActionResult<DogGetDto>> CreateDogAsync([FromBody] DogCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _dogService.CreateDogAsync(dto);

            if (result is null)
                return BadRequest("Failed to create the dog. Ensure that all fields are valid and name is unique.");

            return Created(string.Empty, result);
        }
    }
}
