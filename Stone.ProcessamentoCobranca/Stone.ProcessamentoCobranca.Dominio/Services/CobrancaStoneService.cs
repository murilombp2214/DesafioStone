using Microsoft.Extensions.Logging;
using Stone.ProcessamentoCobranca.Dominio.Entities;
using Stone.ProcessamentoCobranca.Dominio.Repository.Interfaces;
using Stone.ProcessamentoCobranca.Dominio.Services.Interfaces;
using Stone.ProcessamentoCobranca.Infra.CrossCutting.Utils;
using Stone.ProcessamentoCobranca.Infra.CrossCutting.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stone.ProcessamentoCobranca.Dominio.Services
{
    public class CobrancaStoneService : ICobrancaStoneService
    {
        private readonly ICobrancaStoneWriterRepository _cobrancaStoneWriterRepository;
        private readonly ILogger<ICobrancaStoneService> _logger;
        public CobrancaStoneService(ICobrancaStoneWriterRepository cobrancaStoneWriterRepository,
                                    ILogger<ICobrancaStoneService> logger)
        {
            _cobrancaStoneWriterRepository = cobrancaStoneWriterRepository;
            _logger = logger;
        }

        public async Task<IOperation<CobrancaStone>> RegistrarCobranca(CobrancaStone cobrancaStone)
        {
            if (cobrancaStone is null)
                return Result.CreateFailure<CobrancaStone>("Não é possivel registrar uma cobrança nula.");

            var cobrancaRegistada =  await _cobrancaStoneWriterRepository.RegistrarCobranca(cobrancaStone);
            if(cobrancaRegistada == null)
            {
                string cpfMask = ObtenhaCpfMascaradoParaLog(cobrancaStone.Cpf);
                string msg = $"Houve uma falha ao registrar a cobrança para o CPF {cpfMask}.";
                _logger.LogError(msg);
                return Result.CreateFailure<CobrancaStone>(msg);
            }
            return Result.CreateSuccess(cobrancaRegistada);
        }

        private string ObtenhaCpfMascaradoParaLog(in string cpf)
        {
            return $"{cpf.Substring(0, 3)}...{cpf.Substring(7, 3)}";
        }

    }
}
