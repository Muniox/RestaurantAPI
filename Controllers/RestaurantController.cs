using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantDbContext _restaurantDbContext;
        private readonly IMapper _mapper;
        public RestaurantController(RestaurantDbContext restaurantDbContext, IMapper mapper)
        {
            _restaurantDbContext = restaurantDbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll() //jeśli chcemy tylko zwrocic dane do otczytu uzywamy IEnumerable. It avoids unnecessary copying or modification operations
        {
            var restaurants = _restaurantDbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .ToList();
            //var restaurantsDtos = restaurants.Select(r => new RestaurantDto()
            //{
            //    Name = r.Name,
            //    Category = r.Category,
            //    City = r.Address.City,
            //})

            var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);
            return Ok(restaurantsDtos);
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

            var restaurant = _mapper.Map<Restaurant>(dto);
            _restaurantDbContext.Add(restaurant);
            _restaurantDbContext.SaveChanges();

            return Created($"/api/restaurant/{restaurant.Id}", null);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<RestaurantDto> Get([FromRoute] int id)
        {
            var restaurant = _restaurantDbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
            {
                return NotFound();
            }

            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            return Ok(restaurantDto);
        }
    }
}
