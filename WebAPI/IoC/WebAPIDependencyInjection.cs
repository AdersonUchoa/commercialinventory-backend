using Application.Responses;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace WebAPI.IoC
{
    public static class WebAPIDependencyInjection
    {
        public static void AddWebApi(this IServiceCollection services)
        {
            AddApiVersioning(services);
            AddSwagger(services);
            AddCors(services);
            AddControllers(services);
        }

        private static void AddApiVersioning(IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CommercialInventory API",
                    Version = "v1",
                    Description = "API RESTful de Persistência e Ciclo de Vida do Catálogo (.NET)",
                    Contact = new OpenApiContact
                    {
                        Name = "Aderson Uchoa",
                        Email = "iamuchoa@gmail.com"
                    }
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira o token JWT desta forma: Bearer {seu token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        private static void AddCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CommercialInventoryCors", builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                           .WithMethods("GET", "POST", "PUT", "DELETE")
                           .WithHeaders("Content-Type", "Authorization");
                });
            });
        }

        private static void AddControllers(IServiceCollection services)
        {
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                        .Where(m => m.Value != null && m.Value.Errors.Any())
                        .SelectMany(m => m.Value!.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                        var summary = string.Join(" | ", errors);

                        var response = new ApiResponse<object>(false, System.Net.HttpStatusCode.BadRequest, null, "A requisição possui dados inválidos.", summary);

                        return new BadRequestObjectResult(response);
                    };
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                });
        }
    }
}
