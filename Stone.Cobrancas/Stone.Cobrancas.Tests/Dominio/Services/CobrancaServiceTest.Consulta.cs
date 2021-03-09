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
        public async Task Se_CpfOuPaginaInvalido_Entao_RetornarErro()
        {
            var mockCobrancaConsultaValidation = new Mock<IConsultarCobrancasValidation>();
            mockCobrancaConsultaValidation.Setup(x => x.Validar(It.IsAny<string>(), It.IsAny<int>()))
                                          .Returns(Result.CreateFailure<List<Cobranca>>(""));

            var cobrancaService = new CobrancaService(null, mockCobrancaConsultaValidation.Object, null,
                                                      null, null);
            var operation = await cobrancaService.ConsultarCobrancas(It.IsAny<string>(), It.IsAny<int>());
            var operationFail = operation as OperationFail<List<Cobranca>>;
            Assert.NotNull(operationFail);
        }

        [Fact]
        public async Task Se_RepositorioRetornarNuloEmConsultaPorCpf_Entao_RetornarErro()
        {
            var mockCobrancaConsultaValidation = new Mock<IConsultarCobrancasValidation>();
            mockCobrancaConsultaValidation.Setup(x => x.Validar(It.IsAny<string>(), It.IsAny<int>()))
                                          .Returns(Result.CreateSuccess<List<Cobranca>>(null));

            var mockRepository = new Mock<ICobrancaQueryRepository>();
            mockRepository.Setup(x => x.ConsultarCobrancas(It.IsAny<string>(), It.IsAny<int>()))
                          .Returns(Task.FromResult(default(List<Cobranca>)));


            var cobrancaService = new CobrancaService(null, mockCobrancaConsultaValidation.Object, null,
                                                      mockRepository.Object, null);
            var operation = await cobrancaService.ConsultarCobrancas(It.IsAny<string>(), It.IsAny<int>());
            var operationFail = operation as OperationFail<List<Cobranca>>;
            Assert.NotNull(operationFail);
            Assert.True(operationFail.Mensagens.Mensagem == "Houve um erro ao consultar as cobrança.");
            Assert.True(operationFail.Mensagens.Campos.Count == 0);
        }

        [Fact]
        public async Task Se_RepositorioRetornarListNaoNulaEmConsultaPorCpf_Entao_RetornarErro()
        {
            var mockCobrancaConsultaValidation = new Mock<IConsultarCobrancasValidation>();
            mockCobrancaConsultaValidation.Setup(x => x.Validar(It.IsAny<string>(), It.IsAny<int>()))
                                          .Returns(Result.CreateSuccess<List<Cobranca>>(null));

            var mockRepository = new Mock<ICobrancaQueryRepository>();
            mockRepository.Setup(x => x.ConsultarCobrancas(It.IsAny<string>(), It.IsAny<int>()))
                          .Returns(Task.FromResult(new List<Cobranca>()));

            var cobrancaService = new CobrancaService(null, mockCobrancaConsultaValidation.Object, null,
                                                      mockRepository.Object, null);
            var operation = await cobrancaService.ConsultarCobrancas(It.IsAny<string>(), It.IsAny<int>());
            var operationSucess = operation as OperationSuccess<List<Cobranca>>;
            Assert.NotNull(operationSucess);
            Assert.NotNull(operationSucess.Data);
        }

        [Fact]
        public async Task Se_MesOuPaginaInvalido_Entao_RetornarErro()
        {
            var mockCobrancaConsultaValidation = new Mock<IConsultarCobrancasValidation>();
            mockCobrancaConsultaValidation.Setup(x => x.Validar(It.IsAny<int>(), It.IsAny<int>()))
                                          .Returns(Result.CreateFailure<List<Cobranca>>(""));

            var cobrancaService = new CobrancaService(null, mockCobrancaConsultaValidation.Object, null,
                                                      null, null);
            var operation = await cobrancaService.ConsultarCobrancas(It.IsAny<int>(), It.IsAny<int>());
            var operationFail = operation as OperationFail<List<Cobranca>>;
            Assert.NotNull(operationFail);
        }

        [Fact]
        public async Task Se_RepositorioRetornarNuloEmConsultaPorMes_Entao_RetornarErro()
        {
            var mockCobrancaConsultaValidation = new Mock<IConsultarCobrancasValidation>();
            mockCobrancaConsultaValidation.Setup(x => x.Validar(It.IsAny<string>(), It.IsAny<int>()))
                                          .Returns(Result.CreateSuccess<List<Cobranca>>(null));

            var mockRepository = new Mock<ICobrancaQueryRepository>();
            mockRepository.Setup(x => x.ConsultarCobrancas(It.IsAny<int>(), It.IsAny<int>()))
                          .Returns(Task.FromResult(default(List<Cobranca>)));


            var cobrancaService = new CobrancaService(null, mockCobrancaConsultaValidation.Object, null,
                                                      mockRepository.Object, null);
            var operation = await cobrancaService.ConsultarCobrancas(It.IsAny<int>(), It.IsAny<int>());
            var operationFail = operation as OperationFail<List<Cobranca>>;
            Assert.NotNull(operationFail);
            Assert.True(operationFail.Mensagens.Mensagem == "Houve um erro ao consultar as cobrança.");
            Assert.True(operationFail.Mensagens.Campos.Count == 0);
        }

        [Fact]
        public async Task Se_RepositorioRetornarListNaoNulaEmConsultaPorMes_Entao_RetornarErro()
        {
            var mockCobrancaConsultaValidation = new Mock<IConsultarCobrancasValidation>();
            mockCobrancaConsultaValidation.Setup(x => x.Validar(It.IsAny<int>(), It.IsAny<int>()))
                                          .Returns(Result.CreateSuccess<List<Cobranca>>(null));

            var mockRepository = new Mock<ICobrancaQueryRepository>();
            mockRepository.Setup(x => x.ConsultarCobrancas(It.IsAny<int>(), It.IsAny<int>()))
                          .Returns(Task.FromResult(new List<Cobranca>()));

            var cobrancaService = new CobrancaService(null, mockCobrancaConsultaValidation.Object, null,
                                                      mockRepository.Object, null);
            var operation = await cobrancaService.ConsultarCobrancas(It.IsAny<int>(), It.IsAny<int>());
            var operationSucess = operation as OperationSuccess<List<Cobranca>>;
            Assert.NotNull(operationSucess);
            Assert.NotNull(operationSucess.Data);
        }
    }
}
