using Microsoft.Extensions.Logging;
using Stone.ProcessamentoCobranca.Dominio.Entities;
using Stone.ProcessamentoCobranca.Dominio.Queues.Interfaces;
using Stone.ProcessamentoCobranca.Dominio.Services.Interfaces;
using Stone.ProcessamentoCobranca.Infra.CrossCutting.Utils;
using Stone.ProcessamentoCobranca.Infra.CrossCutting.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stone.ProcessamentoCobranca.Dominio.Services
{
    public class FaturamentoStoneService : IFaturamentoStoneService
    {
        private readonly IClienteStoneService _clienteStoneService;
        private readonly ICobrancaStoneService _cobrancaStoneService;
        private readonly IQueueFaturamentoProducer _producer;
        private readonly IQueueFaturamentoConsumer _consumer;
        private readonly ILogger<IFaturamentoStoneService> _logger;


        public FaturamentoStoneService(IClienteStoneService clienteStoneService,
                                       ICobrancaStoneService cobrancaStoneService,
                                       IQueueFaturamentoProducer producer,
                                       IQueueFaturamentoConsumer consumer,
                                       ILogger<IFaturamentoStoneService> logger)
        {
            _clienteStoneService = clienteStoneService;
            _cobrancaStoneService = cobrancaStoneService;
            _producer = producer;
            _consumer = consumer;
            _logger = logger;

        }
        public async Task GerarCobrancasParaClientesStone()
        {
            try
            {
                int pagina = 1;
                bool todosOsClientesForamBuscados = false;
                while (!todosOsClientesForamBuscados)
                {
                    var clientes = await _clienteStoneService.ObterClientes(pagina);
                    if (clientes is OperationSuccess<List<ClienteStone>> sucessOperation)
                    {
                        if (sucessOperation.Data.Count == 0)
                        {
                            todosOsClientesForamBuscados = true;
                        }
                        else
                        {
                            Parallel.ForEach(sucessOperation.Data, async (clienteStone, state) =>
                            {
                                var cobrancaStone = new CobrancaStone(DateTime.Now.AddDays(3),
                                                        clienteStone.Cpf,
                                                        CalculeValorCobranca(clienteStone.Cpf));
                                await ProcesseCobranca(cobrancaStone);
                            });
                            pagina++;
                        }
                    }
                    else
                    {
                        todosOsClientesForamBuscados = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Houve um erro ao gerar as cobranças para os clientes", ex);
            }

        }
        public Task GerarCobrancasParaClientesStoneEnfilerados()
        {
            try
            {
                var cobrancasEnfileradas = _consumer.ObtenhaCobrancasNaoProcessadas();

                Parallel.ForEach(cobrancasEnfileradas, async (cobrancaStone, state) =>
                {
                    await ProcesseCobranca(cobrancaStone);
                });

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError("Houve um erro ao gerar as cobranças enfileradas.", ex);
            }

            return Task.CompletedTask;

            
        }

        private async Task ProcesseCobranca(CobrancaStone cobrancaStone)
        {
            var cobrancaRegistada = await _cobrancaStoneService.RegistrarCobranca(cobrancaStone);
            if (cobrancaRegistada is OperationFail<CobrancaStone> )
            {
                await _producer.EnfilerarCobranca(cobrancaStone);
            }
        }

        private decimal CalculeValorCobranca(string cpf)
        {
            return decimal.Parse(cpf.Substring(0, 2) + cpf.Substring(8, 2));
        }


    }
}
