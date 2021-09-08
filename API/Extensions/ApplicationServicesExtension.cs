using API.Helpers;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace API.Exttensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Add Service Basket repository
            services.AddScoped<IBasketRepository, BasketRepository>();

            services.AddAutoMapper(typeof(MapperProfile));

            // Create JWT service
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}