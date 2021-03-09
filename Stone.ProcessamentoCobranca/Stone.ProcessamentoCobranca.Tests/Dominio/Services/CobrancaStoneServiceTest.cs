using Microsoft.Extensions.Logging;
using Moq;
using Stone.ProcessamentoCobranca.Dominio.Entities;
using Stone.ProcessamentoCobranca.Dominio.Repository.Interfaces;
using Stone.ProcessamentoCobranca.Dominio.Services;
using Stone.ProcessamentoCobranca.Dominio.Services.Interfaces;
using Stone.ProcessamentoCobranca.Infra.CrossCutting.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Stone.ProcessamentoCobranca.Tests.Dominio.Services
{
    public class CobrancaStoneServiceTest
    {
        [Fact]
        public async Task Se_CobrancaNula_Entao_RetonarErro()
        {
            var cobrancaStoneService = new CobrancaStoneService(null,null);
            var operation = await cobrancaStoneService.RegistrarCobranca(null);
            var operationFail = operation as OperationFail<CobrancaStone>;
            Assert.NotNull(operationFail);
            Assert.Equal("Não é possivel registrar uma cobrança nula.",operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 0);
        }

        [Fact]
        public async Task Se_RepositorioNaoRegistrarCobranca_Entao_RetonarErro()
        {
            const string cpf = "11111111111";
            var cobranca = new CobrancaStone(DateTime.Now, cpf, 50);

            var mockCobrancaStoneWriterRepository = new Mock<ICobrancaStoneWriterRepository>();
            mockCobrancaStoneWriterRepository.Setup(x => x.RegistrarCobranca(cobranca))
                                             .Returns(Task.FromResult(default(CobrancaStone)));


            var cobrancaStoneService = new CobrancaStoneService(mockCobrancaStoneWriterRepository.Object,
                                                                new Mock<ILogger<ICobrancaStoneService>>().Object);
            var operation = await cobrancaStoneService.RegistrarCobranca(cobranca);
            var operationFail = operation as OperationFail<CobrancaStone>;
            Assert.NotNull(operationFail);

            string msgEsperada = $"Houve uma falha ao registrar a cobrança para o CPF {cpf.Substring(0, 3)}...{cpf.Substring(7, 3)}.";
            Assert.Equal(msgEsperada, operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 0);
        }


        [Fact]
        public async Task Se_DadosCobrancaCerto_Entao_RetonarSucesso()
        {
            const string cpf = "11111111111";
            var cobranca = new CobrancaStone(DateTime.Now, cpf, 50);

            var mockCobrancaStoneWriterRepository = new Mock<ICobrancaStoneWriterRepository>();
            mockCobrancaStoneWriterRepository.Setup(x => x.RegistrarCobranca(cobranca))
                                             .Returns(Task.FromResult(cobranca));


            var cobrancaStoneService = new CobrancaStoneService(mockCobrancaStoneWriterRepository.Object,
                                                                new Mock<ILogger<ICobrancaStoneService>>().Object);
            var operation = await cobrancaStoneService.RegistrarCobranca(cobranca);
            var operationSucess = operation as OperationSuccess<CobrancaStone>;
            Assert.NotNull(operationSucess);
            Assert.NotNull(operationSucess.Data);
        }
    }
}
