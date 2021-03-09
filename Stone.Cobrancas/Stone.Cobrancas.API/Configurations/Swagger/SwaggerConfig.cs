using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Stone.Cobrancas.API.Configurations.Swagger
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "API de Cobranças",
                        Version = "v1",
                        Description = "API REST criada com o ASP.NET Core 3.1",
                        Contact = new OpenApiContact
                        {
                            Name = "Murilo Barros Peixoto",
                        }
                    });
                c.EnableAnnotations();
            });
        }

        public static void UseSwaggerSetup(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint($"/swagger/v1/swagger.json", "API de Cobranças"));
        }
    }
}
