using Stone.Clientes.Dominio.Entities;
using Stone.Clientes.Dominio.Repository.Interfaces;
using Stone.Clientes.Dominio.Services.Interfaces;
using Stone.Clientes.Dominio.Validations.Interfaces;
using Stone.Clientes.Infra.CrossCutting.Utils;
using Stone.Clientes.Infra.CrossCutting.Utils.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stone.Clientes.Dominio.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteValidation _clienteValidation;
        private readonly IClienteWriterRepository _clienteWriteRepository;
        private readonly IClienteQueryRepository _clienteQueryRepository;
        private readonly ICpfValidation _cpfValidation;

        public ClienteService(IClienteValidation clienteValidation,
                              IClienteWriterRepository clienteWriteRepository,
                              IClienteQueryRepository clienteQueryRepository,
                              ICpfValidation cpfValidation)
        {
            _clienteValidation = clienteValidation;
            _clienteWriteRepository = clienteWriteRepository;
            _clienteQueryRepository = clienteQueryRepository;
            _cpfValidation = cpfValidation;
        }
        public async Task<IOperation<Cliente>> Cadastrar(Cliente cliente)
        {
            var operation = _clienteValidation.Validar(cliente);
            if (operation is OperationFail<Cliente>)
                return operation;
            var operationClienteExistente = await ValidarCpfExistente(cliente);
            if (operationClienteExistente is OperationFail<Cliente>)
                return operationClienteExistente;

            var clienteCadastrado = await _clienteWriteRepository.Cadastrar(cliente);
            if (clienteCadastrado == null)
                return Result.CreateFailure<Cliente>("Houve um erro ao cadastrar esse usuario.");

            return Result.CreateSuccess(clienteCadastrado);
        }

        public async Task<IOperation<List<Cliente>>> Consultar(int pagina)
        {
            if (pagina <= 0)
                return CriarFalhaConsultaGeralCliente(pagina);

            var clientes = await _clienteQueryRepository.Consultar(pagina);
            if(clientes == null)
                return Result.CreateFailure<List<Cliente>>($"Houve um erro ao realizar a consulta paginada para a pagina {pagina}.");

            return Result.CreateSuccess(clientes);

        }


        public async Task<IOperation<Cliente>> ConsultarPorCpf(string cpf)
        {
            var cpfValido = _cpfValidation.Validar(cpf);
            if (cpfValido)
            {
                var cliente = await _clienteQueryRepository.Consultar(cpf);
                if (cliente is object)
                    return Result.CreateSuccess(cliente);

                return Result.CreateFailure<Cliente>("O cpf informado é não foi encontrado.");
            }
            return Result.CreateFailure<Cliente>("O cpf informado é invalido.");
        }



        private async Task<IOperation<Cliente>> ValidarCpfExistente(Cliente cliente)
        {
            var cpfExiste = await _clienteQueryRepository.Consultar(cliente.Cpf) != null;
            if (cpfExiste)
            {
                return Result.CreateFailure<Cliente>("Houve um erro ao cadastrar esse usuario.", new DetalhesDaMensagem
                {
                    Campo = nameof(cliente.Cpf).ToLower(),
                    Mensagem = "O cpf informado já existe",
                    Valor = cliente.Cpf
                });
            }

            return Result.CreateSuccess<Cliente>();
        }

        private IOperation<List<Cliente>> CriarFalhaConsultaGeralCliente(in int pagina)
        {
            return Result.CreateFailure<List<Cliente>>("Houve um erro ao iniciar a busca.",
                new DetalhesDaMensagem
                {
                    Campo = nameof(pagina),
                    Mensagem = "A pagina não pode ser menor ou igual a 0.",
                    Valor = pagina.ToString()
                });
        }
    }
}
