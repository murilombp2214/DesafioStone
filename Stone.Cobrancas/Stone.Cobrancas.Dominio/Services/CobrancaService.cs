using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Stone.Cobrancas.Dominio.Entities;
using Stone.Cobrancas.Dominio.Repository.Interface;
using Stone.Cobrancas.Dominio.Services.Interfaces;
using Stone.Cobrancas.Dominio.Validations.Interfaces;
using Stone.Cobrancas.Infra.CrossCutting.Utils;
using Stone.Cobrancas.Infra.CrossCutting.Utils.Interfaces;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace Stone.Cobrancas.Dominio.Services
{
    public class CobrancaService : ICobrancaService
    {

        private readonly ICobrancaValidation _cobrancaValidation;
        private readonly IConsultarCobrancasValidation _consultarCobrancaValidation;
        private readonly ICobrancaWriterRepository _cobrancaWriterRepository;
        private readonly ICobrancaQueryRepository _cobrancaQueryRepository;
        private readonly ILogger<CobrancaService> _logger;
        public CobrancaService(ICobrancaValidation cobrancaValidation,
                               IConsultarCobrancasValidation consultarCobrancaValidation,
                               ICobrancaWriterRepository cobrancaWriterRepository,
                               ICobrancaQueryRepository cobrancaQueryrRepository,
                               ILogger<CobrancaService> logger)
        {
            _cobrancaValidation = cobrancaValidation;
            _consultarCobrancaValidation = consultarCobrancaValidation;
            _cobrancaWriterRepository = cobrancaWriterRepository;
            _cobrancaQueryRepository = cobrancaQueryrRepository;
            _logger = logger;
        }
        public async Task<IOperation<Cobranca>> Cadastrar(Cobranca cobranca)
        {
            var operation = _cobrancaValidation.Validar(cobranca);
            if (operation is OperationFail<Cobranca>)
                return operation;

            var cobrancaRegistrada = await _cobrancaWriterRepository.Cadastrar(cobranca);
            if (cobrancaRegistrada == null)
                return Result.CreateFailure<Cobranca>("Houve um erro ao registrar a cobrança.");

            RegistrarLogCobranca(cobrancaRegistrada);
            return Result.CreateSuccess(cobrancaRegistrada);
        }

        public async Task<IOperation<List<Cobranca>>> ConsultarCobrancas(string cpf, int pagina)
        {
            var operation = _consultarCobrancaValidation.Validar(cpf, pagina);
            if (operation is OperationFail<List<Cobranca>>)
                return operation;

            var listaCobrancas = await _cobrancaQueryRepository.ConsultarCobrancas(cpf, pagina);
            if(listaCobrancas == null)
                return Result.CreateFailure<List<Cobranca>>("Houve um erro ao consultar as cobrança.");

            return Result.CreateSuccess(listaCobrancas);

        }

        public async Task<IOperation<List<Cobranca>>> ConsultarCobrancas(int mes, int pagina)
        {
            var operation = _consultarCobrancaValidation.Validar(mes, pagina);

            if (operation is OperationFail<List<Cobranca>>)
                return operation;

            var listaCobrancas = await _cobrancaQueryRepository.ConsultarCobrancas(mes, pagina);
            if (listaCobrancas == null)
                return Result.CreateFailure<List<Cobranca>>("Houve um erro ao consultar as cobrança.");

            return Result.CreateSuccess(listaCobrancas);
        }

        private void RegistrarLogCobranca(Cobranca cobrancaRegistrada)
        {
            try
            {
                var json = new ExpandoObject();
                json.TryAdd("valor-cobranca", cobrancaRegistrada.ValorCobranca);
                json.TryAdd("cpf", $"{cobrancaRegistrada.Cpf.Substring(0, 3)}...{cobrancaRegistrada.Cpf.Substring(8, 3)}");
                json.TryAdd("data-vencimento", cobrancaRegistrada.DataVencimento);
                _logger.LogInformation(JsonConvert.SerializeObject(json));
            }
            catch
            {
                _logger.LogWarning("Houve um problema no registro do log.");
            }
        }
    }
}
