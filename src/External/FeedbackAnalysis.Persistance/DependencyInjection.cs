using FeedbackAnalysis.Domain.Abstractions.Repository.Base;
using FeedbackAnalysis.Domain.Entities;
using FeedbackAnalysis.Persistance.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FeedbackAnalysis.Persistance
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRepository<RefreshToken>, BaseRepository<RefreshToken>>();
            services.AddScoped<IRepository<User>, BaseRepository<User>>();
            services.AddScoped<IRepository<Feedback>, BaseRepository<Feedback>>();

            services.AddDbContext<FeedbackDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetValue<string>("SqlConnection"));
            });

            services.AddIdentityCore<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
                .AddEntityFrameworkStores<FeedbackDbContext>();

            var tokenValidationParametres = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),

                ValidateIssuer = true,
                ValidIssuer = configuration["JWT:Issuer"],

                ValidateAudience = true,
                ValidAudience = configuration["JWT:Audience"],

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(30)
            };
            services.AddSingleton(tokenValidationParametres);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = tokenValidationParametres;
            });
            return services;
        }
    }
}
