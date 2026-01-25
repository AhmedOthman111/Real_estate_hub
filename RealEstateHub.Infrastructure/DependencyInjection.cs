using FluentValidation;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RealEstateHub.Application.Common;
using RealEstateHub.Application.Interfaces;
using RealEstateHub.Application.Interfaces.IRep;
using RealEstateHub.Infrastructure.Data;
using RealEstateHub.Infrastructure.ExternalServices;
using RealEstateHub.Infrastructure.Identity;
using RealEstateHub.Infrastructure.Persistence;
using RealEstateHub.Infrastructure.Persistence.Configurations;
using RealEstateHub.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
           
            // Register DbContext

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });


            // Register Identity

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;

                options.SignIn.RequireConfirmedEmail = true;

                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


            // Register JWT

            services.AddAuthentication(options =>
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

                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });


            //Redis caching
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = "RealEstateHub_";

            });


            //hangfire
            services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();


            // register mediatar

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly));


            // register Validators

            services.AddValidatorsFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            
            
            
            
            // Register Repositories

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAdRepository, AdRepository>();
            services.AddScoped<IOwnerRepository, OwnerRepository>();
            services.AddScoped<ICategoryRepository , CategoryRepository>();
            services.AddScoped<IAdPhotosRepository , AdPhotosRepository>();
            services.AddScoped<ICommentRepository , CommentRepository>();
            services.AddScoped<IReplyRepository , ReplyRepository>();


            


            
            // Unit of Work 
            services.AddScoped<IUnitOfWork, UnitOfWork>();



            //Register Services
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IEmailService, EmailService>();
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));

            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<ICacheService, RedisCacheService>(); 

            services.AddScoped<IAdService , Adservice>();

            services.AddScoped<IPhotoStorageService , LocalPhotoStorageService>();  

            services.AddScoped<IAdPhotosService , AdPhotosService>();
            
            services.AddScoped<ICommentReplyService , CommentReplyService>();








            return services;
        }

    }
}
