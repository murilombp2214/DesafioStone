using Stone.Clientes.Aplicacacao.AppService.Interfaces;
using Stone.Clientes.Aplicacacao.Mappers;
using Stone.Clientes.Aplicacacao.Request.Cliente;
using Stone.Clientes.Aplicacacao.Response.Cliente;
using Stone.Clientes.Dominio.Entities;
using Stone.Clientes.Dominio.Services.Interfaces;
using Stone.Clientes.Infra.CrossCutting.Utils;
using Stone.Clientes.Infra.CrossCutting.Utils.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stone.Clientes.Aplicacacao.AppService
{
    public class ClienteAppService : IClienteAppService
    {
        private readonly IClienteService _clienteService;
        private readonly ICpfMask _cpfMask;
        public ClienteAppService(IClienteService clienteService, ICpfMask cpfMask)
        {
            _clienteService = clienteService;
            _cpfMask = cpfMask;
        }
        public async Task<IOperation<ClienteResponse>> Cadastrar(ClienteRequest request)
        {
            if (request is null)
                return Result.CreateFailure<ClienteResponse>("Requisição vazia");

            request.Cpf = _cpfMask.RemoveMaskCpf(request.Cpf);
            var cliente = ClienteRequestMapper.ConverterClienteRequestEmCliente(request);
            
            var operation = await _clienteService.Cadastrar(cliente);

            if (operation is OperationFail<Cliente> operationFail)
                return Result.CreateFailure<ClienteResponse>(operationFail.Mensagens.Mensagem, operationFail.Mensagens.Campos);


            var operationSucess = operation as OperationSuccess<Cliente>;
            var clienteResponse = ClienteReponseMapper.ConverterClienteEmClienteResponse(operationSucess.Data);
            return Result.CreateSuccess(clienteResponse);

        }

        public async Task<IOperation<List<ClienteResponse>>> Consultar(int pagina)
        {
            var opereration = await _clienteService.Consultar(pagina);
            if (opereration is OperationFail<List<Cliente>> operationFail)
                return Result.CreateFailure<List<ClienteResponse>>(operationFail.Mensagens.Mensagem, operationFail.Mensagens.Campos);

            var clientesOperationSuccess = opereration as OperationSuccess<List<Cliente>>;
            var clientes =  clientesOperationSuccess.Data.Select(x => ClienteReponseMapper.ConverterClienteEmClienteResponse(x));
            return Result.CreateSuccess(clientes.ToList());

        }

        public async Task<IOperation<ClienteResponse>> ConsultarPorCpf(string cpf)
        {
            var operation = await _clienteService.ConsultarPorCpf(_cpfMask.RemoveMaskCpf(cpf));
            if(operation is OperationFail<Cliente> operationFail)
                return Result.CreateFailure<ClienteResponse>(operationFail.Mensagens.Mensagem, operationFail.Mensagens.Campos);

            var operationSucess = operation as OperationSuccess<Cliente>;
            var clienteResponse = ClienteReponseMapper.ConverterClienteEmClienteResponse(operationSucess.Data);
            return Result.CreateSuccess(clienteResponse);

        }
    }
}
