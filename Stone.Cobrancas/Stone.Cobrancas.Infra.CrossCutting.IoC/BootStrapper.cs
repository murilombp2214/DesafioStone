
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Stone.Cobrancas.Aplicacacao.AppService;
using Stone.Cobrancas.Aplicacacao.AppService.Interfaces;
using Stone.Cobrancas.Dominio.Repository.Interface;
using Stone.Cobrancas.Dominio.Services;
using Stone.Cobrancas.Dominio.Services.Interfaces;
using Stone.Cobrancas.Dominio.Validations;
using Stone.Cobrancas.Dominio.Validations.Interfaces;
using Stone.Cobrancas.Infra.CrossCutting.Utils.Configuracoes;
using Stone.Cobrancas.Infra.CrossCutting.Utils.Interfaces;
using Stone.Cobrancas.Infra.CrossCutting.Utils.Masks;
using Stone.Cobrancas.Infra.Data.Constants;
using Stone.Cobrancas.Infra.Data.MongoDb;
using Stone.Cobrancas.Infra.Data.MongoDb.Configurations;
using Stone.Cobrancas.Infra.Data.MongoDb.Configurations.Interfaces;
using Stone.Cobrancas.Infra.Data.Query;
using Stone.Cobrancas.Infra.Data.Writer;
using System;
using System.Linq;
using System.Net.Mime;

namespace Stone.Cobrancas.Infra.CrossCutting.IoC
{
    public static class BootStrapper
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ICobrancaService, CobrancaService>()
                    .AddScoped<ICobrancaAppService, CobrancaAppService>()
                    .AddValidations()
                    .AddRepository();

            return services;
        }

        public static IServiceCollection AddHealthChecksApiCobranca(this IServiceCollection services)
        {
            services.AddHealthChecks()
                    .AddMongoDb(mongodbConnectionString: Environment.GetEnvironmentVariable(DataBaseConstants.CONNECTION_STRING),
                                                         Environment.GetEnvironmentVariable(DataBaseConstants.DATABASE_NAME),
                                                         HealthStatus.Unhealthy);
            return services;
        }

        public static void UseHealthChecksApiCobranca(this IApplicationBuilder app)
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

        private static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddSingleton(x =>
            {
                var db = MongoDatabaseProvider.GetDatabase(Environment.GetEnvironmentVariable(DataBaseConstants.CONNECTION_STRING),
                                                           Environment.GetEnvironmentVariable(DataBaseConstants.DATABASE_NAME));
                return db;
            });

            services.AddSingleton<ICobrancaQueryRepositoryConfiguration, CobrancaQueryRepositoryConfiguration>(x =>
                                    new CobrancaQueryRepositoryConfiguration(int.Parse(Environment.GetEnvironmentVariable(DataBaseConstants.TAMANHO_PAGINACAO) ?? "0")));

            services.AddScoped<ICobrancaWriterRepository, CobrancaWriterRepository>();
            services.AddScoped<ICobrancaQueryRepository, CobrancaQueryRepository>();

            return services;
        }

        public static IServiceCollection AddUtil(this IServiceCollection services, bool isDevelopment)
        {
            if (isDevelopment)
                VariaveisDeAmbienteLocal.CarregarVariaveis();

            services.AddSingleton<ICpfMask, CpfMask>();

            return services;
        }

        private static IServiceCollection AddValidations(this IServiceCollection services)
        {
            services.AddSingleton<ICobrancaValidation, CobrancaValidation>()
                    .AddSingleton<ICpfValidation, CpfValidation>()
                    .AddSingleton<IConsultarCobrancasValidation, ConsultarCobrancasValidation>();
            return services;

        }
    }
}
