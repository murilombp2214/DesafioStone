using Stone.Cobrancas.Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stone.Cobrancas.Dominio.Repository.Interface
{
    public interface ICobrancaWriterRepository
    {
        public Task<Cobranca> Cadastrar(Cobranca cobranca);
    }
}
