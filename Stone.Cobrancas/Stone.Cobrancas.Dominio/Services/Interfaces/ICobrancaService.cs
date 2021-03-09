using Stone.Cobrancas.Dominio.Entities;
using Stone.Cobrancas.Infra.CrossCutting.Utils.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stone.Cobrancas.Dominio.Services.Interfaces
{
    public interface ICobrancaService
    {
        Task<IOperation<Cobranca>> Cadastrar(Cobranca cobranca);
        public Task<IOperation<List<Cobranca>>> ConsultarCobrancas(string cpf, int pagina);
        public Task<IOperation<List<Cobranca>>> ConsultarCobrancas(int mes, int pagina);
    }
}
