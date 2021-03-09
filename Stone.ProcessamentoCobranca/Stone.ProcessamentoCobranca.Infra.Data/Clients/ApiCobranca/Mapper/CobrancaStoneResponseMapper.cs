using Stone.ProcessamentoCobranca.Dominio.Entities;
using Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCobranca.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stone.ProcessamentoCobranca.Infra.Data.Clients.ApiCobranca.Mapper
{
    public static class CobrancaStoneResponseMapper
    {
        public static CobrancaStone ConverterCobrancaStoneResponseEmCobranca(CobrancaStoneResponse cobranca)
        {
            return new CobrancaStone(cobranca.Id, cobranca.DataVencimento, cobranca.Cpf, cobranca.ValorCobranca);
        }
    }
}
