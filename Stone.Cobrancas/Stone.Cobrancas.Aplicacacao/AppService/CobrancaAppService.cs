using Stone.Cobrancas.Aplicacacao.AppService.Interfaces;
using Stone.Cobrancas.Aplicacacao.Mappers;
using Stone.Cobrancas.Aplicacacao.Request;
using Stone.Cobrancas.Aplicacacao.Response;
using Stone.Cobrancas.Dominio.Entities;
using Stone.Cobrancas.Dominio.Services.Interfaces;
using Stone.Cobrancas.Infra.CrossCutting.Utils;
using Stone.Cobrancas.Infra.CrossCutting.Utils.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stone.Cobrancas.Aplicacacao.AppService
{
    public class CobrancaAppService : ICobrancaAppService
    {
        private readonly ICobrancaService _cobrancaService;
        private readonly ICpfMask _cpfMask;
        public CobrancaAppService(ICobrancaService cobrancaService, ICpfMask cpfMask)
        {
            _cobrancaService = cobrancaService;
            _cpfMask = cpfMask;
        }
        public async Task<IOperation<CobrancaResponse>> Cadastrar(CobrancaRequest request)
        {
            if (request is null)
                return Result.CreateFailure<CobrancaResponse>("Requisição vazia");

            request.Cpf = _cpfMask.RemoveMaskCpf(request.Cpf);
            var cobranca = CobrancaRequestMapper.ConverterCobrancaRequestEmCobranca(request);
            var operation = await _cobrancaService.Cadastrar(cobranca);

            if (operation is OperationFail<Cobranca> operationFail)
                return Result.CreateFailure<CobrancaResponse>(operationFail.Mensagens.Mensagem, operationFail.Mensagens.Campos);

            var operationSucess = operation as OperationSuccess<Cobranca>;
            var cobrancaResponse = CobrancaResponseMapper.ConverterCobrancaEmCobrancaResponse(operationSucess.Data);

            return Result.CreateSuccess(cobrancaResponse);

        }

        public async Task<IOperation<IEnumerable<CobrancaResponse>>> ConsultarCobrancasPorCpf(string cpf, int pagina)
        {
            var operationCobrancas = await _cobrancaService.ConsultarCobrancas(cpf, pagina);
            if (operationCobrancas is OperationFail<List<Cobranca>> operationFail)
            {
                return Result.CreateFailure<IEnumerable<CobrancaResponse>>(operationFail.Mensagens.Mensagem,
                                                                           operationFail.Mensagens.Campos);
            }

            var cobrancas = operationCobrancas as OperationSuccess<List<Cobranca>>;
            var cobrancasReponse = cobrancas.Data
                                   .Select(x => CobrancaResponseMapper.ConverterCobrancaEmCobrancaResponse(x));
            return Result.CreateSuccess(cobrancasReponse);

        }

        public async Task<IOperation<IEnumerable<CobrancaResponse>>> ConsultarCobrancasPorMes(int mes, int pagina)
        {
            var operationCobrancas = await _cobrancaService.ConsultarCobrancas(mes, pagina);
            if (operationCobrancas is OperationFail<List<Cobranca>> operationFail)
            {
                return Result.CreateFailure<IEnumerable<CobrancaResponse>>(operationFail.Mensagens.Mensagem,
                                                                           operationFail.Mensagens.Campos);
            }

            var cobrancas = operationCobrancas as OperationSuccess<List<Cobranca>>;
            var cobrancasReponse = cobrancas.Data
                                   .Select(x => CobrancaResponseMapper.ConverterCobrancaEmCobrancaResponse(x));
            return Result.CreateSuccess(cobrancasReponse);
        }
    }
}
