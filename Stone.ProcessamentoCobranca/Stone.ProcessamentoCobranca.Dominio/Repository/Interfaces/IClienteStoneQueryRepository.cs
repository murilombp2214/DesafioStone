using Stone.ProcessamentoCobranca.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stone.ProcessamentoCobranca.Dominio.Repository.Interfaces
{
    public interface IClienteStoneQueryRepository
    {
        Task<List<ClienteStone>> ConsultarClientes(int pagina);

    }
}
