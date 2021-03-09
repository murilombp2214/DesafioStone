using Stone.Cobrancas.Aplicacacao.Request;
using Stone.Cobrancas.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stone.Cobrancas.Aplicacacao.Mappers
{
    public static class CobrancaRequestMapper
    {
        public static Cobranca ConverterCobrancaRequestEmCobranca(CobrancaRequest request)
        {
            return new Cobranca(request.DataVencimento,request.Cpf,request.ValorCobranca);
        }
    }
}
