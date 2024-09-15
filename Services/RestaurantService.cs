using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        RestaurantDto GetById(int id);
        IEnumerable<RestaurantDto> GetAll();
        int Create(CreateRestaurantDto dto);
        bool Delete(int id);
        bool Update(int id, UpdateRestaurantDto dto);

    }

    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _restaurantDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        public RestaurantService(RestaurantDbContext restaurantDbContext, IMapper mapper, ILogger<RestaurantService> logger)
        {
            _restaurantDbContext = restaurantDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public bool Update(int id, UpdateRestaurantDto dto)
        {
            var restaurant = _restaurantDbContext.Restaurants.FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
            {
                //throw new NotFoundException("Restaurant not found");
                return false;
            }

            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;

            _restaurantDbContext.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            _logger.LogWarning($"Restaurant with id: {id} DELETE acrion invoked");

            var restaurant = _restaurantDbContext
                .Restaurants
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
            {
                return false;
            }

            _restaurantDbContext.Restaurants.Remove(restaurant);
            _restaurantDbContext.SaveChanges();

            return true;
        }


        public RestaurantDto GetById(int id)
        {
            var restaurant = _restaurantDbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null) return null;

            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            return restaurantDto;
        }

        public IEnumerable<RestaurantDto> GetAll()
        {
            _logger.LogError($"Returned all Restaurants :)");

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

            var restaurantDtos = _mapper.Map<List<RestaurantDto>>(restaurants);
            return restaurantDtos;
        }

        public int Create(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            _restaurantDbContext.Add(restaurant);
            _restaurantDbContext.SaveChanges();

            return restaurant.Id;
        }
    }
}
