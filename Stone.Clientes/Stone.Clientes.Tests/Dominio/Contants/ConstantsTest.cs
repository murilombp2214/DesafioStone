using Stone.Clientes.Dominio.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Stone.Clientes.Tests.Dominio.Contants
{
    public class ConstantsTest
    {
        [Fact]
        public void Se_TamanhoNomeMaiorQue300_Entao_GerarErro()
        {
            Assert.True(ClienteConstants.TAMANHO_MAX_NOME == 300);
        }

        [Fact]
        public void Se_TamanhoCpfMaiorQue11_Entao_GerarErro()
        {
            Assert.True(ClienteConstants.TAMANHO_CPF == 11);
        }

        [Fact]
        public void Se_TamanhoEstadoMaiorQue2_Entao_GerarErro()
        {
            Assert.True(ClienteConstants.TAMANHO_MAX_ESTADO == 2);
        }
    }
}
