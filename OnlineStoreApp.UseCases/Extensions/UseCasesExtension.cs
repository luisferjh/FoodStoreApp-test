using Microsoft.Extensions.DependencyInjection;
using OnlineStoreApp.UseCases.Interfaces;
using OnlineStoreApp.UseCases.Services;
using OnlineStoreApp.UseCases.UseCases;

namespace OnlineStoreApp.UseCases.Extensions
{
    public static class UseCasesExtension
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IManageCategoryUseCase, ManageCategoryUseCase>();
            services.AddScoped<IManageFoodUseCase, ManageFoodUseCase>();
            services.AddScoped<IMakeOrderUseCase, MakeOrderUseCase>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ISenderEmailService, SenderEmailService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
