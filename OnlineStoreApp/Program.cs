using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Repository.EFCore.DataContext;
using OnlineStoreApp.Repository.EFCore.Repositories;
using OnlineStoreApp.UseCases.Helpers;
using OnlineStoreApp.UseCases.Interfaces;
using OnlineStoreApp.UseCases.Services;
using OnlineStoreApp.UseCases.UseCases;
using System.Text;

namespace OnlineStoreApp
{
    public class Programs
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.           

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<KeyValuesConfiguration>(options => builder.Configuration.GetSection("KeyValuesConfiguration").Bind(options));
            builder.Services.Configure<JwtSettings>(options => builder.Configuration.GetSection("JwtSettings").Bind(options));
            builder.Services.Configure<EmailProvider>(options => builder.Configuration.GetSection("EmailProviderSettings").Bind(options));

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer("name=DefaultConnection"));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),
                ValidAudience = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:SecretKey")))
            };
            builder.Services.AddSingleton(tokenValidationParameters);

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("IsAdmin", policy => policy.RequireClaim("isAdmin"));
                options.AddPolicy("IsUser", policy => policy.RequireClaim("isUser"));
                options.AddPolicy("IsAdminOrUser", policy => policy.RequireClaim("logUser", "isAdmin", "isUser"));
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = tokenValidationParameters;
            });
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StoreOnlineApp", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference  = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{ }
                    }
                });
            });

            builder.Services.AddTransient<IManageCategoryUseCase, ManageCategoryUseCase>();
            builder.Services.AddTransient<IManageFoodUseCase, ManageFoodUseCase>();
            builder.Services.AddTransient<IMakeOrderUseCase, MakeOrderUseCase>();
            builder.Services.AddTransient<ISenderEmailService, SenderEmailService>();
            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddTransient<ILoginService, LoginService>();
            builder.Services.AddTransient<IUserService, UserService>();

            builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
            builder.Services.AddTransient<IFoodRepository, FoodRepository>();
            builder.Services.AddTransient<IOrderRepository, OrderRepository>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<ITokenRepository, TokenRepository>();
            builder.Services.AddTransient<IUnitOfWorkRepository, UnitOfWorkRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.Run();
        }
    }
}
