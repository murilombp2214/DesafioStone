using Moq;
using Stone.Cobrancas.Dominio.Entities;
using Stone.Cobrancas.Dominio.Validations;
using Stone.Cobrancas.Dominio.Validations.Interfaces;
using Stone.Cobrancas.Infra.CrossCutting.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Stone.Cobrancas.Tests.Dominio.Validations
{
    public class ConsultarCobrancasValidationTest
    {
        [Fact]
        public void Se_CpfInvalido_Entao_RetorneErro()
        {
            const string cpf = "1234654";
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(cpf))
                             .Returns(false);

            var consultarCobrancaValidation = new ConsultarCobrancasValidation(mockCpfValidation.Object);
            var operation = consultarCobrancaValidation.Validar(cpf,1);
            var operationFail = operation as OperationFail<List<Cobranca>>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados da consulta.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 1);
            Assert.True(operationFail.Mensagens.Campos[0].Campo == "cpf");
            Assert.True(operationFail.Mensagens.Campos[0].Mensagem == $"O CPF informado é invalido.");
            Assert.True(operationFail.Mensagens.Campos[0].Valor == cpf);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Se_CpfVazio_Entao_RetorneErro(string cpf)
        {
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(cpf))
                             .Returns(false);

            var consultarCobrancaValidation = new ConsultarCobrancasValidation(mockCpfValidation.Object);
            var operation = consultarCobrancaValidation.Validar(cpf, 1);
            var operationFail = operation as OperationFail<List<Cobranca>>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados da consulta.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 1);
            Assert.True(operationFail.Mensagens.Campos[0].Campo == "cpf");
            Assert.True(operationFail.Mensagens.Campos[0].Mensagem == $"CPF não pode ser vazio.");
            Assert.True(operationFail.Mensagens.Campos[0].Valor == cpf);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Se_PaginaMenorOuIgualZeroEmValidacaoCpf_Entao_RetorneErro(int pagina)
        {
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>()))
                             .Returns(true);

            var consultarCobrancaValidation = new ConsultarCobrancasValidation(mockCpfValidation.Object);
            var operation = consultarCobrancaValidation.Validar("1456", pagina);
            var operationFail = operation as OperationFail<List<Cobranca>>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados da consulta.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 1);
            Assert.True(operationFail.Mensagens.Campos[0].Campo == "pagina");
            Assert.True(operationFail.Mensagens.Campos[0].Mensagem == $"A pagina deve ser maior ou igual a 1.");
            Assert.True(operationFail.Mensagens.Campos[0].Valor == pagina.ToString());
        }


        [Fact]
        public void Se_Cpf_E_PaginaValidos_Entao_RetorneSucesso()
        {
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>()))
                             .Returns(true);

            var consultarCobrancaValidation = new ConsultarCobrancasValidation(mockCpfValidation.Object);
            var operation = consultarCobrancaValidation.Validar("1456", 1);
            var operationSucess = operation as OperationSuccess<List<Cobranca>>;
            Assert.NotNull(operationSucess);
            Assert.Null(operationSucess.Data);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Se_PaginaMenorOuIgualZeroEmValidacaoMes_Entao_RetorneErro(int pagina)
        {
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>()))
                             .Returns(true);

            var consultarCobrancaValidation = new ConsultarCobrancasValidation(mockCpfValidation.Object);
            var operation = consultarCobrancaValidation.Validar("1456", pagina);
            var operationFail = operation as OperationFail<List<Cobranca>>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados da consulta.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 1);
            Assert.True(operationFail.Mensagens.Campos[0].Campo == "pagina");
            Assert.True(operationFail.Mensagens.Campos[0].Mensagem == $"A pagina deve ser maior ou igual a 1.");
            Assert.True(operationFail.Mensagens.Campos[0].Valor == pagina.ToString());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        public void Se_Mes_E_PaginaValidos_Entao_RetorneSucesso(int mes)
        {
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>()))
                             .Returns(true);

            var consultarCobrancaValidation = new ConsultarCobrancasValidation(mockCpfValidation.Object);
            var operation = consultarCobrancaValidation.Validar(mes, 1);
            var operationSucess = operation as OperationSuccess<List<Cobranca>>;
            Assert.NotNull(operationSucess);
            Assert.Null(operationSucess.Data);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(13)]
        public void Se_MesInvalido_Entao_RetorneErro(int mes)
        {
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>()))
                             .Returns(true);

            var consultarCobrancaValidation = new ConsultarCobrancasValidation(mockCpfValidation.Object);
            var operation = consultarCobrancaValidation.Validar(mes, 1);
            var operationFail = operation as OperationFail<List<Cobranca>>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados da consulta.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 1);
            Assert.True(operationFail.Mensagens.Campos[0].Campo == "mes");
            Assert.True(operationFail.Mensagens.Campos[0].Mensagem == "O mês informado é invalido.");
            Assert.True(operationFail.Mensagens.Campos[0].Valor == mes.ToString());
        }

    }
}
