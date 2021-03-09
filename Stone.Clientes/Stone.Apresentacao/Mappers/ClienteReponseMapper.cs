using Stone.Clientes.Aplicacacao.Response.Cliente;
using Stone.Clientes.Dominio.Entities;

namespace Stone.Clientes.Aplicacacao.Mappers
{
    public static class ClienteReponseMapper
    {
        public static ClienteResponse ConverterClienteEmClienteResponse(Cliente cliente)
        {
            return new ClienteResponse
            {
                Cpf = cliente.Cpf,
                Estado = cliente.Estado,
                Id = cliente.Id,
                Nome = cliente.Nome
            };
        }
    }
}
