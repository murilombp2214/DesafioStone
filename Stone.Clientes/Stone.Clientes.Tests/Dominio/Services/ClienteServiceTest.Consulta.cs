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
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Se_PaginaDeConsultaPaginadaMenorOuIgualAhZero_Entao_RetorneErro(int pagina)
        {
            var clienteService = new ClienteService(null, null, null, null);
            var operation = await clienteService.Consultar(pagina);
            var operationFail = operation as OperationFail<List<Cliente>>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao iniciar a busca.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 1);
            Assert.Equal(nameof(pagina), operationFail.Mensagens.Campos[0].Campo);
            Assert.Equal("A pagina não pode ser menor ou igual a 0.", operationFail.Mensagens.Campos[0].Mensagem);
            Assert.Equal(pagina.ToString(), operationFail.Mensagens.Campos[0].Valor);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(12)]
        public async Task Se_ConsultaPaginadaRetornarUmaListaNula_Entao_RetoneErro(int pagina)
        {
            var mockClienteQueyRepository = new Mock<IClienteQueryRepository>();
            mockClienteQueyRepository.Setup(x => x.Consultar(pagina))
                          .Returns(Task.FromResult(default(List<Cliente>)));

            var clienteService = new ClienteService(null, null, mockClienteQueyRepository.Object, null);

            var operation = await clienteService.Consultar(pagina);
            var operationFail = operation as OperationFail<List<Cliente>>;
            Assert.NotNull(operationFail);
            Assert.Equal($"Houve um erro ao realizar a consulta paginada para a pagina {pagina}.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 0);
        }

        [Fact]
        public async Task Se_ConsultaPaginadaRetornarUmaListaNaoNula_Entao_RetoneSuceso()
        {
            var mockClienteQueyRepository = new Mock<IClienteQueryRepository>();
            mockClienteQueyRepository.Setup(x => x.Consultar(1))
                          .Returns(Task.FromResult(new List<Cliente>()));

            var clienteService = new ClienteService(null, null, mockClienteQueyRepository.Object, null);

            var operation = await clienteService.Consultar(1);
            var operationSucess = operation as OperationSuccess<List<Cliente>>;
            Assert.NotNull(operationSucess);
            Assert.NotNull(operationSucess.Data);
        }
        [Fact]
        public async Task Se_CpfPassadoParaConsultaPorCpfForInvalido_Entao_RetoneErro()
        {
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>())).Returns(false);
            var clienteService = new ClienteService(null, null, null, mockCpfValidation.Object);

            var operation = await clienteService.ConsultarPorCpf(It.IsAny<string>());
            var operationFail = operation as OperationFail<Cliente>;
            Assert.NotNull(operationFail);
            Assert.Equal($"O cpf informado é invalido.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 0);
        }

        [Fact]
        public async Task Se_CpfPassadoParaConsultaPorCpfNaoForEncontrado_Entao_RetoneErro()
        {
            const string cpf = "12345678911";
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(cpf)).Returns(true);

            var mockClienteQueyRepository = new Mock<IClienteQueryRepository>();
            mockClienteQueyRepository.Setup(x => x.Consultar(cpf))
                          .Returns(Task.FromResult(default(Cliente)));

            var clienteService = new ClienteService(null, null, mockClienteQueyRepository.Object, mockCpfValidation.Object);

            var operation = await clienteService.ConsultarPorCpf(cpf);
            var operationFail = operation as OperationFail<Cliente>;
            Assert.NotNull(operationFail);
            Assert.Equal($"O cpf informado é não foi encontrado.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 0);
        }

        [Fact]
        public async Task Se_CpfPassadoParaConsultaPorCpfForEncontrado_Entao_RetoneSucesso()
        {
            const string cpf = "12345678911";
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(cpf)).Returns(true);

            var mockClienteQueyRepository = new Mock<IClienteQueryRepository>();
            mockClienteQueyRepository.Setup(x => x.Consultar(cpf))
                          .Returns(Task.FromResult(new Cliente("","","")));

            var clienteService = new ClienteService(null, null, mockClienteQueyRepository.Object, mockCpfValidation.Object);

            var operation = await clienteService.ConsultarPorCpf(cpf);
            var operationSucess = operation as OperationSuccess<Cliente>;
            Assert.NotNull(operationSucess);
            Assert.NotNull(operationSucess.Data);
        }
    }
}
