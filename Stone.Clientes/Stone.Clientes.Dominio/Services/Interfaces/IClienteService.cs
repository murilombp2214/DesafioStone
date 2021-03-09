using Stone.Clientes.Dominio.Entities;
using Stone.Clientes.Infra.CrossCutting.Utils.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stone.Clientes.Dominio.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IOperation<Cliente>> Cadastrar(Cliente cliente);
        Task<IOperation<Cliente>> ConsultarPorCpf(string cpf);
        Task<IOperation<List<Cliente>>> Consultar(int pagina);
    }
}
