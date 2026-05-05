using System.Reflection;
using System.Text;
using DesignAutomation.API.Common.Behaviors;
using DesignAutomation.API.Common.Identity;
using DesignAutomation.API.Common.Middleware;
using DesignAutomation.API.Common.OAuthState;
using DesignAutomation.API.Data;
using DesignAutomation.API.Data.Seed;
using DesignAutomation.API.Models;
using DesignAutomation.API.Services;
using DesignAutomation.API.Services.Aps;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace DesignAutomation.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var assembly = Assembly.GetExecutingAssembly();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            var jwt = builder.Configuration.GetSection("Jwt");
            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwt["Issuer"],
                        ValidAudience = jwt["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)),
                        ClockSkew = TimeSpan.Zero,
                    };
                });

            builder.Services.AddAuthorization();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddScoped<ITokenService, TokenService>();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            builder.Services.AddValidatorsFromAssembly(assembly);

            builder.Services.Configure<ApsOptions>(builder.Configuration.GetSection(ApsOptions.SectionName));
            builder.Services.AddMemoryCache();
            builder.Services.AddHttpClient<IApsAuthService, ApsAuthService>((sp, http) =>
            {
                var opts = sp.GetRequiredService<IOptions<ApsOptions>>().Value;
                http.BaseAddress = new Uri(opts.BaseUrl);
            });
            builder.Services.AddHttpClient<IApsOssService, ApsOssService>((sp, http) =>
            {
                var opts = sp.GetRequiredService<IOptions<ApsOptions>>().Value;
                http.BaseAddress = new Uri(opts.BaseUrl);
            });
            builder.Services.AddHttpClient<IApsOAuthService, ApsOAuthService>((sp, http) =>
            {
                var opts = sp.GetRequiredService<IOptions<ApsOptions>>().Value;
                http.BaseAddress = new Uri(opts.BaseUrl);
            });

            builder.Services.AddDataProtection()
                .SetApplicationName("DesignAutomation.API");
            builder.Services.AddSingleton<IOAuthStateService, OAuthStateService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DesignAutomation.API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter the JWT token (no 'Bearer ' prefix needed).",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        Array.Empty<string>()
                    },
                });
            });

            var app = builder.Build();

            await DbSeeder.SeedAsync(app.Services);

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
