using Microsoft.Extensions.DependencyInjection;
using Stone.ProcessamentoCobranca.Dominio.Repository.Interfaces;
using Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCliente;
using Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCliente.Configurations;
using Stone.ProcessamentoCobranca.Infra.Data.Clients.Polices.Interfaces;
using Stone.ProcessamentoCobranca.Infra.Data.Clients.Polices;
using Stone.ProcessamentoCobranca.Infra.CrossCutting.Utils.Configuracoes;
using Stone.ProcessamentoCobranca.Dominio.Services.Interfaces;
using Stone.ProcessamentoCobranca.Dominio.Services;
using Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCobranca;
using Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCobranca.Configuration;
using Stone.ProcessamentoCobranca.Dominio.Queues.Interfaces;
using Stone.ProcessamentoCobranca.Infra.Data.Queues;

namespace Stone.ProcessamentoCobranca.Infra.CrossCutting.IoC
{
    public static class BootStrapper
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.RegisterPoliciesFactoty()
                    .RegisterHttpClientApiCliente()
                    .RegisterHttpClientApiCobranca()
                    .RegisterFilas()
                    .AddSingleton<IClienteStoneService, ClienteStoneService>()
                    .AddSingleton<ICobrancaStoneService, CobrancaStoneService>()
                    .AddSingleton< IFaturamentoStoneService,FaturamentoStoneService>();

            return services;
        }

        private static IServiceCollection RegisterFilas(this IServiceCollection services)
        {
            services.AddSingleton<IQueueFaturamentoConsumer, QueueFaturamentoConsumer>();
            services.AddSingleton<IQueueFaturamentoProducer, QueueFaturamentoProducer>();
            return services;
        }

        private static IServiceCollection RegisterPoliciesFactoty(this IServiceCollection services)
        {
            services.AddScoped<IPoliciesFactoty, PoliciesFactoty>();
            return services;
        }

        private static IServiceCollection RegisterHttpClientApiCliente(this IServiceCollection services)
        {
            services.AddHttpClient<IClienteStoneQueryRepository,
                                  ClienteStoneQueryClient>(ConfigurationApiCliente.ConfigureHttpClient)
                                  .AddPolicyHandler((service, request) => service.GetService<IPoliciesFactoty>().Retry())
                                  .AddPolicyHandler((service, request) => service.GetService<IPoliciesFactoty>().Timeout());
            return services;
        }

        private static IServiceCollection RegisterHttpClientApiCobranca(this IServiceCollection services)
        {
            services.AddHttpClient<ICobrancaStoneWriterRepository,
                                  CobrancaStoneWriterClient>(ConfigurationApiCobranca.ConfigureHttpClient)
                                  .AddPolicyHandler((service, request) => service.GetService<IPoliciesFactoty>().Retry())
                                  .AddPolicyHandler((service, request) => service.GetService<IPoliciesFactoty>().Timeout());
            return services;
        }

        public static IServiceCollection AddUtil(this IServiceCollection services, bool isDevelopment)
        {
            if (isDevelopment)
                VariaveisDeAmbienteLocal.CarregarVariaveis();

            return services;
        }
    }
}
