using Stone.Clientes.Dominio.Entities;
using System.Threading.Tasks;

namespace Stone.Clientes.Dominio.Repository.Interfaces
{
    public interface IClienteWriterRepository
    {
        Task<Cliente> Cadastrar(Cliente cartao);
    }
}
