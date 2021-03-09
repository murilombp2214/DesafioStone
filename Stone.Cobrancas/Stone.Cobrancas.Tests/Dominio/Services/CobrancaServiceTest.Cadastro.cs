using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Stone.Cobrancas.Dominio.Entities;
using Stone.Cobrancas.Dominio.Repository.Interface;
using Stone.Cobrancas.Dominio.Services;
using Stone.Cobrancas.Dominio.Validations.Interfaces;
using Stone.Cobrancas.Infra.CrossCutting.Utils;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Stone.Cobrancas.Tests.Dominio.Services
{
    public partial class CobrancaServiceTest
    {
        [Fact]
        public async Task Se_CobrancaCadastradaInvalida_Entao_RetorneErro()
        {
            var mockCobrancaValidation = new Mock<ICobrancaValidation>();
            mockCobrancaValidation.Setup(x => x.Validar(It.IsAny<Cobranca>()))
                                  .Returns(Result.CreateFailure<Cobranca>(""));
            var cobrancaService = new CobrancaService(mockCobrancaValidation.Object,null,null,null,null);
            var operation = await cobrancaService.Cadastrar(null);
            var operationFail = operation as OperationFail<Cobranca>;
            Assert.NotNull(operationFail);
        }


        [Fact]
        public async Task Se_HouverFalhaAoCadastrarUmaCobranca_Entao_RetorneErro()
        {
            var mockCobrancaValidation = new Mock<ICobrancaValidation>();
            mockCobrancaValidation.Setup(x => x.Validar(It.IsAny<Cobranca>()))
                                  .Returns(Result.CreateSuccess<Cobranca>(null));

            var mockCobrancaRepositoryWriter = new Mock<ICobrancaWriterRepository>();
            mockCobrancaRepositoryWriter.Setup(x => x.Cadastrar(It.IsAny<Cobranca>()))
                                  .Returns(Task.FromResult(default(Cobranca)));


            var cobrancaService = new CobrancaService(mockCobrancaValidation.Object, null, 
                                                      mockCobrancaRepositoryWriter.Object, null, null);

            var operation = await cobrancaService.Cadastrar(null);
            var operationFail = operation as OperationFail<Cobranca>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao registrar a cobrança.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 0);
        }

        [Fact]
        public async Task Se_DadosCobrancaCertos_Entao_RetorneSucesso()
        {
            var mockCobrancaValidation = new Mock<ICobrancaValidation>();
            mockCobrancaValidation.Setup(x => x.Validar(It.IsAny<Cobranca>()))
                                  .Returns(Result.CreateSuccess<Cobranca>(null));

            var mockCobrancaRepositoryWriter = new Mock<ICobrancaWriterRepository>();
            mockCobrancaRepositoryWriter.Setup(x => x.Cadastrar(It.IsAny<Cobranca>()))
                                  .Returns(Task.FromResult(new Cobranca(DateTime.Now,"cpf",decimal.Zero)));

            var mockLog = new Mock<ILogger<CobrancaService>>();

            var cobrancaService = new CobrancaService(mockCobrancaValidation.Object, null,
                                                      mockCobrancaRepositoryWriter.Object, null, 
                                                      mockLog.Object);

            var operation = await cobrancaService.Cadastrar(new Cobranca(DateTime.Now, "cpf", decimal.Zero));
            var operationSucess = operation as OperationSuccess<Cobranca>;
            Assert.NotNull(operationSucess);
            Assert.NotNull(operationSucess.Data);
        }
    }
}
