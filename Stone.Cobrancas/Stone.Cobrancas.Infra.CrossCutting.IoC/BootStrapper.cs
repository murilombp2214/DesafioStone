
using Microsoft.Extensions.DependencyInjection;
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
