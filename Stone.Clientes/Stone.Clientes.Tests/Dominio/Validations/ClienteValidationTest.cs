using Moq;
using Stone.Clientes.Dominio.Entities;
using Stone.Clientes.Dominio.Validations;
using Stone.Clientes.Dominio.Validations.Interfaces;
using Stone.Clientes.Infra.CrossCutting.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Stone.Clientes.Tests.Dominio.Validations
{
    public class ClienteValidationTest
    {
        [Fact]
        public void Se_ClienteNulo_Entao_RetorneErro()
        {
            var clienteValidation = new ClienteValidation(default);
            var operation = clienteValidation.Validar(default);
            var operationFail = operation as OperationFail<Cliente>;
            Assert.NotNull(operationFail);
            Assert.Equal("A entidade não pode ser vazia.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 0);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Se_NomeDoClienteNuloOuVazio_Entao_RetorneErro(string nome)
        {

            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>())).Returns(true);

            var clienteValidation = new ClienteValidation(mockCpfValidation.Object);
            var operation = clienteValidation.Validar(new Cliente(nome, "es","cpf"));
            var operationFail = operation as OperationFail<Cliente>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados do cliente.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 1);
            Assert.True(operationFail.Mensagens.Campos[0].Campo == "nome");
            Assert.True(operationFail.Mensagens.Campos[0].Valor == nome);
            Assert.True(operationFail.Mensagens.Campos[0].Mensagem == "Nome não pode ser vazio.");
        }

        [Fact]
        public void Se_NomeDoClienteMaiorQue300_Entao_RetorneErro()
        {
            string nome = string.Empty;
            for (; nome.Length < 302;nome += "a");

            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>())).Returns(true);

            var clienteValidation = new ClienteValidation(mockCpfValidation.Object);
            var operation = clienteValidation.Validar(new Cliente(nome, "es", "cpf"));
            
            var operationFail = operation as OperationFail<Cliente>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados do cliente.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 1);
            Assert.True(operationFail.Mensagens.Campos[0].Campo == "nome");
            Assert.True(operationFail.Mensagens.Campos[0].Valor == nome);
            Assert.True(operationFail.Mensagens.Campos[0].Mensagem == "Nome não pode ser maior que 300.");
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Se_EstadoDoClienteNuloOuVazio_Entao_RetorneErro(string estado)
        {

            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>())).Returns(true);

            var clienteValidation = new ClienteValidation(mockCpfValidation.Object);
            var operation = clienteValidation.Validar(new Cliente("nome", estado, "cpf"));
            var operationFail = operation as OperationFail<Cliente>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados do cliente.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 1);
            Assert.True(operationFail.Mensagens.Campos[0].Campo == "estado");
            Assert.True(operationFail.Mensagens.Campos[0].Valor == estado);
            Assert.True(operationFail.Mensagens.Campos[0].Mensagem == "Estado não pode ser vazio.");
        }

        [Fact]
        public void Se_EstadoDoClienteMaiorQueDois_Entao_RetorneErro()
        {
            string estado = "123";

            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>())).Returns(true);

            var clienteValidation = new ClienteValidation(mockCpfValidation.Object);
            var operation = clienteValidation.Validar(new Cliente("nome", estado, "cpf"));
            var operationFail = operation as OperationFail<Cliente>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados do cliente.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 1);
            Assert.True(operationFail.Mensagens.Campos[0].Campo == "estado");
            Assert.True(operationFail.Mensagens.Campos[0].Valor == estado);
            Assert.True(operationFail.Mensagens.Campos[0].Mensagem == "Estado não pode ser maior que 2.");
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Se_CpfDoClienteNuloOuVazio_Entao_RetorneErro(string cpf)
        {

            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(It.IsAny<string>())).Returns(true);

            var clienteValidation = new ClienteValidation(mockCpfValidation.Object);
            var operation = clienteValidation.Validar(new Cliente("nome", "es", cpf));
            var operationFail = operation as OperationFail<Cliente>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados do cliente.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 1);
            Assert.True(operationFail.Mensagens.Campos[0].Campo == "cpf");
            Assert.True(operationFail.Mensagens.Campos[0].Valor == cpf);
            Assert.True(operationFail.Mensagens.Campos[0].Mensagem == "CPF não pode ser vazio.");
        }

        [Fact]
        public void Se_CpfInvalidoClienteNuloOuVazio_Entao_RetorneErro()
        {
            const string cpf = "123456789";
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(cpf)).Returns(false);

            var clienteValidation = new ClienteValidation(mockCpfValidation.Object);
            var operation = clienteValidation.Validar(new Cliente("nome", "es", cpf));
            var operationFail = operation as OperationFail<Cliente>;
            Assert.NotNull(operationFail);
            Assert.Equal("Houve um erro ao validar os dados do cliente.", operationFail.Mensagens.Mensagem);
            Assert.True(operationFail.Mensagens.Campos.Count == 1);
            Assert.True(operationFail.Mensagens.Campos[0].Campo == "cpf");
            Assert.True(operationFail.Mensagens.Campos[0].Valor == cpf);
            Assert.True(operationFail.Mensagens.Campos[0].Mensagem == "O CPF informado é invalido.");
        }

        [Fact]
        public void Se_ClienteValido_Entao_RetorneSucesso()
        {
            const string cpf = "123456789";
            var mockCpfValidation = new Mock<ICpfValidation>();
            mockCpfValidation.Setup(x => x.Validar(cpf)).Returns(true);

            var clienteValidation = new ClienteValidation(mockCpfValidation.Object);
            var operation = clienteValidation.Validar(new Cliente("nome", "es", cpf));
            var operationSucess = operation as OperationSuccess<Cliente>;
            Assert.NotNull(operationSucess);
            Assert.Null(operationSucess.Data);
        }
    }
}
