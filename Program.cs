using RestaurantAPI.Entities;
using RestaurantAPI.Services;
using System.Reflection;

namespace RestaurantAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //builder.Services.AddSingleton<>(); // pewnoœæ, ¿e dana zale¿noœæ zosta³a utworzona tylko raz
            //builder.Services.AddScoped<>() //na jedno zapytanie bêdziemy mieæ tylko jedn¹ instancjê danego serwiu
            //builder.Services.AddTransient<>(); //obiekty bêd¹ tworzone za ka¿dym razem kiedy odwo³ujemy siê don nich przez konstrukor
            builder.Services.AddControllers();
            builder.Services.AddDbContext<RestaurantDbContext>();
            builder.Services.AddScoped<RestaurantSeeder>();
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //builder.Services.AddAutoMapper(typeof(RestaurantMappingProfile));
            builder.Services.AddScoped<IRestaurantService, RestaurantService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            using (var scope = app.Services.CreateScope()) // Create a scope for dependency injection
            {
                var services = scope.ServiceProvider;
                var seeder = services.GetRequiredService<RestaurantSeeder>(); // jeœli RestaurantSeeder nie bêdzie w Services.AddScoped<RestaurantSeeder>() wyrzuci b³¹d!
                seeder.Seed(); // Call the Seed method to populate data
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
