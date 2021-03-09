using Stone.Cobrancas.Aplicacacao.Request;
using Stone.Cobrancas.Aplicacacao.Response;
using Stone.Cobrancas.Infra.CrossCutting.Utils.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stone.Cobrancas.Aplicacacao.AppService.Interfaces
{
    public interface ICobrancaAppService
    {
        public Task<IOperation<CobrancaResponse>> Cadastrar(CobrancaRequest request);
        public Task<IOperation<IEnumerable<CobrancaResponse>>> ConsultarCobrancasPorCpf(string cpf, int pagina);
        public Task<IOperation<IEnumerable<CobrancaResponse>>> ConsultarCobrancasPorMes(int mes, int pagina);
    }
}
