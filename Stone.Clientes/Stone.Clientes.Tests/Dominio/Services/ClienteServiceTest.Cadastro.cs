using Moq;
using Stone.Clientes.Dominio.Entities;
using Stone.Clientes.Dominio.Repository.Interfaces;
using Stone.Clientes.Dominio.Services;
using Stone.Clientes.Dominio.Validations.Interfaces;
using Stone.Clientes.Infra.CrossCutting.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Stone.Clientes.Tests.Dominio.Services
{
    public partial class ClienteServiceTest
    {
        [Fact]
        public async Task Se_ClienteNulo_Entao_RetorneErro()
        {

            var mockValidation = new Mock<IClienteValidation>();
            mockValidation.Setup(x => x.Validar(default))
                          .Returns(Result.CreateFailure<Cliente>("A entidade não pode ser vazia."));

            var clienteService = new ClienteService(mockValidation.Object,null,null,null);
            var operation =  await clienteService.Cadastrar(null);
            var operationFail = operation as OperationFail<Cliente>;
            Assert.NotNull(operationFail);
            Assert.Equal("A entidade não pode ser vazia.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 0);
        }

        [Fact]
        public async Task Se_CpfClienteExistir_Entao_RetorneErro()
        {
            const string cpf = "1111111111";
            var cliente = new Cliente(Guid.NewGuid(), "nome", "estado", cpf);

            var mockValidation = new Mock<IClienteValidation>();
            mockValidation.Setup(x => x.Validar(default))
                          .Returns(Result.CreateSuccess(default(Cliente)));

            var mockClienteQueyRepository = new Mock<IClienteQueryRepository>();
            mockClienteQueyRepository.Setup(x => x.Consultar(cpf))
                          .Returns(Task.FromResult(cliente));

            var clienteService = new ClienteService(mockValidation.Object, null, 
                                                    mockClienteQueyRepository.Object,null);

            var operation =  await clienteService.Cadastrar(cliente);
            var operationFail = operation as OperationFail<Cliente>;
            Assert.Equal("Houve um erro ao cadastrar esse usuario.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 1);
            Assert.Equal("cpf",operationFail.Mensagens.Campos[0].Campo);
            Assert.Equal("O cpf informado já existe", operationFail.Mensagens.Campos[0].Mensagem);
            Assert.Equal(cpf, operationFail.Mensagens.Campos[0].Valor);

        }

        [Fact]
        public async Task Se_RespositorioNaoCadastrarCliente_Entao_RetorneErro()
        {
            const string cpf = "1111111111";
            var cliente = new Cliente(Guid.NewGuid(), "nome", "estado", cpf);

            var mockValidation = new Mock<IClienteValidation>();
            mockValidation.Setup(x => x.Validar(default))
                          .Returns(Result.CreateSuccess(default(Cliente)));

            var mockClienteQueyRepository = new Mock<IClienteQueryRepository>();
            mockClienteQueyRepository.Setup(x => x.Consultar(cpf))
                          .Returns(Task.FromResult(default(Cliente)));

            var mockClienteWriterRepository = new Mock<IClienteWriterRepository>();
            mockClienteWriterRepository.Setup(x => x.Cadastrar(cliente))
                          .Returns(Task.FromResult(default(Cliente)));

            var clienteService = new ClienteService(mockValidation.Object, mockClienteWriterRepository.Object,
                                                    mockClienteQueyRepository.Object, null);

            var operation = await clienteService.Cadastrar(cliente);
            var operationFail = operation as OperationFail<Cliente>;
            Assert.Equal("Houve um erro ao cadastrar esse usuario.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 0);

        }


        [Fact]
        public async Task Se_ValidacoesCorretas_Entao_CadastrarCliente()
        {
            const string cpf = "1111111111";
            var cliente = new Cliente(Guid.NewGuid(), "nome", "estado", cpf);

            var mockValidation = new Mock<IClienteValidation>();
            mockValidation.Setup(x => x.Validar(default))
                          .Returns(Result.CreateSuccess(default(Cliente)));

            var mockClienteQueyRepository = new Mock<IClienteQueryRepository>();
            mockClienteQueyRepository.Setup(x => x.Consultar(cpf))
                          .Returns(Task.FromResult(default(Cliente)));

            var mockClienteWriterRepository = new Mock<IClienteWriterRepository>();
            mockClienteWriterRepository.Setup(x => x.Cadastrar(cliente))
                          .Returns(Task.FromResult(cliente));

            var clienteService = new ClienteService(mockValidation.Object, mockClienteWriterRepository.Object,
                                                    mockClienteQueyRepository.Object, null);

            var operation = await clienteService.Cadastrar(cliente);
            var operationSucess = operation as OperationSuccess<Cliente>;
            Assert.NotNull(operationSucess);
            Assert.NotNull(operationSucess.Data);
            Assert.Equal(cliente.Cpf, operationSucess.Data.Cpf);
            Assert.Equal(cliente.Estado, operationSucess.Data.Estado);
            Assert.Equal(cliente.Nome, operationSucess.Data.Nome);
            Assert.Equal(cliente.Id, operationSucess.Data.Id);

        }
    }
}
