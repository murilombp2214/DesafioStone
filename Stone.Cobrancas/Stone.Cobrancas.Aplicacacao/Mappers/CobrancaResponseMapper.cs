using Stone.Cobrancas.Aplicacacao.Response;
using Stone.Cobrancas.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stone.Cobrancas.Aplicacacao.Mappers
{
    public static class CobrancaResponseMapper
    {
        public static CobrancaResponse ConverterCobrancaEmCobrancaResponse(Cobranca cobranca)
        {
            return new CobrancaResponse 
            {
                Cpf = cobranca.Cpf,
                DataVencimento = cobranca.DataVencimento,
                Id = cobranca.Id,
                ValorCobranca = cobranca.ValorCobranca
            };
        }
    }
}
