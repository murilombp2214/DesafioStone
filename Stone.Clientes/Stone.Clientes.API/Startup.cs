using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stone.Clientes.API.Configurations.Swagger;
using Stone.Clientes.API.Middlewares;
using Stone.Clientes.Infra.CrossCutting.IoC;

namespace Stone.Clientes.API
{
    public class Startup
    {
        private readonly bool isDevelopment;
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            isDevelopment = env.IsDevelopment();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddGlobalExceptionHandlerMiddleware();
            services.AddSwaggerConfiguration();
            services.RegisterServices()
                    .AddUtil(isDevelopment)
                    .AddHealthChecksApiCliente();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGlobalExceptionHandlerMiddleware();

            app.UseSwaggerSetup();

            app.UseCors();

            app.UseRouting();

            app.UseAuthorization();

            app.UseHealthChecksApiCliente();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
