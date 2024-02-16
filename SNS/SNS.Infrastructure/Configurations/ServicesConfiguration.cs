﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Primitives;
using Primitives.Command;
using Primitives.DomainEvent;
using Primitives.Query;
using Shared.Storage.Configurations;
using SNS.Domain.AlbumEntity;
using SNS.Domain.ImageAggregate;
using SNS.Domain.UserEntity;
using SNS.Infrastructure.DomainRepositories;
using SNS.Infrastructure.EventBus;
using SNS.Infrastructure.Persistence;
using System.Reflection;

namespace SNS.Infrastructure.Configurations
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigureDbContext(
            this IServiceCollection services,
            string connectionString
        )
        {
            services.AddDbContext<SNSDbContext>(
                options => options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention()
            );
            return services;
        }

        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection ConfigureMediator(this IServiceCollection services)
        {
            services.AddScoped<IQueryRequestSender, InternalEventBus>();
            services.AddScoped<ICommandRequestSender, InternalEventBus>();
            services.AddScoped<IDomainEventPublisher, InternalEventBus>();

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly);
            });
            return services;
        }

        public static IServiceCollection ConfigureEventBus(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var config = configuration.GetSection("EventBus");

            services.AddCap(x =>
            {
                x.UseEntityFramework<SNSDbContext>();
                x.UseRabbitMQ(options =>
                {
                    options.Port = config.GetValue<int>("Port");
                    options.HostName = config["HostName"]!;
                    options.UserName = config["UserName"]!;
                    options.Password = config["Password"]!;
                });
            });

            return services;
        }

        public static IServiceCollection ConfigureStorages(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.ConfigureAvatarStorage(configuration);
            services.ConfigureHeaderStorage(configuration);
            return services;
        }

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                var scheme = new OpenApiSecurityScheme
                {
                    Description = "Authorization Header \r\nExample:'Bearer 123456789'",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Authorization"
                    },
                    Scheme = "oauth2",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                };
                options.AddSecurityDefinition("Authorization", scheme);
                var requirement = new OpenApiSecurityRequirement();
                requirement[scheme] = new List<string>();
                options.AddSecurityRequirement(requirement);
                var xmlFilename = $"{Assembly.GetEntryAssembly()!.GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            return services;
        }
    }
}
