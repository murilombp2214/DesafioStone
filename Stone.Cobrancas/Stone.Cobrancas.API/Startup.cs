using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stone.Cobrancas.API.Configurations.Swagger;
using Stone.Cobrancas.API.Middlewares;
using Stone.Cobrancas.Infra.CrossCutting.IoC;

namespace Stone.Cobrancas.API
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
                    .AddHealthChecksApiCobranca();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGlobalExceptionHandlerMiddleware();

            app.UseCors();

            app.UseSwaggerSetup();

            app.UseRouting();

            app.UseAuthorization();

            app.UseHealthChecksApiCobranca();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
