using CRS.ChannelManager.Application;
using CRS.ChannelManager.Domain;
using CRS.ChannelManager.Domain.Dtos;
using CRS.ChannelManager.Infrastructure;
using CRS.ChannelManager.Library.Base;
using CRS.ChannelManager.Library.BaseEntities;
using CRS.ChannelManager.Library.BaseInterface;
using Elasticsearch.Net;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nest;
using Npgsql;
using System.Data;
using static CRS.ChannelManager.Domain.Dtos.ConfigSettingDto;

namespace CRS.ChannelManager.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAuthenticationExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            IdentityModelEventSource.ShowPII = true;
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = bool.Parse(configuration.GetValue<string>("Authentication:RequireHttpsMetadata"));
                options.Authority = configuration.GetValue<string>("Authentication:Authority", string.Empty);
                options.IncludeErrorDetails = bool.Parse(configuration.GetValue<string>("Authentication:IncludeErrorDetails"));
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = bool.Parse(configuration.GetValue<string>("Authentication:ValidateAudience")),
                    ValidAudience = configuration.GetValue<string>("Authentication:ValidAudience"),
                    ValidateIssuerSigningKey = bool.Parse(configuration.GetValue<string>("Authentication:ValidateIssuerSigningKey")),
                    ValidateIssuer = bool.Parse(configuration.GetValue<string>("Authentication:ValidateIssuer")),
                    ValidIssuer = configuration.GetValue<string>("Authentication:ValidIssuer"),
                    ValidateLifetime = bool.Parse(configuration.GetValue<string>("Authentication:ValidateLifetime")),
                    NameClaimType = "preferred_username"
                };
            });
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "CRS Channel Manager API", Description = "API for CRS Channel Manager", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }

        public static IServiceCollection AddExtensionHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks();
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string? assemblyMigration = typeof(CRSChannelManagerContext).Namespace;
            string strConnection = configuration.GetConnectionString("DefaultConnectionString") ?? string.Empty;
            services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(strConnection));
            services.AddDbContext<CRSChannelManagerContext>(options => options.UseNpgsql(strConnection, optionsBuilder => optionsBuilder.MigrationsAssembly(assemblyMigration)));
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            ApplicationExtensions.AddLayer(services);
            InfrastructureExtensions.DependencyInjection(services);
            services.AddFastEndpoints(o =>
            {
                o.IncludeAbstractValidators = true;
            })
            .SwaggerDocument(o =>
            {
                o.AutoTagPathSegmentIndex = 0;
                o.TagStripSymbols = true;
                o.RemoveEmptyRequestSchema = false;
            });

            return services;
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddDbContext<IDbContext, CRSChannelManagerContext>();
            services.AddDbContext<DbContext, CRSChannelManagerContext>();
            services.AddDbContext<CRSChannelManagerContext, CRSChannelManagerContext>();
            return services;
        }

        private const string kafkaConfig = "kafkaConfig";
        private const string kafkaProducerConfig = "kafkaProducerConfig";
        private const string kafkaProducerInventoryConfig = "kafkaProducerInventoryConfig";
        private const string inventoryConfig = "Inventory";
        private const string permissionConfig = "PermissionConfig";
        private const string ssoConfig = "SSOConfig";

        public static IServiceCollection AddConfigure(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<ConfigSettingDto.kafkaConsumerConfig>(configuration.GetSection(kafkaConfig));
            services.Configure<ConfigSettingDto.kafkaProducerConfig>(configuration.GetSection(kafkaProducerConfig));
            services.Configure<ConfigSettingDto.kafkaProducerConfig>(configuration.GetSection(kafkaProducerInventoryConfig));
            services.Configure<ConfigSettingDto.InventoryConfig>(configuration.GetSection(inventoryConfig));
            services.Configure<ConfigSettingDto.PermissionConfig>(configuration.GetSection(permissionConfig));
            services.Configure<ConfigSettingDto.SSOConfig>(configuration.GetSection(ssoConfig));
            return services;
        }

        public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                var url = configuration["ElasticSearch:Uri"];
                var defaultIndex = configuration["ElasticSearch:Index"];
                var userName = configuration["ElasticSearch:Username"];
                var pass = configuration["ElasticSearch:Password"];

                var settings = new ConnectionSettings(new SingleNodeConnectionPool(new Uri(url)))
                    .BasicAuthentication(userName, pass)
                    .PrettyJson()
                    .DefaultIndex(defaultIndex)
                    .EnableApiVersioningHeader();
                settings.DefaultMappingFor<AuditEntry>(x => x);

                //AddDefaultMappings(settings);

                var client = new ElasticClient(settings);
                // Test the connection
                var pingResponse = client.Ping();
                if (!pingResponse.IsValid) { throw new DomainExceptionBase("Elasticsearch connection failed."); }
                client.Indices.Create(defaultIndex, index => index.Map<AuditEntry>(x => x.AutoMap()));
                services.AddSingleton<IElasticClient>(client);

                //CreateIndex(client, defaultIndex);
            }
            catch (Exception ex)
            {

            }

        }
    }
}
