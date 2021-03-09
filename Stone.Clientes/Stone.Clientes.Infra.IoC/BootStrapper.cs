using HealthChecks.MongoDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Stone.Clientes.Aplicacacao.AppService;
using Stone.Clientes.Aplicacacao.AppService.Interfaces;
using Stone.Clientes.Dominio.Repository.Interfaces;
using Stone.Clientes.Dominio.Services;
using Stone.Clientes.Dominio.Services.Interfaces;
using Stone.Clientes.Dominio.Validations;
using Stone.Clientes.Dominio.Validations.Interfaces;
using Stone.Clientes.Infra.CrossCutting.Utils.Configuracoes;
using Stone.Clientes.Infra.CrossCutting.Utils.Interfaces;
using Stone.Clientes.Infra.CrossCutting.Utils.Masks;
using Stone.Clientes.Infra.Data.Constants;
using Stone.Clientes.Infra.Data.MongoDb;
using Stone.Clientes.Infra.Data.MongoDb.Configurations;
using Stone.Clientes.Infra.Data.MongoDb.Configurations.Interfaces;
using Stone.Clientes.Infra.Data.Query;
using Stone.Clientes.Infra.Data.Repository;
using System;
using System.Linq;
using System.Net.Mime;

namespace Stone.Clientes.Infra.CrossCutting.IoC
{
    public static class BootStrapper
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IClienteService, ClienteService>()
                    .AddScoped<IClienteAppService, ClienteAppService>()
                    .AddValidations()
                    .AddRepository();

            return services;
        }

        public static IServiceCollection AddHealthChecksApiCliente(this IServiceCollection services)
        {
            services.AddHealthChecks()
                    .AddMongoDb(mongodbConnectionString: Environment.GetEnvironmentVariable(DataBaseConstants.CONNECTION_STRING), 
                                                         Environment.GetEnvironmentVariable(DataBaseConstants.DATABASE_NAME),
                                                         HealthStatus.Unhealthy);
            return services;
        }

        public static void UseHealthChecksApiCliente(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/hc",
                new HealthCheckOptions
                {
                    ResponseWriter = async (context, report) =>
                    {
                        var result = JsonConvert.SerializeObject(
                            new
                            {
                                status = report.Status.ToString(),
                                errors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) })
                            });
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(result);
                    }
                });
        }

        private static IServiceCollection AddValidations(this IServiceCollection services)
        {
            services.AddSingleton<IClienteValidation, ClienteValidation>()
                    .AddSingleton<ICpfValidation, CpfValidation>();
            return services;
        }

        private static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddSingleton(x =>
            {
                var db = MongoDatabaseProvider.GetDatabase(Environment.GetEnvironmentVariable(DataBaseConstants.CONNECTION_STRING),
                                                           Environment.GetEnvironmentVariable(DataBaseConstants.DATABASE_NAME));
                return db;
            });


            services.AddSingleton<IClienteQueryRepositoryConfiguration, ClienteQueryRepositoryConfiguration>(x =>
                                    new ClienteQueryRepositoryConfiguration(int.Parse(Environment.GetEnvironmentVariable(DataBaseConstants.TAMANHO_PAGINACAO) ?? "0" )));


            services.AddScoped<IClienteWriterRepository, ClienteWriterRepository>();
            services.AddScoped<IClienteQueryRepository, ClienteQueryRepository>();

            return services;
        }

        public static IServiceCollection AddUtil(this IServiceCollection services, bool isDevelopment)
        {
            if (isDevelopment)
                VariaveisDeAmbienteLocal.CarregarVariaveis();

            services.AddSingleton<ICpfMask, CpfMask>();

            return services;
        }

    }
}
