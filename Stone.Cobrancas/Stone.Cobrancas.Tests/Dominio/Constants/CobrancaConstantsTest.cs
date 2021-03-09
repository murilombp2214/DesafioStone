using Stone.Cobrancas.Dominio.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Stone.Cobrancas.Tests.Dominio.Constants
{
    public class CobrancaConstantsTest
    {
        [Fact]
        public void Se_CpfDiferenteDe11_Entao_RetorneErro()
        {
            Assert.True(CobrancaConstants.TAMANHO_CPF == 11);
        }
    }
}
