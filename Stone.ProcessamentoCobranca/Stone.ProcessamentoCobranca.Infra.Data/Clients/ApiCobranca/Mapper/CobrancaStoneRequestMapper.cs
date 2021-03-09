using Stone.ProcessamentoCobranca.Dominio.Entities;
using Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCobranca.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCobranca.Mapper
{
    public static class CobrancaStoneRequestMapper
    {
        public static CobrancaStoneRequest ConverterCobrancaEmCobrancaStoneRequest(CobrancaStone cobranca)
        {
            return new CobrancaStoneRequest 
            {
                Cpf = cobranca.Cpf,
                DataVencimento = cobranca.DataVencimento,
                ValorCobranca = cobranca.ValorCobranca
            };
        }
    }
}
