using Stone.Clientes.Aplicacacao.Request.Cliente;
using Stone.Clientes.Aplicacacao.Response.Cliente;
using Stone.Clientes.Infra.CrossCutting.Utils.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stone.Clientes.Aplicacacao.AppService.Interfaces
{
    public interface IClienteAppService
    {
        Task<IOperation<ClienteResponse>> Cadastrar(ClienteRequest request);
        Task<IOperation<ClienteResponse>> ConsultarPorCpf(string cpf);
        Task<IOperation<List<ClienteResponse>>> Consultar(int pagina);
    }
}
