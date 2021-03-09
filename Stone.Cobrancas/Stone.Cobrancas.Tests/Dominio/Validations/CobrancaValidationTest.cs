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
    public class CobrancaValidationTest
    {
        [Fact]
        public void Se_CobrancaNula_Entao_RetorneErro()
        {
            var cobrancaValidation = new CobrancaValidation(null);
            var operation = cobrancaValidation.Validar(null);
            var operationFail = operation as OperationFail<Cobranca>;
            Assert.NotNull(operationFail);
            Assert.Equal("A entidade não pode ser vazia.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 0);
        }

        [Fact]
        public void Se_DataVencimentoNula_Entao_RetorneErro()
        {
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>()))
                             .Returns(true);
            var cobrancaValidation = new CobrancaValidation(mockCpfValidation.Object);
            var operation = cobrancaValidation.Validar(new Cobranca(default,"cpf",1));
            var operationFail = operation as OperationFail<Cobranca>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados da cobrança.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 1);
            Assert.True(operationFail.Mensagens.Campos[0].Campo == "data-vencimento");
            Assert.True(operationFail.Mensagens.Campos[0].Mensagem == "A data de vencimento é invalida.");
            Assert.True(operationFail.Mensagens.Campos[0].Valor == default(DateTime).ToString());
        }

        [Fact]
        public void Se_DataVencimentoAntesDoDiaAtual_Entao_RetorneErro()
        {
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>()))
                             .Returns(true);
            var cobrancaValidation = new CobrancaValidation(mockCpfValidation.Object);
            var operation = cobrancaValidation.Validar(new Cobranca(DateTime.Now.AddDays(-1), "cpf", 1));
            var operationFail = operation as OperationFail<Cobranca>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados da cobrança.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 1);
            Assert.True(operationFail.Mensagens.Campos[0].Campo == "data-vencimento");
            Assert.True(operationFail.Mensagens.Campos[0].Mensagem == "A data de vencimento não pode ser antes do dia atual.");
            Assert.True(operationFail.Mensagens.Campos[0].Valor == DateTime.Now.AddDays(-1).ToString());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Se_ValorCobrancaMenorOuIgualZero_Entao_RetorneErro(int valorCobranca)
        {
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>()))
                             .Returns(true);
            var cobrancaValidation = new CobrancaValidation(mockCpfValidation.Object);
            var operation = cobrancaValidation.Validar(new Cobranca(DateTime.Now, "cpf", valorCobranca));
            var operationFail = operation as OperationFail<Cobranca>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados da cobrança.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 1);
            Assert.True(operationFail.Mensagens.Campos[0].Campo == "valor-cobranca");
            Assert.True(operationFail.Mensagens.Campos[0].Mensagem == "O valor da cobrança deve ser maior que zero.");
            Assert.True(operationFail.Mensagens.Campos[0].Valor == valorCobranca.ToString());
        }

        [Fact]
        public void Se_ValorCobrancaMaiorQueMaxDecimal_Entao_RetorneErro()
        {
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>()))
                             .Returns(true);
            var cobrancaValidation = new CobrancaValidation(mockCpfValidation.Object);
            var operation = cobrancaValidation.Validar(new Cobranca(DateTime.Now, "cpf", decimal.MaxValue));
            var operationFail = operation as OperationFail<Cobranca>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados da cobrança.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 1);
            Assert.True(operationFail.Mensagens.Campos[0].Campo == "valor-cobranca");
            Assert.True(operationFail.Mensagens.Campos[0].Mensagem == $"O valor da cobrança deve ser menor que {decimal.MaxValue}.");
            Assert.True(operationFail.Mensagens.Campos[0].Valor == decimal.MaxValue.ToString());
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Se_CpfVazio_Entao_RetorneErro(string cpf)
        {
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(cpf))
                             .Returns(false);
            var cobrancaValidation = new CobrancaValidation(mockCpfValidation.Object);
            var operation = cobrancaValidation.Validar(new Cobranca(DateTime.Now, cpf, 1));
            var operationFail = operation as OperationFail<Cobranca>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados da cobrança.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 1);
            Assert.True(operationFail.Mensagens.Campos[0].Campo == "cpf");
            Assert.True(operationFail.Mensagens.Campos[0].Mensagem == $"CPF não pode ser vazio.");
            Assert.True(operationFail.Mensagens.Campos[0].Valor == cpf);
        }

        [Fact]
        public void Se_CpfInvalido_Entao_RetorneErro()
        {
            const string cpf = "1234654";
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(cpf))
                             .Returns(false);
            var cobrancaValidation = new CobrancaValidation(mockCpfValidation.Object);
            var operation = cobrancaValidation.Validar(new Cobranca(DateTime.Now, cpf, 1));
            var operationFail = operation as OperationFail<Cobranca>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados da cobrança.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 1);
            Assert.True(operationFail.Mensagens.Campos[0].Campo == "cpf");
            Assert.True(operationFail.Mensagens.Campos[0].Mensagem == $"O CPF informado é invalido.");
            Assert.True(operationFail.Mensagens.Campos[0].Valor == cpf);
        }

        [Fact]
        public void Se_CobrancaValida_Entao_RetorneSucesso()
        {
            const string cpf = "1234654";
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(cpf))
                             .Returns(true);
            var cobrancaValidation = new CobrancaValidation(mockCpfValidation.Object);
            var operation = cobrancaValidation.Validar(new Cobranca(DateTime.Now, cpf, 1));
            var operationFail = operation as OperationSuccess<Cobranca>;
            Assert.NotNull(operationFail);
            Assert.Null(operationFail.Data);
        }
    }
}
