using NLog.Extensions.Logging;
using RestaurantAPI.Entities;
using RestaurantAPI.Middleware;
using RestaurantAPI.Services;
using System.Reflection;

namespace RestaurantAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure NLog
            builder.Services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
            });

            // Add services to the container.

            // Add NLog as the logger provider
            builder.Services.AddSingleton<ILoggerProvider, NLogLoggerProvider>();

            //builder.Services.AddSingleton<>(); // pewnoœæ, ¿e dana zale¿noœæ zosta³a utworzona tylko raz
            //builder.Services.AddScoped<>() //na jedno zapytanie bêdziemy mieæ tylko jedn¹ instancjê danego serwiu
            //builder.Services.AddTransient<>(); //obiekty bêd¹ tworzone za ka¿dym razem kiedy odwo³ujemy siê don nich przez konstrukor
            builder.Services.AddControllers();
            builder.Services.AddDbContext<RestaurantDbContext>();
            builder.Services.AddScoped<RestaurantSeeder>();
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //builder.Services.AddAutoMapper(typeof(RestaurantMappingProfile));
            builder.Services.AddScoped<IRestaurantService, RestaurantService>();
            builder.Services.AddScoped<ErrorHandlingMiddleware>();

            //builder.Services.AddEndpointsApiExplorer(); //Only for minimal API
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            using (var scope = app.Services.CreateScope()) // Create a scope for dependency injection
            {
                var services = scope.ServiceProvider;
                var seeder = services.GetRequiredService<RestaurantSeeder>(); // jeœli RestaurantSeeder nie bêdzie w Services.AddScoped<RestaurantSeeder>() wyrzuci b³¹d!
                seeder.Seed(); // Call the Seed method to populate data
            }

            //Middlewares
            if (!app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ErrorHandlingMiddleware>();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI((options => // UseSwaggerUI is called only in Development.
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API");
                    //options.RoutePrefix = string.Empty; //jeœli chcemy bez swaggera w sciezce
                }));
            }


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
