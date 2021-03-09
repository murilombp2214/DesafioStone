using Stone.Clientes.Dominio.Entities;
using Stone.Clientes.Infra.CrossCutting.Utils.Interfaces;

namespace Stone.Clientes.Dominio.Validations.Interfaces
{
    public interface IClienteValidation
    {
        IOperation<Cliente> Validar(Cliente cliente);
    }
}
