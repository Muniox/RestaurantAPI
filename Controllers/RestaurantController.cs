using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll() //jeśli chcemy tylko zwrocic dane do otczytu uzywamy IEnumerable. It avoids unnecessary copying or modification operations
        {
            var restaurantDtos = _restaurantService.GetAll();
            return Ok(restaurantDtos);
        }

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            //Model state nie jes potrzebny, błąd jest automatycznie zwracany z annotation
            //Jeśli nie damy annotation [ApiController] zadziała poniższe: https://stackoverflow.com/questions/66545845/what-does-the-apicontroller-attribute-do
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            int id = _restaurantService.Create(dto);

            return Created($"/api/restaurant/{id}", null);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<RestaurantDto> Get([FromRoute] int id)
        {
            var restaurantDto = _restaurantService.GetById(id);

            if (restaurantDto is null)
            {
                return NotFound();
            }

            return Ok(restaurantDto);
        }
    }
}
