using Microsoft.Extensions.DependencyInjection;
using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Repository.EFCore.Repositories;

namespace OnlineStoreApp.Repository.EFCore.Extensions
{
    public static class RepositoriesEFCoreExtension
    {
        public static IServiceCollection AddEFCoreRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IFoodRepository, FoodRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IUnitOfWorkRepositories, UnitOfWorkRepositories>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUnitOfWorkAdapter, UnitOfWorkAdapter>();

            return services;
        }
    }
}
