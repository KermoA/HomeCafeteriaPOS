using HomeCafeteriaPOS.Data;
using HomeCafeteriaPOS.Hubs;
using HomeCafeteriaPOS.Services;
using Microsoft.EntityFrameworkCore;

namespace HomeCafeteriaPOS
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<HomeCafeteriaDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<ProductService>();

            builder.Services.AddSignalR();

            builder.Services.AddCors(options => options.AddPolicy("AllowAll", policy =>
                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));


            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var productService = scope.ServiceProvider.GetRequiredService<ProductService>();
                await productService.LoadProductsAsync();
            }


            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapHub<ProductHub>("/productHub");
            app.MapHub<SaleHub>("/saleHub");

            app.Run();
        }
    }
}
