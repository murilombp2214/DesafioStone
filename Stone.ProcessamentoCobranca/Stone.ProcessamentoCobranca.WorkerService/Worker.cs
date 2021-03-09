using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stone.ProcessamentoCobranca.Dominio.Entities;
using Stone.ProcessamentoCobranca.Dominio.Services.Interfaces;
using Stone.ProcessamentoCobranca.Infra.CrossCutting.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stone.ProcessamentoCobranca.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly IFaturamentoStoneService _faturamento;


        public Worker(IFaturamentoStoneService faturamento)
        {
            _faturamento = faturamento;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                await _faturamento.GerarCobrancasParaClientesStone();
                await Task.Delay(1000, stoppingToken);
                await _faturamento.GerarCobrancasParaClientesStoneEnfilerados();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
