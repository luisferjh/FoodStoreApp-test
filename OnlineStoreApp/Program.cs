using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using OnlineStoreApp.Extensions;
using OnlineStoreApp.Repository.EFCore.DataContext;
using OnlineStoreApp.Repository.EFCore.Extensions;
using OnlineStoreApp.UseCases.Extensions;
using OnlineStoreApp.UseCases.Helpers;
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

            builder.Services.AddEFCoreRepositories();
            builder.Services.AddUseCases();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StoreOnlineApp", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });
            });

            // Generates one AITool per OpenAPI operation of this API, ready to be exposed via an MCP server.
            builder.Services.AddOpenApiAITools();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() ||
                app.Environment.IsProduction() ||
                app.Environment.IsStaging())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.ApplyMigrations();

                // Diagnostics endpoint: lists the generated AITools plus any operation that could not be cleanly mapped.
                app.MapGet("/ai-tools", async (IOpenApiAIToolsProvider toolsProvider, CancellationToken cancellationToken) =>
                {
                    var result = await toolsProvider.GetToolsAsync(cancellationToken);
                    return Results.Ok(new
                    {
                        Tools = result.Tools.Select(t => new { t.Name, t.Description }),
                        result.Issues
                    });
                }).WithName("GetAITools");
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
