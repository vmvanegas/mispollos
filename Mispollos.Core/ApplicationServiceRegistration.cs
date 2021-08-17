using Microsoft.Extensions.DependencyInjection;
using Mispollos.Application.Services;
using Mispollos.Domain.Contracts.Services;

namespace Mispollos.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProviderService, ProviderService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderItemService, OrderItemService>();

            return services;
        }
    }
}