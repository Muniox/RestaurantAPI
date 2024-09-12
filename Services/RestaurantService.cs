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
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _restaurantDbContext;
        private readonly IMapper _mapper;
        public RestaurantService(RestaurantDbContext restaurantDbContext, IMapper mapper)
        {
            _restaurantDbContext = restaurantDbContext;
            _mapper = mapper;
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
