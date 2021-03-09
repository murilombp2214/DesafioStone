using Moq;
using Stone.ProcessamentoCobranca.Dominio.Entities;
using Stone.ProcessamentoCobranca.Dominio.Repository.Interfaces;
using Stone.ProcessamentoCobranca.Dominio.Services;
using Stone.ProcessamentoCobranca.Infra.CrossCutting.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Stone.ProcessamentoCobranca.Tests.Dominio.Services
{
    public class ClienteStoneServiceTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Se_PaginaMenorOuIgualZero_Entao_RetornarErro(int pagina)
        {
            var clienteStoneService = new ClienteStoneService(null);
            var operation = await clienteStoneService.ObterClientes(pagina);
            var operationFail = operation as OperationFail<List<ClienteStone>>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao iniciar a busca.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 1);
            Assert.True(operationFail.Mensagens.Campos[0].Campo == "pagina");
            Assert.True(operationFail.Mensagens.Campos[0].Mensagem == "A pagina não pode ser menor ou igual a 0.");
            Assert.True(operationFail.Mensagens.Campos[0].Valor == pagina.ToString());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Se_ConsultaClienteForNula_Entao_RetornarErro(int pagina)
        {
            var mockClienteStoneQueryRepository = new Mock<IClienteStoneQueryRepository>();
            mockClienteStoneQueryRepository.Setup(x => x.ConsultarClientes(pagina))
                                            .Returns(Task.FromResult(default(List<ClienteStone>)));

            var clienteStoneService = new ClienteStoneService(mockClienteStoneQueryRepository.Object);
            var operation = await clienteStoneService.ObterClientes(pagina);
            var operationFail = operation as OperationFail<List<ClienteStone>>;
            Assert.NotNull(operationFail);
            Assert.Equal($"Houve um erro ao consultar a pagina {pagina}.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 0);
        }

        [Fact]
        public async Task Se_ConsultaClienteRetornarListaNaoNula_Entao_RetornarSucesso()
        {
            var mockClienteStoneQueryRepository = new Mock<IClienteStoneQueryRepository>();
            mockClienteStoneQueryRepository.Setup(x => x.ConsultarClientes(1))
                                            .Returns(Task.FromResult(new List<ClienteStone>()));

            var clienteStoneService = new ClienteStoneService(mockClienteStoneQueryRepository.Object);
            var operation = await clienteStoneService.ObterClientes(1);
            var operationSucess = operation as OperationSuccess<List<ClienteStone>>;
            Assert.NotNull(operationSucess);
            Assert.NotNull(operationSucess.Data);
        }
    }
}
