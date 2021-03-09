using Stone.Clientes.Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stone.Clientes.Dominio.Repository.Interfaces
{
    public interface IClienteQueryRepository
    {
        Task<Cliente> Consultar(string cpf);
        Task<List<Cliente>> Consultar(int pagina);
    }
}
