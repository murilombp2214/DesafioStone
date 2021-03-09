using Stone.Clientes.Aplicacacao.Request.Cliente;
using Stone.Clientes.Dominio.Entities;

namespace Stone.Clientes.Aplicacacao.Mappers
{
    public static class ClienteRequestMapper
    {
        public static Cliente ConverterClienteRequestEmCliente(ClienteRequest request)
        {
            return new Cliente(request?.Nome, request?.Estado, request?.Cpf);
        }
    }
}
