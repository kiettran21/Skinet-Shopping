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

            // Resgister Unit Of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add Service Basket repository
            services.AddScoped<IBasketRepository, BasketRepository>();

            // Add Order Service
            services.AddScoped<IOrderService, OrderService>();

            services.AddSingleton<IResponseCacheService, ResponseCacheService>();

            services.AddAutoMapper(typeof(MapperProfile));

            // Send Mail
            services.AddScoped<IMailSenderService, MailSenderService>();

            // Rating Service
            services.AddScoped<IRatingService, RatingService>();

            // Create JWT service
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}